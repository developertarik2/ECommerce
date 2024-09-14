using AutoMapper;
using ECommerce.Dtos;
using ECommerce.Models.Entities;
using ECommerce.Models.Entities.OrderAggregate;
//using Core.Entities.Identity;

namespace ECommerce.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductUrlResolver>());
            CreateMap<ECommerce.Models.Entities.Identity.UserAddress, AddressDto>().ReverseMap();
            CreateMap<CustomerCartDto, CustomerCart>();
            CreateMap<CartItemDto, CartItem>();
            CreateMap<AddressDto, ECommerce.Models.Entities.OrderAggregate.Address>();
            CreateMap<Order, OrderToReturnDto>()
                 .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                 .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
        }
    }
}
