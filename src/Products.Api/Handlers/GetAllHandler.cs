using AutoMapper;
using MediatR;
using Products.Api.Models;
using Products.Data;

namespace Products.Api.Handlers
{
    public class GetAllHandler : IRequestHandler<GetAllProductsRequest, List<GetProductResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _repository;

        public GetAllHandler(IProductsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<GetProductResponse>> Handle(GetAllProductsRequest request, CancellationToken token)
        {
            return _mapper.Map<List<GetProductResponse>>(await _repository.GetAll());
        }
    }
}