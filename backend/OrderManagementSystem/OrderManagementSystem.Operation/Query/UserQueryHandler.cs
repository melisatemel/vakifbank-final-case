using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Schema;
using static OrderManagementSystem.Operation.Cqrs.UserCqrs;

namespace OrderManagementSystem.Operation.Query;

public class UserQueryHandler :

    IRequestHandler<GetAllUserQuery, ApiResponse<List<UserResponse>>>,
    IRequestHandler<GetUserByIdQuery, ApiResponse<UserResponse>>
{

    private readonly OmsDbContext dbContext;
    private readonly IMapper mapper;

    public UserQueryHandler(OmsDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<UserResponse>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        List<User> list = await dbContext.Set<User>()
            .ToListAsync(cancellationToken);

        List<UserResponse> mapped = mapper.Map<List<UserResponse>>(list);
        return new ApiResponse<List<UserResponse>>(mapped);
    }

    public async Task<ApiResponse<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        User entity = await dbContext.Set<User>()
            .Include(x => x.Addresses.Where(a => a.IsActive))
            .Include(x => x.Cards.Where(a => a.IsActive))
            .FirstOrDefaultAsync(x => x.UserId == request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<UserResponse>("Record not found!" + request);
        }

        UserResponse mapped = mapper.Map<UserResponse>(entity);
        return new ApiResponse<UserResponse>(mapped);
    }
}
