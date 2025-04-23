using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using ServicesAbstractions;

namespace Services
{
    public class ServiceManager
        (IBasketRepository basketRepository,
        ICacheRepository cacheRepository,
        IUnitOfWork unitOfWork ,
        UserManager<AppUser> userManager,
        IMapper mapper) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(unitOfWork,mapper);

        public IBasketService basketService { get; } = new BasketService(basketRepository, mapper);

        public ICacheService CacheService { get; } = new CacheService(cacheRepository);

        public IAuthService AuthService { get; } = new AuthService(userManager);

    }
}
