﻿using MediatR;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation
{
    public record CreateAddressCommand(AddressRequest Model) : IRequest<ApiResponse<AddressResponse>>;
    public record UpdateAddressCommand(AddressRequest Model, int Id) : IRequest<ApiResponse>;
    public record DeleteAddressCommand(int Id) : IRequest<ApiResponse>;
    public record GetAllAddressQuery() : IRequest<ApiResponse<List<AddressResponse>>>;
    public record GetAddressByIdQuery(int Id) : IRequest<ApiResponse<AddressResponse>>;
    public record GetAddressByCustomerIdQuery(int UserId) : IRequest<ApiResponse<List<AddressResponse>>>;
}
