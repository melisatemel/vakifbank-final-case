using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation.Command
{
    public class AddressCommandHandler :
        IRequestHandler<CreateAddressCommand, ApiResponse<AddressResponse>>,
        IRequestHandler<UpdateAddressCommand, ApiResponse>,
        IRequestHandler<DeleteAddressCommand, ApiResponse>
    {
        private readonly OmsDbContext dbContext;
        private readonly IMapper mapper;

        public AddressCommandHandler(OmsDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<AddressResponse>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            Address mapped = mapper.Map<Address>(request.Model);
            mapped.IsActive = true;
            var entity = await dbContext.Set<Address>().AddAsync(mapped, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var response = mapper.Map<AddressResponse>(entity.Entity);
            return new ApiResponse<AddressResponse>(response);
        }

        public async Task<ApiResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Set<Address>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                return new ApiResponse("Record not found!");
            }

            entity.AddressLine1 = request.Model.AddressLine1;

            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Set<Address>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                return new ApiResponse("Record not found!");
            }

            entity.IsActive = false;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }
    }
}
