namespace BlazorInvoiceMaker.Models
{
    public class Invoice
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public string ReferenceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(30);
        public string PayeeName { get; set; } = string.Empty;
        public string BillingAddress { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal ShippingFees { get; set; }
        public decimal OtherFees { get; set; }
        public decimal Taxes { get; set; }
        public decimal Total {
            get
            {
                return Subtotal + ShippingFees + OtherFees + Taxes;
            }
        }
        public List<InvoiceLine> Lines { get; set; } = [];
    }
}
