﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared;

namespace Presentation
{
    // Api Controller 
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        // EndPoint : Public non-Static Method 

        // Sort : NameAsec [default]
        // Sort : NameDesec
        // Sort : PriceAsec
        // Sort : PriceDesec
        [HttpGet] // Get : /api/Products
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductSpecificationsParamters specParams)
        {
            var result = await serviceManager.ProductService.GetAllProductsAsync(specParams);
            if (result is null) return BadRequest(); // 400
            return Ok(result);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await serviceManager.ProductService.GetProductByIdAsync (id);
            if (result is null) return BadRequest();  //400
            return Ok(result);
        }


        // TODO : Get All Brands 
        [HttpGet("brands")]  // Get : /api/Products/brands
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await serviceManager.ProductService.GetAllBrandsAsync();
            if (result == null || !result.Any())
                return NotFound(); // 404
            return Ok(result);
        }


        // TODO : Get All Types 
        [HttpGet("types")]  // Get : /api/Products/types
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();
            if (result == null || !result.Any())
                return NotFound(); // 404
            return Ok(result);
        }

    }
}
