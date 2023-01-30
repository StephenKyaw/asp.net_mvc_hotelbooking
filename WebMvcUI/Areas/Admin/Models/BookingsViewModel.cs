namespace WebMvcUI.Areas.Admin.Models
{
    public class BookingsViewModel
    {
        public string Id { get; set; }
        public DateTime BookingDate { get; set; }
        public string Remark { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }

        public List<BookingsItemViewModel> Items { get; set; } = new List<BookingsItemViewModel>();

    }

    public class BookingsItemViewModel
    {
        public string ItemId { get; set; }
        public string BookingsID { get; set; }
        public string RoomId { get; set; }
        public string RoomType { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Township { get; set; }
        public string HotelName { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal price { get; set; }
        public decimal Amount { get; set; }
    }
}
