using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IOrderService, OrderService>();
            #region Error Handling
            Services.Configure<ApiBehaviorOptions>(Options =>
               {
                   Options.InvalidModelStateResponseFactory = (actionContext) =>
                   {
                       var errors = actionContext.ModelState.Where(P => P.Value?.Errors.Count > 0)
                                                 .SelectMany(P => P.Value.Errors)
                                                 .Select(P => P.ErrorMessage).ToList();

                       var ValidatationErrorResponse = new APIValidationErrorResponse()
                       {
                           Error = errors
                       };
                       return new BadRequestObjectResult(ValidatationErrorResponse);
                   };
               }); 
            #endregion

            return Services;
        }
    }
}
