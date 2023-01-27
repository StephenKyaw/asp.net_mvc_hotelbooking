using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Bookings
{
    public class Booking
    {
        [Key]
        public string BookingID { get; set; }
        public DateTime BookingDate { get; set; }
        public string Remark { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }

        public bool IsConfirm { get; set; }
        public ICollection<BookingItem> BookingItems { get; set; }
    }

    public class BookingItem
    {
        [Key]
        public string BookingItemID { get; set; }
        public string BookingID { get; set; }
        public Booking Booking { get; set; }
        public string RoomId { get; set; }
        public Room Room { get; set; }
        public string Description { get; set; }
        public string BedTypes { get; set; }
        public decimal Price { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal Amount { get; set; }
    }
}


