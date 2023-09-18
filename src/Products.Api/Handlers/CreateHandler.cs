using AutoMapper;
using MediatR;
using Products.Api.Models;
using Products.Data;
using Products.Data.Persistence.Entities;

namespace Products.Api.Handlers
{
    public class CreateHandler : IRequestHandler<CreateProductRequest, CreateProductResponse>
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _repository;

        public CreateHandler(IProductsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken token)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return _mapper.Map<CreateProductResponse>(await _repository.Add(_mapper.Map<Product>(request)));
        }
    }
}