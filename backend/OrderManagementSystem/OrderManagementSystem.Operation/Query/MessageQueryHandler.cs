using AutoMapper;
using MediatR;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Schema;
using Microsoft.EntityFrameworkCore;

namespace OrderManagementSystem.Operation.Query;

public class MessageQueryHandler :
    IRequestHandler<GetAllMessageQuery, ApiResponse<List<MessageResponse>>>,
    IRequestHandler<GetMessageByIdQuery, ApiResponse<List<MessageResponse>>>,
    IRequestHandler<GetAdminMessagesQuery, ApiResponse<List<AdminMessageResponse>>>
{
    private readonly OmsDbContext dbContext;
    private readonly IMapper mapper;

    public MessageQueryHandler(OmsDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<MessageResponse>>> Handle(GetAllMessageQuery request,
        CancellationToken cancellationToken)
    {
        List<Message> list = await dbContext.Set<Message>().ToListAsync(cancellationToken);

        List<MessageResponse> mapped = mapper.Map<List<MessageResponse>>(list);
        return new ApiResponse<List<MessageResponse>>(mapped);
    }

    public async Task<ApiResponse<List<MessageResponse>>> Handle(GetMessageByIdQuery request,
CancellationToken cancellationToken)
    {
        List<Message> list = await dbContext.Set<Message>().Where(x => x.ChatId == request.Id).OrderBy(x => x.InsertDate).ToListAsync(cancellationToken);

        List<MessageResponse> mapped = mapper.Map<List<MessageResponse>>(list);
        return new ApiResponse<List<MessageResponse>>(mapped);
    }

    public async Task<ApiResponse<List<AdminMessageResponse>>> Handle(GetAdminMessagesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            List<Message> adminMessages = await dbContext.Messages
             .OrderBy(x => x.InsertDate).ToListAsync(cancellationToken);

            var groupedMessages = adminMessages
                .GroupBy(x => x.ChatId)
                .Select(group => new AdminMessageResponse
                {
                    ChatId = group.Key,
                    Email = group.First().Email,
                    Messages = mapper.Map<List<MessageResponse>>(group.ToList())
                })
                .ToList();

            return new ApiResponse<List<AdminMessageResponse>>(groupedMessages);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<AdminMessageResponse>>($"Error: {ex.Message}");
        }
    }
}
