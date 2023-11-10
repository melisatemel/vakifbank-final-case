using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation
{
    public class AddressQueryHandler :
        IRequestHandler<GetAllAddressQuery, ApiResponse<List<AddressResponse>>>,
        IRequestHandler<GetAddressByIdQuery, ApiResponse<AddressResponse>>,
        IRequestHandler<GetAddressByCustomerIdQuery, ApiResponse<List<AddressResponse>>>
    {
        private readonly OmsDbContext dbContext;
        private readonly IMapper mapper;

        public AddressQueryHandler(OmsDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<List<AddressResponse>>> Handle(GetAllAddressQuery request,
            CancellationToken cancellationToken)
        {
            List<Address> list = await dbContext.Set<Address>().Include(x => x.User).ToListAsync(cancellationToken);

            List<AddressResponse> mapped = mapper.Map<List<AddressResponse>>(list);
            return new ApiResponse<List<AddressResponse>>(mapped);
        }

        public async Task<ApiResponse<AddressResponse>> Handle(GetAddressByIdQuery request,
            CancellationToken cancellationToken)
        {
            Address? entity = await dbContext.Set<Address>().Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return new ApiResponse<AddressResponse>("Record not found");
            }

            AddressResponse mapped = mapper.Map<AddressResponse>(entity);
            return new ApiResponse<AddressResponse>(mapped);
        }

        public async Task<ApiResponse<List<AddressResponse>>> Handle(GetAddressByCustomerIdQuery request,
            CancellationToken cancellationToken)
        {
            List<Address> list = await dbContext.Set<Address>().Include(x => x.User)
                .Where(x => x.UserId == request.UserId).ToListAsync(cancellationToken);

            List<AddressResponse> mapped = mapper.Map<List<AddressResponse>>(list);
            return new ApiResponse<List<AddressResponse>>(mapped);
        }
    }
}
