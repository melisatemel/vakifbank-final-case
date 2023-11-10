namespace OrderManagementSystem.Schema;

public class UserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
}

public class UserResponse
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
    public decimal ProfitMargin { get; set; }
    public virtual List<AddressResponse> Addresses { get; set; }
    public virtual List<CardResponse> Cards { get; set; }
}
