using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RichDomainStore.Catalog.Application.DTOS;
using RichDomainStore.Catalog.Domain.Entities;
using RichDomainStore.Catalog.Domain.Interfaces;
using RichDomainStore.Core.DomainObjects;
using RichDomainStore.Core.Exceptions;

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

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync().ConfigureAwait(false);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<IEnumerable<ProductDTO>> GetByCategoryCodeAsync(int code)
        {
            var products = await _productRepository.GetByCategoryCodeAsync(code).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (product == null) 
            {
                throw new NotFoundException("Product not found");
            }

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            var categories = await _productRepository.GetCategoriesAsync().ConfigureAwait(false);
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<ProductDTO> AddProductAsync(AddProductDTO createProductDTO)
        {
            var product = _mapper.Map<Product>(createProductDTO);
            _productRepository.Add(product);

            await _productRepository.UnitOfWork.CommitAsync().ConfigureAwait(false);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> UpdateProductAsync(UpdateProductDTO updateProductDTO)
        {
            var product = _mapper.Map<Product>(updateProductDTO);
            _productRepository.Update(product);

            await _productRepository.UnitOfWork.CommitAsync().ConfigureAwait(false);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> DebitStockAsync(Guid id, UpdateStockDTO updateStockDTO)
        {
            if (!_stockService.DebitStockAsync(id, updateStockDTO.Quantity).Result)
            {
                throw new DomainException("Failed to debit stock");
            }

            var product = await _productRepository.GetByIdAsync(id).ConfigureAwait(false);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> ReStockAsync(Guid id, UpdateStockDTO updateStockDTO)
        {
            if (!_stockService.ReStockAsync(id, updateStockDTO.Quantity).Result)
            {
                throw new DomainException("Failed to restock");
            }

            var product = await _productRepository.GetByIdAsync(id).ConfigureAwait(false);
            return _mapper.Map<ProductDTO>(product);
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
            _stockService?.Dispose();
        }
    }
}