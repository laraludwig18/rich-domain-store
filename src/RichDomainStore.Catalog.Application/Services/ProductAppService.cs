using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RichDomainStore.Catalog.Application.Dtos;
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

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync().ConfigureAwait(continueOnCapturedContext: false);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<IEnumerable<ProductDto>> GetByCategoryCodeAsync(int code)
        {
            var products = await _productRepository.GetByCategoryCodeAsync(code).ConfigureAwait(continueOnCapturedContext: false);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id).ConfigureAwait(continueOnCapturedContext: false);
            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _productRepository.GetCategoriesAsync().ConfigureAwait(continueOnCapturedContext: false);
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<ProductDto> AddProductAsync(AddProductDto createProductDTO)
        {
            var product = _mapper.Map<Product>(createProductDTO);
            _productRepository.Add(product);

            await _productRepository.UnitOfWork.CommitAsync().ConfigureAwait(continueOnCapturedContext: false);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductDto updateProductDTO)
        {
            var product = await _productRepository.GetByIdAsync(id).ConfigureAwait(continueOnCapturedContext: false);
            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            var updatedProduct = _mapper.Map<Product>(updateProductDTO);
            _productRepository.Update(updatedProduct);

            await _productRepository.UnitOfWork.CommitAsync().ConfigureAwait(continueOnCapturedContext: false);
            return _mapper.Map<ProductDto>(updatedProduct);
        }

        public async Task<ProductDto> DebitStockAsync(Guid id, UpdateStockDto updateStockDTO)
        {
            if (!_stockService.DebitStockAsync(id, updateStockDTO.Quantity).Result)
            {
                throw new DomainException("Failed to debit stock");
            }

            var product = await _productRepository.GetByIdAsync(id).ConfigureAwait(continueOnCapturedContext: false);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> ReStockAsync(Guid id, UpdateStockDto updateStockDTO)
        {
            if (!_stockService.ReStockAsync(id, updateStockDTO.Quantity).Result)
            {
                throw new DomainException("Failed to restock");
            }

            var product = await _productRepository.GetByIdAsync(id).ConfigureAwait(continueOnCapturedContext: false);
            return _mapper.Map<ProductDto>(product);
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
            _stockService?.Dispose();
        }
    }
}