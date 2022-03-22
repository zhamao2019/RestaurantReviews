namespace lab7.Models
{
    public class RestaurantInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FoodType { get; set; }

        public int Rating { get; set; }

        public int Cost { get; set; }

        public string Summary { get; set; }

        public Address Address { get; set; }
    }
    public class Address
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string ProvinceState { get; set; }

        public string PostalCode { get; set; }
    }
}
