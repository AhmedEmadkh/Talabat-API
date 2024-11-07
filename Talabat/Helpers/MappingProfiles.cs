using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Product
            CreateMap<Product, ProductToReturnDTO>()
        .ForMember(d => d.ProductType, O => O.MapFrom(S => S.ProductType.Name))
        .ForMember(d => d.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
        .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureResolver>()); 
            #endregion


            #region Address
            CreateMap<Core.Entites.Identity.Address, AddressDTO>().ReverseMap();
            CreateMap<AddressDTO, Core.Entites.Order_Aggregate.Address>(); 
            #endregion


            #region Basket
            CreateMap<CustomerBasketDTO, CustomerBasket>();
            CreateMap<BasketItemDTO, BasketItem>(); 
            #endregion

            #region Order
            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(d => d.ProductUrl, O => O.MapFrom(S => S.Product.ProductUrl))
                .ForMember(d => d.ProductUrl,O => O.MapFrom<OrderItemPictureURLResolver>()); 
            #endregion

        }
    }
}
