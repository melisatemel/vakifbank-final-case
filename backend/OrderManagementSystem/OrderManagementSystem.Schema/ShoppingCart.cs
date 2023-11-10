using OrderManagementSystem.Data.Domain;

namespace OrderManagementSystem.Schema;

public class ShoppingCartRequest
{
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public List<int> ProductIds { get; set; }
    public bool isMinus { get; set; }    
    public bool isDelete { get; set; }
    public int SelectedAddressId { get; set; }
    public int SelectedCardId { get; set; }
    public bool WaitForPayment { get; set; }
}

public class ShoppingCartUpdateRequest
{
    public bool? isActive { get; set; }
    public bool? IsCompleted { get; set; }
    public int? SelectedAddressId { get; set; }
    public int? SelectedCardId { get; set; }
    public bool? WaitForPayment { get; set; }
    public bool? OpenAccount { get; set; }
}

public class ShoppingCartResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsActive { get; set; }
    public bool IsCanceled { get; set; }
    public bool WaitForPayment { get; set; }
    public List<ProductQuantity> ProductQuantities { get; set; }
}

