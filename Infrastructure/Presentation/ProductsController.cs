using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;

namespace Presentation
{
    // Api Controller 
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        // EndPoint : Public non-Static Method 

        [HttpGet] // Get : /api/Products
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await serviceManager.ProductService.GetAllProductsAsync();
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
        [HttpGet("brands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await serviceManager.ProductService.GetAllBrandsAsync();
            if (result == null || !result.Any())
                return NotFound(); // 404
            return Ok(result);
        }


        // TODO : Get All Types 
        [HttpGet("types")] 
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();
            if (result == null || !result.Any())
                return NotFound(); // 404
            return Ok(result);
        }

    }
}
