namespace ClassLibrary1
{
    public enum RoomType { Single, Double, Suite }
    public enum ReservationStatus { Pending, Confirmed, CheckedIn, CheckedOut, Cancelled }
    public class Guest
    {
        private readonly List<Reservation> _reservations = new List<Reservation>();
        public  string Name { get;  } 
        public int Id { get;  }
        public string PhoneNumber {  get;  }
        public IReadOnlyList<Reservation> Reservations => _reservations.AsReadOnly();

        public Guest(int id, string name, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty.");

            Id = id;
            Name = name;
            PhoneNumber = phoneNumber;
        }
        public void MakeReservation(Reservation reservation)
        {
            bool isDoubleBooked = false;
            foreach (Reservation r in _reservations)
            {
                if (r.Room.Number == reservation.Room.Number &&
                    r.Status != ReservationStatus.Cancelled &&
                    r.Status != ReservationStatus.CheckedOut &&
                    r.CheckIn < reservation.CheckOut &&
                    r.CheckOut > reservation.CheckIn)
                {
                    isDoubleBooked = true; // وجدنا تعارضاً!
                    break; // نوقف حلقة التكرار فوراً لأنه لا داعي لفحص باقي الحجوزات
                }
            }

            if (isDoubleBooked)
                throw new InvalidOperationException("The room is already booked for these dates.");

            _reservations.Add(reservation);
        }

    }
    public class Reservation
    {
        public int Id { get;  }
        public DateTime CheckIn { get;  }
        public DateTime CheckOut { get; }
        public Room Room {  get;  }
        public ReservationStatus Status { get; private set; }
        public Reservation(int id, DateTime checkIn, DateTime checkOut, Room room)
        {
            if (checkOut <= checkIn)
                throw new ArgumentException("Check-out date must be after check-in date.");

            if (room.IsUnderMaintenance)
                throw new InvalidOperationException("Cannot book a room that is under maintenance.");

            Id = id;
            CheckIn = checkIn;
            CheckOut = checkOut;
            Room = room;
            Status = ReservationStatus.Pending; 
        }
        public decimal TotalCost => (decimal)(CheckOut - CheckIn).TotalDays * Room.NightlyRate;
        public void Confirm()
        {
            if (Status != ReservationStatus.Pending)
                throw new InvalidOperationException("Can only confirm a pending reservation.");
            Status = ReservationStatus.Confirmed;
        }

        public void CheckInGuest()
        {
            if (Status != ReservationStatus.Confirmed)
                throw new InvalidOperationException("Can only check in a confirmed reservation.");
            Status = ReservationStatus.CheckedIn;
        }

        public void CheckOutGuest()
        {
            if (Status != ReservationStatus.CheckedIn)
                throw new InvalidOperationException("Can only check out a checked-in reservation.");
            Status = ReservationStatus.CheckedOut;
        }

        public void Cancel()
        {
            if (Status == ReservationStatus.CheckedIn || Status == ReservationStatus.CheckedOut)
                throw new InvalidOperationException("Cannot cancel an already active or completed reservation.");
            Status = ReservationStatus.Cancelled;
        }
    }

    public class Room
    {
        public int Number { get; }
        public RoomType Type { get; }
        public decimal NightlyRate {  get; private set; }
        public bool IsUnderMaintenance {  get;private set; }
        public Room(int number,RoomType roomType, decimal nightlyRate)
        {
            Number = number;
            Type = roomType;
            ChangeNightlyRate(nightlyRate);
        }
        public void ChangeNightlyRate(decimal newRate)
        {
            if (newRate <= 0)
                throw new ArgumentException("Nightly rate must be a positive value.");

            NightlyRate = newRate;
        }
        public void StartMaintenance() => IsUnderMaintenance = true;
        public void EndMaintenance() => IsUnderMaintenance = false;
    }
}
