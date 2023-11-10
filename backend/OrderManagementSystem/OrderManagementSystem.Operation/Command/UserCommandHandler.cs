using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Base;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Schema;
using static OrderManagementSystem.Operation.Cqrs.UserCqrs;

namespace OrderManagementSystem.Operation.Command;

public class UserCommandHandler :
    IRequestHandler<CreateUserCommand, ApiResponse<UserResponse>>,
    IRequestHandler<UpdateUserCommand, ApiResponse>,
    IRequestHandler<DeleteUserCommand, ApiResponse>

{
    private readonly OmsDbContext dbContext;
    private readonly IMapper mapper;

    public UserCommandHandler(OmsDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }


    public async Task<ApiResponse<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var md5 = Md5.Create(request.Model.Password.ToUpper());
        request.Model.Password = md5;
        User mapped = mapper.Map<User>(request.Model);

        var entity = await dbContext.Set<User>().AddAsync(mapped, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<UserResponse>(entity.Entity);
        return new ApiResponse<UserResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<User>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }
        entity.FirstName = request.Model.FirstName;
        entity.LastName = request.Model.LastName;

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<User>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }

        entity.IsActive = false;
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}
