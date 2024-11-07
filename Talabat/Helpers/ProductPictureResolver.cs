using AutoMapper;
using AutoMapper.Execution;
using Talabat.APIs.DTOs;
using Talabat.Core.Entites;

namespace Talabat.APIs.Helpers
{
    public class ProductPictureResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["APIBaseUrl"]}/{source.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
