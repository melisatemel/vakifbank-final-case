namespace OrderManagementSystem.Schema;

public class MessageRequest
{
    public int ChatId { get; set; }
    public string Content { get; set; }
    public bool? IsAdmin { get; set; }
}

public class MessageResponse
{
    public int ChatId { get; set; }
    public string Content { get; set; }
    public bool IsAdmin { get; set; }
    public string Email { get; set; }
}

public class AdminMessageResponse
{
    public int ChatId { get; set; }
    public List<MessageResponse> Messages { get; set; }
    public string Email { get; set; }
}