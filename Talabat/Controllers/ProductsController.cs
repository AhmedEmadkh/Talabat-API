using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        #region Services
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;

        public ProductsController(
            IGenericRepository<Product> ProductRepo,
            IMapper mapper,
            IGenericRepository<ProductType> ProductTypeRepo,
            IGenericRepository<ProductBrand> ProductBrandRepo
            )
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
            _productTypeRepo = ProductTypeRepo;
            _productBrandRepo = ProductBrandRepo;
        }
		#endregion

		// GetAll Products
		[Authorize]
		[HttpGet] // BaseUrl/api/Products
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
            var Spec = new ProductWithBrandTypeSpecifications(Params);
            var Products = await _productRepo.GetAllWithSpecifications(Spec);
            var MappedProducts = _mapper.Map<IReadOnlyList<ProductToReturnDTO>>(Products);
            var CountSpec = new ProductWithFiltirationForCountAsync(Params);
            var Count = await _productRepo.GetCountWithSpecAsync(CountSpec);
            var PaginatedProducts = new Pagination<ProductToReturnDTO>
            {
                PageIndex = Params.PageIndex,
                PageSize = Params.PageSize,
                Count = Count,
                Data = MappedProducts
            };

            return Ok(PaginatedProducts);
        }

        // Get Product By Id
        [Authorize]
        [HttpGet("{id}")] // BaseUrl/api/Products/{id}
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var Spec = new ProductWithBrandTypeSpecifications(id);
            var Product = await _productRepo.GetByIdWithSpecifications(Spec);
            if (Product is null) return NotFound(new APIResponse(404));
            var MappedProducts = _mapper.Map<ProductToReturnDTO>(Product);
            return Ok(MappedProducts);
        }

        // Get Product Types
        [HttpGet("Types")] // BaseUrl/api/Products/Types
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types = await _productTypeRepo.GetAllAsync();

            return Ok(Types);
        }

        // Get Product Types

        [HttpGet("Brands")] // BaseUrl/api/Products/Brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _productBrandRepo.GetAllAsync();
            return Ok(Brands);
        }
    }
}
