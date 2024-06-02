namespace ShalehPrpject.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string Nationality { get; set; }
        public string PhoneNumber { get; set; }
        public string IdNumber { get; set; }
        public double TotalPrice { get; set; }
        public int Amount { get; set; }
        public int Reset { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartureDate { get; set; }
    }
}
