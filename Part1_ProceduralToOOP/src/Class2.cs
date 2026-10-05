using System;

public class OrderManager
{
    public List<Customer> Customers { get; set; } = new List<Customer>();
    public List<Product> Products { get; set; } = new List<Product>();
    public List<Order> Orders { get; set; } = new List<Order>();

    
    private Customer GetCustomerById(int id)
    {
        foreach (Customer c in Customers)
        {
            if (c.Id == id)
                return c;
        }
        return null;
    }

    private Product GetProductById(int id)
    {
        foreach (Product p in Products)
        {
            if (p.Id == id)
                return p;
        }
        return null;
    }

    private Order GetOrderById(int id)
    {
        foreach (Order o in Orders)
        {
            if (o.Id == id)
                return o;
        }
        return null;
    }

   
    public void AddCustomer(int id, string name, string email, string city, bool isVip)
    {
        if (GetCustomerById(id) != null)
        {
            Console.WriteLine($"ERROR: customer id {id} already exists.");
            return;
        }
        Customers.Add(new Customer(id, name, email, city, isVip));
    }

    public void AddProduct(int id, string name, double price, int stock)
    {
        if (GetProductById(id) != null)
        {
            Console.WriteLine($"ERROR: product id {id} already exists.");
            return;
        }
        Products.Add(new Product(id, name, price, stock));
    }

    public void CreateOrder(int orderId, int customerId, string date)
    {
        if (GetOrderById(orderId) != null)
        {
            Console.WriteLine($"ERROR: order id {orderId} already exists.");
            return;
        }

        Customer customer = GetCustomerById(customerId);
        if (customer == null)
        {
            Console.WriteLine($"ERROR: customer id {customerId} not found.");
            return;
        }

        Orders.Add(new Order(orderId, customer, date));
    }

    public void AddLineToOrder(int orderId, int productId, int quantity)
    {
        Order order = GetOrderById(orderId);
        if (order == null)
        {
            Console.WriteLine($"ERROR: order id {orderId} not found.");
            return;
        }

        if (order.IsPaid)
        {
            Console.WriteLine("ERROR: cannot change a paid order.");
            return;
        }

        Product product = GetProductById(productId);
        if (product == null)
        {
            Console.WriteLine($"ERROR: product id {productId} not found.");
            return;
        }

        if (quantity <= 0)
        {
            Console.WriteLine("ERROR: quantity must be positive.");
            return;
        }

        if (product.Stock < quantity)
        {
            Console.WriteLine($"ERROR: not enough stock for product #{productId}.");
            return;
        }

        product.Stock -= quantity;
        order.Lines.Add(new OrderLine(product, quantity));
    }

    public void MarkOrderPaid(int orderId)
    {
        Order order = GetOrderById(orderId);
        if (order == null)
        {
            Console.WriteLine($"ERROR: order id {orderId} not found.");
            return;
        }

        if (order.Lines.Count == 0)
        {
            Console.WriteLine("ERROR: cannot pay an empty order.");
            return;
        }

        order.IsPaid = true;
    }

    public double TotalSalesPaidOnly()
    {
        double sum = 0.0;
        foreach (Order o in Orders)
        {
            if (o.IsPaid)
            {
                sum += o.CalculateTotal();
            }
        }
        return sum;
    }
}

