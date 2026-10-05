using static Part3_BuilderPattern.Invoice;

namespace Part3_BuilderPattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var billingAddress = new AddressBuilder()
           .SetStreet("Tahrir Street")
           .SetCity("Cairo")
           .SetState("Cairo")
           .SetZipCode("11511")
           .SetCountry("Egypt");

            var shippingAddress = new AddressBuilder()
                .SetStreet("Nile Street")
                .SetCity("Giza")
                .SetState("Giza")
                .SetZipCode("12511")
                .SetCountry("Egypt");

            var order = new OrderBuilder()
                .SetOrderDate(DateTime.Now)
                .SetPayment("Credit Card", "EGP")
                .SetAmounts(1000m, 100m, 140m, 1040m);

            var invoice = new InvoiceBuilder(100, "Hossam")
                .SetCustomerContact(
                    "hossam@gmail.com",
                    "01020304050")
                .SetBillingAddress(billingAddress)
                .SetShippingAddress(shippingAddress)
                .SetOrder(order)
                .Build();

            Console.WriteLine(invoice.CustomerName);
            Console.WriteLine(invoice.BillingAddress.City);
            Console.WriteLine(invoice.Order.TotalAmount);
        }
    }

    public class Address
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
        public string Country { get; }

        public Address(
            string street,
            string city,
            string state,
            string zipCode,
            string country)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
            Country = country;
        }
    }

    public class AddressBuilder
    {
        private string? street;
        private string? city;
        private string? state;
        private string? zipCode;
        private string? country;

        public AddressBuilder SetStreet(string value)
        {
            street = value;
            return this;
        }

        public AddressBuilder SetCity(string value)
        {
            city = value;
            return this;
        }

        public AddressBuilder SetState(string value)
        {
            state = value;
            return this;
        }

        public AddressBuilder SetZipCode(string value)
        {
            zipCode = value;
            return this;
        }

        public AddressBuilder SetCountry(string value)
        {
            country = value;
            return this;
        }

        public Address Build()
        {
            if (string.IsNullOrWhiteSpace(street) ||
                string.IsNullOrWhiteSpace(city) ||
                string.IsNullOrWhiteSpace(state) ||
                string.IsNullOrWhiteSpace(zipCode) ||
                string.IsNullOrWhiteSpace(country))
            {
                throw new InvalidOperationException(
                    "All address fields are required.");
            }

            return new Address(
                street, city, state, zipCode, country);
        }
    }

    public class OrderInfo
    {
        public DateTime OrderDate { get; }
        public string PaymentMethod { get; }
        public string Currency { get; }
        public decimal SubTotal { get; }
        public decimal DiscountAmount { get; }
        public decimal TaxAmount { get; }
        public decimal TotalAmount { get; }

        public OrderInfo(
            DateTime orderDate,
            string paymentMethod,
            string currency,
            decimal subTotal,
            decimal discountAmount,
            decimal taxAmount,
            decimal totalAmount)
        {
            OrderDate = orderDate;
            PaymentMethod = paymentMethod;
            Currency = currency;
            SubTotal = subTotal;
            DiscountAmount = discountAmount;
            TaxAmount = taxAmount;
            TotalAmount = totalAmount;
        }
    }

    public class OrderBuilder
    {
        private DateTime? orderDate;
        private string? paymentMethod;
        private string? currency;

        private decimal subTotal;
        private decimal discountAmount;
        private decimal taxAmount;
        private decimal totalAmount;
        private bool amountsProvided;

        public OrderBuilder SetOrderDate(DateTime value)
        {
            if (value == default)
                throw new ArgumentException("Order date is required.");

            orderDate = value;
            return this;
        }

        public OrderBuilder SetPayment(
            string method, string currency)
        {
            if (string.IsNullOrWhiteSpace(method))
                throw new ArgumentException("Payment method is required.");

            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency is required.");

            paymentMethod = method;
            this.currency = currency;
            return this;
        }

        public OrderBuilder SetAmounts(
            decimal subTotal,
            decimal discount,
            decimal tax,
            decimal total)
        {
            if (subTotal < 0 || discount < 0 || tax < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(subTotal), "Amounts cannot be negative.");

            if (discount > subTotal)
                throw new ArgumentException(
                    "Discount cannot exceed subtotal.");

            if (total != subTotal - discount + tax)
                throw new ArgumentException(
                    "Total must equal subtotal minus discount plus tax.");

            this.subTotal = subTotal;
            discountAmount = discount;
            taxAmount = tax;
            totalAmount = total;
            amountsProvided = true;

            return this;
        }

        public OrderInfo Build()
        {
            if (orderDate == null)
                throw new InvalidOperationException(
                    "Call SetOrderDate() before Build().");

            if (paymentMethod == null || currency == null)
                throw new InvalidOperationException(
                    "Call SetPayment() before Build().");

            if (!amountsProvided)
                throw new InvalidOperationException(
                    "Call SetAmounts() before Build().");

            return new OrderInfo(
                orderDate.Value,
                paymentMethod,
                currency,
                subTotal,
                discountAmount,
                taxAmount,
                totalAmount);
        }
    }

    public class Invoice
    {
        public int InvoiceId { get; }
        public string CustomerName { get; }
        public string CustomerEmail { get; }
        public string CustomerPhone { get; }

        public Address BillingAddress { get; }
        public Address? ShippingAddress { get; }
        public OrderInfo Order { get; }

        public Invoice(
            int invoiceId,
            string customerName,
            string customerEmail,
            string customerPhone,
            Address billingAddress,
            Address? shippingAddress,
            OrderInfo order)
        {
            InvoiceId = invoiceId;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerPhone = customerPhone;

            BillingAddress = billingAddress;
            ShippingAddress = shippingAddress;
            Order = order;
        }
    }

    public class InvoiceBuilder
    {
        private readonly int invoiceId;
        private readonly string customerName;

        private string customerEmail = "";
        private string customerPhone = "";

        private AddressBuilder? billingAddressBuilder;
        private AddressBuilder? shippingAddressBuilder;
        private OrderBuilder? orderBuilder;

        public InvoiceBuilder(int invoiceId, string customerName)
        {
            if (invoiceId <= 0)
                throw new ArgumentException(
                    "InvoiceId must be greater than zero.");

            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException(
                    "Customer name is required.");

            this.invoiceId = invoiceId;
            this.customerName = customerName;
        }

        public InvoiceBuilder SetCustomerContact(
            string email, string phone)
        {
            customerEmail = email ?? "";
            customerPhone = phone ?? "";
            return this;
        }

        public InvoiceBuilder SetBillingAddress(
            AddressBuilder addressBuilder)
        {
            billingAddressBuilder = addressBuilder
                ?? throw new ArgumentNullException(nameof(addressBuilder));

            return this;
        }

        public InvoiceBuilder SetShippingAddress(
            AddressBuilder addressBuilder)
        {
            shippingAddressBuilder = addressBuilder
                ?? throw new ArgumentNullException(nameof(addressBuilder));

            return this;
        }

        public InvoiceBuilder SetOrder(OrderBuilder builder)
        {
            orderBuilder = builder
                ?? throw new ArgumentNullException(nameof(builder));

            return this;
        }

        public Invoice Build()
        {
            if (billingAddressBuilder == null)
                throw new InvalidOperationException(
                    "Billing address is required.");

            if (orderBuilder == null)
                throw new InvalidOperationException(
                    "Order information is required.");

            Address billingAddress = billingAddressBuilder.Build();

            Address? shippingAddress =
                shippingAddressBuilder?.Build();

            OrderInfo order = orderBuilder.Build();

            return new Invoice(
                invoiceId,
                customerName,
                customerEmail,
                customerPhone,
                billingAddress,
                shippingAddress,
                order);
        }
    }
}
   
