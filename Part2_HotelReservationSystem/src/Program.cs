using ClassLibrary1;

namespace Part2_HotelReservationSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Room room101 = new Room(101, RoomType.Single, 1000m);
            Guest guest1 = new Guest(1, "Hossam Shawky", "01012345678");

            Console.WriteLine($"Guest: {guest1.Name} created successfully.\n");

            DateTime checkInDate = DateTime.Today.AddDays(1);
            DateTime checkOutDate = DateTime.Today.AddDays(4);

            Reservation firstBooking = new Reservation(1001, checkInDate, checkOutDate, room101);

            guest1.MakeReservation(firstBooking);

            Console.WriteLine($"Reservation {firstBooking.Id} added for room {firstBooking.Room.Number}.");
            Console.WriteLine($"Total Cost: {firstBooking.TotalCost} EGP");

            firstBooking.Confirm();
            firstBooking.CheckInGuest();
            Console.WriteLine($"Current Status: {firstBooking.Status}\n");

            Console.WriteLine("--- Testing Maintenance Rule ---");
            Room room202 = new Room(202, RoomType.Double, 2000m);

            room202.StartMaintenance();

            try
            {
                Reservation failedBooking = new Reservation(1002, checkInDate, checkOutDate, room202);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error expected: {ex.Message}");
            }
        }
    }
}
