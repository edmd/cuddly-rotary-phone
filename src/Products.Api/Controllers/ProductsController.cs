using AutoMapper;
using Humanizer.Localisation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Models;
using Products.Api.Services;
using System.Net;

namespace Products.Api.Controllers
{
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ApiController]
    [Authorize]
    public class ProductsController : ApiControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductsController(ILogger<ProductsController> logger, IMapper mapper, IMediator mediator, 
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Produces(typeof(CreateProductResponse))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            if (request == null) {
                return BadRequest();
            }

            var response = await _mediator.Send(request);

            _logger.LogInformation(response.ToString());

            return Created($"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}{_httpContextAccessor.HttpContext?.Request.Path}/{response}", 
                response);
        }

        /// Sample query string:
        /// 
        ///     ?filter=username+eq+"foo"
        ///    
        [HttpGet("{sku}")]
        [Produces(typeof(GetProductResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(IResourceQuery resourceQuery)
        {
            if (
            resourceQuery.Filters.Count != 1 ||
            resourceQuery.Filters.First().AdditionalFilter != null)
            {
                return ScimError(
                    HttpStatusCode.BadRequest,
                    Resources.UserSearch_WrongFilterNumberError,
                    ErrorType.invalidFilter
                );
            }

            var filter = resourceQuery.Filters.First();
            if (filter.FilterOperator != ComparisonOperator.Equals)
            {
                return ScimError(
                    HttpStatusCode.BadRequest,
                    string.Format(Resources.UserSearch_WrongFilterOperatorError, ComparisonOperator.Equals),
                    ErrorType.invalidFilter
                );
            }

            var (isFilterValid, users) = await TryGetUsers(filter);
            if (!isFilterValid)
            {
                return ScimError(
                    HttpStatusCode.BadRequest,
                    string.Format(Resources.UserSearch_WrongFilterAttributeError, $"{AttributeNames.UserName}, {AttributeNames.ExternalIdentifier} or {AttributeNames.ElectronicMailAddresses}"),
                    ErrorType.invalidFilter
                );
            }

            var resources = _mapper.Map<GetUserModel[], Core2EnterpriseUser[]>(users);
            var response = new QueryResponse()
            {
                Resources = resources,
                TotalResults = resources.Length,
                ItemsPerPage = resources.Length
            };

            return Ok(response);



            var response = _mapper.Map<GetProductResponse>(
                await _mediator.Send(new GetProductRequest(sku)));

            _logger.LogInformation(response.ToString());
            return Ok(response);
        }

        [HttpGet()]
        [Produces(typeof(GetProductResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var response = _mapper.Map<GetProductResponse>(
                await _mediator.Send(new GetAllProductsRequest()));

            _logger.LogInformation(response.ToString());
            return Ok(response);
        }

        [HttpPatch()]
        [Produces(typeof(GetProductResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(int sku, [FromBody] JsonPatchDocument<PatchProductRequest> request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var response = await _mediator.Send(request);

            _logger.LogInformation(response.ToString());

            return Created($"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}{_httpContextAccessor.HttpContext?.Request.Path}/{response}",
                response);
        }
    }
}