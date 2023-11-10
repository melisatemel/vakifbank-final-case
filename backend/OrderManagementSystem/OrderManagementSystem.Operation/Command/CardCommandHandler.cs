using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation.Command
{
    public class CardCommandHandler :
        IRequestHandler<CreateCardCommand, ApiResponse<CardResponse>>,
        IRequestHandler<UpdateCardCommand, ApiResponse>,
        IRequestHandler<DeleteCardCommand, ApiResponse>
    {
        private readonly OmsDbContext dbContext;
        private readonly IMapper mapper;

        public CardCommandHandler(OmsDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<CardResponse>> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            Card mapped = mapper.Map<Card>(request.Model);
            mapped.IsActive = true;
            var entity = await dbContext.Set<Card>().AddAsync(mapped, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var response = mapper.Map<CardResponse>(entity.Entity);
            return new ApiResponse<CardResponse>(response);
        }

        public async Task<ApiResponse> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Set<Card>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                return new ApiResponse("Record not found!");
            }

            entity.CardHolder = request.Model.CardHolder;

            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Set<Card>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
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
