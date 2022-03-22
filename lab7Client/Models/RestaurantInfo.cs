using System.ComponentModel.DataAnnotations;

namespace lab7Client.Models
{
    public class RestaurantInfo
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Rataurant")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Food Type")]
        public string FoodType { get; set; }

        [Required]
        [Range(typeof(int), "1", "5", ErrorMessage = "Rating must be between 1 and 5")]
        [Display(Name = "Rating (best=5)")]
        public int Rating { get; set; }

        [Required]
        [Range(typeof(int), "1", "5", ErrorMessage = "Price must be between 1 and 5")]
        [Display(Name = "Cost (most expensive=5)")]
        public int Cost { get; set; }

        [Required]
        public string Summary { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Province")]
        public string ProvinceState { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$",
                        ErrorMessage = "Must be in the form of A1A 1A1")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
    }

}
