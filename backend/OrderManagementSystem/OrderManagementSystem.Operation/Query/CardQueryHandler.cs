using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation.Query
{
    public class CardQueryHandler :
        IRequestHandler<GetAllCardQuery, ApiResponse<List<CardResponse>>>,
        IRequestHandler<GetCardByIdQuery, ApiResponse<CardResponse>>,
        IRequestHandler<GetCardByUserIdQuery, ApiResponse<List<CardResponse>>>
    {
        private readonly OmsDbContext dbContext;
        private readonly IMapper mapper;

        public CardQueryHandler(OmsDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<List<CardResponse>>> Handle(GetAllCardQuery request,
            CancellationToken cancellationToken)
        {
            List<Card> list = await dbContext.Set<Card>().Include(x => x.User).ToListAsync(cancellationToken);

            List<CardResponse> mapped = mapper.Map<List<CardResponse>>(list);
            return new ApiResponse<List<CardResponse>>(mapped);
        }

        public async Task<ApiResponse<CardResponse>> Handle(GetCardByIdQuery request,
            CancellationToken cancellationToken)
        {
            Card? entity = await dbContext.Set<Card>().Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return new ApiResponse<CardResponse>("Record not found");
            }

            CardResponse mapped = mapper.Map<CardResponse>(entity);
            return new ApiResponse<CardResponse>(mapped);
        }

        public async Task<ApiResponse<List<CardResponse>>> Handle(GetCardByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            List<Card> list = await dbContext.Set<Card>().Include(x => x.User)
                .Where(x => x.UserId == request.UserId).ToListAsync(cancellationToken);

            List<CardResponse> mapped = mapper.Map<List<CardResponse>>(list);
            return new ApiResponse<List<CardResponse>>(mapped);
        }
    }
}
