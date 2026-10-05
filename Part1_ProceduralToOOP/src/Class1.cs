namespace ClassLibrary1
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public bool IsVip { get; set; }

        public Customer(int id, string name, string email, string city, bool isVip)
        {
            Id = id;
            Name = name;
            Email = email;
            City = city;
            IsVip = isVip;
        }
        public void PrintCustomer()
        {
            Console.WriteLine($"Id: {Id}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"City: {City}");
            Console.WriteLine($"IsVip: {IsVip}\n");
        }
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price {  get; set; }
        public int Stock {  get; set; }
        public Product(int id, string name, double price, int stock)
        {
            Id = id;
            Name = name;
            Price = price;
            Stock = stock;
        }
        public void printProducts()
        {
            Console.WriteLine($"Id: {Id}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"price: {Price}");
            Console.WriteLine($"stock: {Stock}\n");
        }
    }
    public class Order
    {
        public int Id { get; set; }

        public Customer Customer { get; set; }

        public string Date { get; set; }

        public bool IsPaid { get; set; } = false;

        public List<OrderLine> Lines { get; set; } = new List<OrderLine>();

        public Order(int id, Customer customer, string date)
        {
            Id = id;
            Customer = customer;
            Date = date;
            Lines = new List<OrderLine>();
        }
        public double CalculateTotal()
        {
            double total = Lines.Sum(Line=>Line.LineTotal);
            if(Customer.IsVip)
            {
                total *= 0.9;
            }
            return total;
        }
    }
    public class OrderLine
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public double LineTotal => Product.Price * Quantity;
        public OrderLine(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}
