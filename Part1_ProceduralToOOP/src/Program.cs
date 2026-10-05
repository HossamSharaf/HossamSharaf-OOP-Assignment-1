using ClassLibrary1;

namespace ProceduralToOOP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            OrderManager manager = new OrderManager();

            SeedSampleData(manager);
            RunDemoScenario(manager);

            PrintCustomers(manager);
            PrintProducts(manager);
            PrintAllOrders(manager);

            Console.WriteLine($"\nPaid sales total after demo: {manager.TotalSalesPaidOnly():F2}\n");

            RunInteractiveMenu(manager);
        }
    
    static void SeedSampleData(OrderManager manager)
        {
            manager.AddCustomer(1, "Mona Ali", "mona@example.com", "Cairo", true);
            manager.AddCustomer(2, "Omar Hassan", "omar@example.com", "Alexandria", false);
            manager.AddCustomer(3, "Sara Nabil", "sara@example.com", "Giza", false);

            manager.AddProduct(101, "USB Cable", 50.0, 100);
            manager.AddProduct(102, "Wireless Mouse", 250.0, 40);
            manager.AddProduct(103, "Mechanical Keyboard", 1200.0, 15);
            manager.AddProduct(104, "Laptop Stand", 400.0, 25);
        }

        static void RunDemoScenario(OrderManager manager)
        {
            manager.CreateOrder(1001, 1, "2026-10-02");
            manager.AddLineToOrder(1001, 101, 2);
            manager.AddLineToOrder(1001, 102, 1);
            manager.MarkOrderPaid(1001);

            manager.CreateOrder(1002, 2, "2026-10-02");
            manager.AddLineToOrder(1002, 103, 1);
            manager.AddLineToOrder(1002, 104, 1);

            manager.CreateOrder(1003, 3, "2026-10-03");
            manager.AddLineToOrder(1003, 101, 5);
            manager.MarkOrderPaid(1003);
        }

        static void PrintCustomers(OrderManager manager)
        {
            Console.WriteLine($"\n=== CUSTOMERS ({manager.Customers.Count}) ===");
            foreach (Customer c in manager.Customers)
            {
                string vipStatus = c.IsVip ? "yes" : "no";
                Console.WriteLine($"#{c.Id}  {c.Name}  <{c.Email}>  {c.City}  vip={vipStatus}");
            }
        }

        static void PrintProducts(OrderManager manager)
        {
            Console.WriteLine($"\n=== PRODUCTS ({manager.Products.Count}) ===");
            foreach (Product p in manager.Products)
            {
                Console.WriteLine($"#{p.Id}  {p.Name}  price={p.Price:F2}  stock={p.Stock}");
            }
        }

        static void PrintOrder(Order order)
        {
            if (order == null)
            {
                Console.WriteLine("ERROR: Order not found.");
                return;
            }

            Console.WriteLine($"\n=== ORDER #{order.Id} ===");
            Console.WriteLine($"Date: {order.Date}");
            Console.WriteLine($"Customer: {order.Customer.Name} (#{order.Customer.Id})");

            string paidStatus = order.IsPaid ? "yes" : "no";
            Console.WriteLine($"Paid: {paidStatus}");
            Console.WriteLine("Lines:");

            foreach (OrderLine line in order.Lines)
            {
                Console.WriteLine($"  - {line.Product.Name}  x{line.Quantity}  @{line.Product.Price:F2}  = {line.LineTotal:F2}");
            }

            Console.WriteLine($"TOTAL: {order.CalculateTotal():F2}");
        }

        static void PrintAllOrders(OrderManager manager)
        {
            Console.WriteLine($"\n=== ALL ORDERS ({manager.Orders.Count}) ===");
            foreach (Order o in manager.Orders)
            {
                PrintOrder(o);
            }
        }

       
        static void PrintMenu()
        {
            Console.WriteLine("\n---------- MENU ----------");
            Console.WriteLine("1) Print customers");
            Console.WriteLine("2) Print products");
            Console.WriteLine("3) Print all orders");
            Console.WriteLine("4) Print one order by id");
            Console.WriteLine("5) Create order");
            Console.WriteLine("6) Add line to order");
            Console.WriteLine("7) Mark order paid");
            Console.WriteLine("8) Show paid sales total");
            Console.WriteLine("0) Exit");
            Console.Write("Choice: ");
        }

        static void RunInteractiveMenu(OrderManager manager)
        {
            int choice = -1;
            while (choice != 0)
            {
                PrintMenu();

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                if (choice == 1)
                {
                    PrintCustomers(manager);
                }
                else if (choice == 2)
                {
                    PrintProducts(manager);
                }
                else if (choice == 3)
                {
                    PrintAllOrders(manager);
                }
                else if (choice == 4)
                {
                    Console.Write("Order id: ");
                    int orderId = int.Parse(Console.ReadLine());

                    Order foundOrder = null;
                    foreach (Order o in manager.Orders)
                    {
                        if (o.Id == orderId)
                        {
                            foundOrder = o;
                            break;
                        }
                    }
                    PrintOrder(foundOrder);
                }
                else if (choice == 5)
                {
                    Console.Write("Order id: ");
                    int orderId = int.Parse(Console.ReadLine());
                    Console.Write("Customer id: ");
                    int customerId = int.Parse(Console.ReadLine());
                    Console.Write("Date (YYYY-MM-DD): ");
                    string date = Console.ReadLine();

                    manager.CreateOrder(orderId, customerId, date);
                }
                else if (choice == 6)
                {
                    Console.Write("Order id: ");
                    int orderId = int.Parse(Console.ReadLine());
                    Console.Write("Product id: ");
                    int productId = int.Parse(Console.ReadLine());
                    Console.Write("Quantity: ");
                    int quantity = int.Parse(Console.ReadLine());

                    manager.AddLineToOrder(orderId, productId, quantity);
                }
                else if (choice == 7)
                {
                    Console.Write("Order id: ");
                    int orderId = int.Parse(Console.ReadLine());
                    manager.MarkOrderPaid(orderId);
                }
                else if (choice == 8)
                {
                    Console.WriteLine($"Paid sales total: {manager.TotalSalesPaidOnly():F2}");
                }
                else if (choice == 0)
                {
                    Console.WriteLine("Bye.");
                }
                else
                {
                    Console.WriteLine("Unknown choice.");
                }
            }
        }
    }
}
