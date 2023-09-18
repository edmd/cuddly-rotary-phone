using AutoMapper;
using MediatR;
using Products.Api.Models;
using Products.Data;

namespace Products.Api.Handlers
{
    public class GetByNameHandler : IRequestHandler<GetProductRequest, GetProductResponse>
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _repository;

        public GetByNameHandler(IProductsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetProductResponse> Handle(GetProductRequest request, CancellationToken token)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return _mapper.Map<GetProductResponse>(await _repository.Get(request.Name));
        }
    }
}