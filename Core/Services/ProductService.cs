using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using ServicesAbstractions;
using Shared;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork,IMapper mapper) : IProductService
    {
        
        public async Task<IEnumerable<ProductResultDto>> GetAllProductsAsync()
        {
            // Get All Products Though The Product Repository :

             var products =  await unitOfWork.GetRepository<Product,int>().GetAllAsync();

            // Mapping From IEnumerable<Product> To IEnumerable<ProductResultDto> : AutoMapper 
             var result = mapper.Map<IEnumerable<ProductResultDto>>(products);

            return result;
        }

        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var products = await unitOfWork.GetRepository<Product,int>().GetAsync(id);
            if (products is null) return null;
            var result = mapper.Map<ProductResultDto>(products);
            return result;
        }

        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            // Get All Brands Though The Brands Repository :

            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();

            // Mapping From IEnumerable<ProductBrand> To IEnumerable<BrandResultDto> : AutoMapper 
            var result = mapper.Map<IEnumerable<BrandResultDto>>(brands);

            return result;
        }

      
        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            // Get All Types Though The Types Repository :
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            // Mapping :
            var result = mapper.Map<IEnumerable<TypeResultDto>>(types);
            return result;

        }


    }
}
