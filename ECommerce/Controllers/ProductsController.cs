using AutoMapper;
using ECommerce.Dtos;
using ECommerce.Errors;
using ECommerce.Helpers;
using ECommerce.Infrastructure.Data.Interfaces;
using ECommerce.Infrastructure.Specifications;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IMapper _mapper;
        public ProductsController(IUnitOfWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }
        //[HttpGet]
        //public async Task<ActionResult<Product>> GetProducts()
        //{
        //    var product =await _unitofWork.Product.GetAll(isPaging: true, pageIndex: 2, pageSize: 4);
        //    return Ok(product);
        //}
      // [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            //var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            //var countSpec = new ProductWithFiltersForCountSpecificication(productParams);
            //  var totalItems = await _productsRepo.CountAsync(countSpec);
            Expression<Func<Product, bool>> filter = x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.BrandId.HasValue || x.BrandId == productParams.BrandId) &&
                (!productParams.TypeId.HasValue || x.CategoryId == productParams.TypeId);

            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy;
            orderBy = u => u.OrderBy(u => u.Name);


            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                       // products.OrderBy(p => p.Price);
                        orderBy = u => u.OrderBy(u => u.Price);
                        break;
                    case "priceDesc":
                        // products.OrderByDescending(p => p.Price);
                        orderBy = u => u.OrderByDescending(u => u.Price);
                        break;
                    default:
                        // products.OrderBy(n => n.Name);
                        orderBy = u => u.OrderBy(u => u.Name);
                        break;
                }
            }
            var products = await _unitofWork.Product.GetAll(filter: filter,
                orderBy: orderBy, includeProperties: "Category,Brand", isPaging: true,
                pageIndex: productParams.PageIndex, pageSize: productParams.PageSize);

            var productsCount = await _unitofWork.Product.GetAll(filter: filter,
                orderBy: orderBy, includeProperties: "Category,Brand");
            //var products =await _unitofWork.Product.GetAll(includeProperties: "Brand,Category", orderBy: u => u.OrderBy(u => u.Name),
            //    isPaging:true, pageIndex:productParams.PageIndex,pageSize:productParams.PageSize);
            // products= products.OrderBy(p => p.Name).ToList();
            // var product = await _productsRepo.ListAsync(spec);  
            var data = _mapper
                 .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
           // return Ok( _mapper.Map<IReadOnlyList< Product>,IReadOnlyList< ProductToReturnDto>>(products));
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, productsCount.Count, data));
        }

       // [Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            // return View();
         //   var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _unitofWork.Product.GetFirstOrDefault(x=>x.Id==id, includeProperties: "Category,Brand");
            if (product == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<Product, ProductToReturnDto>(product);
            //return Ok(product);
        }
       // [Cached(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<Brand>>> GetProductBrands()
        {

            var brands = await _unitofWork.Brand.GetAll();
            return Ok(brands);
        }
      //  [Cached(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetProductTypes()
        {

            var categories = await _unitofWork.Category.GetAll(orderBy: u => u.OrderBy(u => u.Id));
            return Ok(categories);
        }
    }
}
