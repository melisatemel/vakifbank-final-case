namespace OrderManagementSystem.Schema;

public class CardRequest
{
    public int UserId { get; set; }
    public string CardHolder { get; set; }
    public long CardNumber { get; set; }
    public string Cvv { get; set; } // nnn
    public string ExpiryDate { get; set; } // DDyy
    public int? ExpenseLimit { get; set; }
}

public class CardResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string CardHolder { get; set; }
    public long CardNumber { get; set; }
    public string Cvv { get; set; } // nnn
    public string ExpiryDate { get; set; } // DDyy
    public int? ExpenseLimit { get; set; }
    public string Name { get; set; }
}