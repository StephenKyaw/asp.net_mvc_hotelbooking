namespace WebMvcUI.Models
{
    public class BookingViewModel
    {
        public string Id { get; set; }
        public DateTime BookingDate { get; set; }
        public string Remark { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }

        public IEnumerable<BookingItemViewModel> BookingItems { get;set;} = new List<BookingItemViewModel>();
    }
    public class BookingItemViewModel
    {
        public string Id { get; set; }
        public string RoomId { get; set; }
        public string Description { get;set;}
        public string BedTypes { get;set;}
        public decimal Price { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal Amount { get;set;}
        public string UserID { get;set;}
        public string UserName { get;set;}

    }
}
