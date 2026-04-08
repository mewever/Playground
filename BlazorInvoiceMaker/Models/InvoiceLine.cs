namespace BlazorInvoiceMaker.Models
{
    public class InvoiceLine
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal ExtendedPrice
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }
    }
}
