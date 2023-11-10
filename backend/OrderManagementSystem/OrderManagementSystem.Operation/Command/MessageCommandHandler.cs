using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Schema;

namespace OrderManagementSystem.Operation.Command;


    public class MessageCommandHandler :
        IRequestHandler<CreateMessageCommand, ApiResponse<MessageResponse>>,
        IRequestHandler<UpdateMessageCommand, ApiResponse>,
        IRequestHandler<DeleteMessageCommand, ApiResponse>
{
    private readonly OmsDbContext dbContext;
    private readonly IMapper mapper;

    public MessageCommandHandler(OmsDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<MessageResponse>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var mapped = mapper.Map<Message>(request.Model);
            mapped.IsActive = true;

            User user = await dbContext.Set<User>()
                .FirstOrDefaultAsync(x => x.UserId == request.Model.ChatId && x.IsActive, cancellationToken);

            if (user == null || string.IsNullOrWhiteSpace(user.Email))
            {
                return new ApiResponse<MessageResponse>("Error: User not found or user email is null for the provided ChatId.");
            }

            mapped.Email = user.Email;

            var entityEntry = await dbContext.Set<Message>().AddAsync(mapped, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = mapper.Map<MessageResponse>(entityEntry.Entity);
            response.Email = user.Email;
            return new ApiResponse<MessageResponse>(response);
        }
        catch (DbUpdateException ex)
        {
            return new ApiResponse<MessageResponse>($"Error: An error occurred while saving the entity changes. {ex.InnerException?.Message}");
        }
        catch (Exception ex)
        {
            return new ApiResponse<MessageResponse>($"Error: {ex.Message}");
        }
    }


    public async Task<ApiResponse> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<Message>().FirstOrDefaultAsync(x => x.ChatId == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }

        entity.Content = request.Model.Content;

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<Message>().FirstOrDefaultAsync(x => x.ChatId == request.Id, cancellationToken);
        if (entity == null)
        {
            return new ApiResponse("Record not found!");
        }

        entity.IsActive = false;
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}

