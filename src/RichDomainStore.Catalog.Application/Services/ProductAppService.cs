using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RichDomainStore.Catalog.Application.Dtos;
using RichDomainStore.Catalog.Domain;
using RichDomainStore.Core.DomainObjects;

namespace RichDomainStore.Catalog.Application.Services
{
    public class ProductAppService : IProductAppService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public ProductAppService(
            IProductRepository productRepository,
            IMapper mapper,
            IStockService stockService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _stockService = stockService;
        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var products = await _productRepository.GetAll().ConfigureAwait(false);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<IEnumerable<ProductDTO>> GetByCategoryCode(int code)
        {
            var products = await _productRepository.GetByCategoryCode(code).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetById(Guid id)
        {
            var product = await _productRepository.GetById(id).ConfigureAwait(false);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategories()
        {
            var categories = await _productRepository.GetCategories().ConfigureAwait(false);
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task AddProduct(ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            _productRepository.Add(product);

            await _productRepository.UnitOfWork.Commit().ConfigureAwait(false);
        }

        public async Task UpdateProduct(ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            _productRepository.Update(product);

            await _productRepository.UnitOfWork.Commit().ConfigureAwait(false);
        }

        public async Task<ProductDTO> DebitStock(Guid id, int quantity)
        {
            if (!_stockService.DebitStock(id, quantity).Result)
            {
                throw new DomainException("Failed to debit stock");
            }

            var product = await _productRepository.GetById(id).ConfigureAwait(false);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> ReStock(Guid id, int quantity)
        {
            if (!_stockService.ReStock(id, quantity).Result)
            {
                throw new DomainException("Failed to restock");
            }

            var product = await _productRepository.GetById(id).ConfigureAwait(false);
            return _mapper.Map<ProductDTO>(product);
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
            _stockService?.Dispose();
        }
    }
}