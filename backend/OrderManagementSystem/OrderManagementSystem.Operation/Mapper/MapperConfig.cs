using AutoMapper;
using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<UserRequest, User>();
        CreateMap<User, UserResponse>();

        CreateMap<ProductRequest, Product>();
        CreateMap<Product, ProductResponse>();

        CreateMap<ShoppingCartRequest, ShoppingCart>()
            .ForMember(dest => dest.Products, opt => opt.Ignore());

        CreateMap<int, Product>().ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src));


        CreateMap<ShoppingCart, ShoppingCartResponse>()
            .ForMember(dest => dest.ProductQuantities, opt => opt.MapFrom(src => src.ProductQuantities));

        CreateMap<AddressRequest, Address>();
        CreateMap<Address, AddressResponse>()
            .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));

        CreateMap<CardRequest, Card>();
        CreateMap<Card, CardResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

        CreateMap<MessageRequest, Message>();
        CreateMap<Message, MessageResponse>();
    }
}