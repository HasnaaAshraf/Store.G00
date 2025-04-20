using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;
using Shared;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
 
    public class BasketController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]  // Get : /api/Baskets/?id=eaf
        public async Task<IActionResult> GetBasketById(string id)
        {
           var result =  await serviceManager.basketService.GetBasketAsync(id);
            return Ok(result);
        }

        [HttpPost]  // Post : /api/Baskets
        public async Task<IActionResult> UpdateBasketById(BasketDto basketDto )
        {
            var result = await serviceManager.basketService.UpdateBasketAsync(basketDto);
            return Ok(result);
        }

        [HttpDelete] // Delete : /api/Baskets?id
        public async Task<IActionResult> DeleteBasketById(string id)
        {
            var result = await serviceManager.basketService.DeleteBasketAsync(id);
            return NoContent(); // 204
        }

    }
}
