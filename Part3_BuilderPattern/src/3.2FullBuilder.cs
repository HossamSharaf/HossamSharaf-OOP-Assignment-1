using static Part3_BuilderPattern.Invoice;

namespace Part3_BuilderPattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var newInvoice = new InvoiceBuilder(100, "hossam", DateTime.Now)
                .SetCustomerContact("hossam@gmail.com","01020304050")
                .SetBillingAddress("tahreer","Cairo", "Cairo", "11511","egypt")
                .SetShippingAddress("Nile Street", "Giza", "Giza", "12511", "Egypt")
                .SetPayment("Credit Card", "EGP")
                .SetAmounts(1000m, 100m, 140m, 1040m)
                .Build();

        }
    }

    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        public string BillingStreet { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingZipCode { get; set; }
        public string BillingCountry { get; set; }

        public string ShippingStreet { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingZipCode { get; set; }
        public string ShippingCountry { get; set; }

        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Currency { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }

        // Constructor
        public Invoice(
            int invoiceId,
            string customerName,
            string customerEmail,
            string customerPhone,
            string billingStreet,
            string billingCity,
            string billingState,
            string billingZipCode,
            string billingCountry,
            string shippingStreet,
            string shippingCity,
            string shippingState,
            string shippingZipCode,
            string shippingCountry,
            DateTime orderDate,
            string paymentMethod,
            string currency,
            decimal subTotal,
            decimal discountAmount,
            decimal taxAmount,
            decimal totalAmount)
        {
            InvoiceId = invoiceId;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerPhone = customerPhone;

            BillingStreet = billingStreet;
            BillingCity = billingCity;
            BillingState = billingState;
            BillingZipCode = billingZipCode;
            BillingCountry = billingCountry;

            ShippingStreet = shippingStreet;
            ShippingCity = shippingCity;
            ShippingState = shippingState;
            ShippingZipCode = shippingZipCode;
            ShippingCountry = shippingCountry;

            OrderDate = orderDate;
            PaymentMethod = paymentMethod;
            Currency = currency;
            SubTotal = subTotal;
            DiscountAmount = discountAmount;
            TaxAmount = taxAmount;
            TotalAmount = totalAmount;
        }

        public class InvoiceBuilder
        {
            private int invoiceId;
            private string customerName;
            private DateTime orderDate;

            private string customerEmail = "";
            private string customerPhone = "";

            private string billingStreet = "";
            private string billingCity = "";
            private string billingState = "";
            private string billingZipCode = "";
            private string billingCountry = "";

            private string shippingStreet = "";
            private string shippingCity = "";
            private string shippingState = "";
            private string shippingZipCode = "";
            private string shippingCountry = "";

            private string paymentMethod = "";
            private string currency = "EGP";

            private decimal subTotal;
            private decimal discountAmount;
            private decimal taxAmount;
            private decimal totalAmount;

            // Constructor: 3 required parameters
            public InvoiceBuilder(
                int invoiceId,
                string customerName,
                DateTime orderDate)
            {
                this.invoiceId = invoiceId;
                this.customerName = customerName;
                this.orderDate = orderDate;
            }

            public InvoiceBuilder SetCustomerContact(
                string email, string phone)
            {
                customerEmail = email;
                customerPhone = phone;
                return this;
            }

            public InvoiceBuilder SetBillingAddress(
                string street,
                string city,
                string state,
                string zipCode,
                string country)
            {
                billingStreet = street;
                billingCity = city;
                billingState = state;
                billingZipCode = zipCode;
                billingCountry = country;
                return this;
            }

            public InvoiceBuilder SetShippingAddress(
                string street,
                string city,
                string state,
                string zipCode,
                string country)
            {
                shippingStreet = street;
                shippingCity = city;
                shippingState = state;
                shippingZipCode = zipCode;
                shippingCountry = country;
                return this;
            }

            public InvoiceBuilder SetPayment(
                string method, string currency)
            {
                paymentMethod = method;
                this.currency = currency;
                return this;
            }

            public InvoiceBuilder SetAmounts(
                decimal subTotal,
                decimal discount,
                decimal tax,
                decimal total)
            {
                subTotal = subTotal;
                discountAmount = discount;
                taxAmount = tax;
                totalAmount = total;
                return this;
            }

            public Invoice Build()
            {
                return new Invoice(
                    invoiceId,
                    customerName,
                    customerEmail,
                    customerPhone,
                    billingStreet,
                    billingCity,
                    billingState,
                    billingZipCode,
                    billingCountry,
                    shippingStreet,
                    shippingCity,
                    shippingState,
                    shippingZipCode,
                    shippingCountry,
                    orderDate,
                    paymentMethod,
                    currency,
                    subTotal,
                    discountAmount,
                    taxAmount,
                    totalAmount
                );
            }
        }
    }
}
