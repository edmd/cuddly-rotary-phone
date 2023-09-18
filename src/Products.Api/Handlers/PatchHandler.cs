using AutoMapper;
using MediatR;
using Products.Api.Models;
using Products.Data;
using Products.Data.Persistence.Entities;

namespace Products.Api.Handlers
{
    public class PatchHandler : IRequestHandler<PatchProductRequest, PatchProductResponse>
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _repository;

        public PatchHandler(IProductsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PatchProductResponse> Handle(PatchProductRequest request, CancellationToken token)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return _mapper.Map<PatchProductResponse>(await _repository.Update(_mapper.Map<Product>(request)));
        }
    }
}