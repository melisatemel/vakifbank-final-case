namespace OrderManagementSystem.Schema
{
    public class OrderReportDto
    {
        public int ShoppingCartId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsCompleted { get; set; }
        public decimal TotalPrice { get; set; }
        public int SelectedAddressId { get; set; }
        public int SelectedCardId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
