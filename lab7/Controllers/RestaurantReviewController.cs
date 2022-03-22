using lab7.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace lab7.Controllers
{
    [EnableCors]
    [Route("[controller]")]
    [ApiController]
    public class RestaurantReviewController : ControllerBase
    {
        // GET: <RestaurantReviewController>
        [HttpGet]
        public List<RestaurantInfo> GetAllRestaurantReviews()
        {
            restaurantlist restaurantlist = GetRestaurantsFromXml();
            List<RestaurantInfo> restaurantInfos = new List<RestaurantInfo>();
            int id = 1;

            foreach (restaurant r in restaurantlist.restaurant) 
            { 
                RestaurantInfo restaurantInfo = GetRestaurantInfo(r);

                restaurantInfo.Id = id;
                restaurantInfos.Add(restaurantInfo);
                id++;
            }

            return restaurantInfos;
        }

        [HttpGet("names")]
        public List<string> GetRestaurantNames()
        {
            restaurantlist restaurantlist = GetRestaurantsFromXml();
            List<string> names = new List<string>();

            foreach (restaurant r in restaurantlist.restaurant)
            {
                names.Add(r.name);
            }

            return names;
        }

        // GET api/<RestaurantReviewController>/5
        [HttpGet("{id}")]
        public string GetRestaurantReviewbyId(int id)
        {
            restaurantlist restaurantlist = GetRestaurantsFromXml();
            restaurant restaurant = restaurantlist.restaurant[id - 1];
            RestaurantInfo restaurantInfo = GetRestaurantInfo(restaurant);
            restaurantInfo.Id=id;
            //string restaurantInfoJson = JsonSerializer.Serialize(restaurantInfo);
            string restaurantInfoJson = JsonConvert.SerializeObject(restaurantInfo);
            return restaurantInfoJson;
        }

        // POST api/<RestaurantReviewController>
        [HttpPost]
        public void SaveRestaurantReview([FromBody] RestaurantInfo restaurantInfo)
        {
            restaurantlist restaurantlist = GetRestaurantsFromXml();
            List<restaurant> restaurants = restaurantlist.restaurant.ToList();
            // TODO: figure out why no sub attributs in object
            var restaurant = new restaurant();
            restaurant.name = restaurantInfo.Name;
            restaurant.summary = restaurantInfo.Summary;
            restaurant.food = restaurantInfo.FoodType;

            var rating = new restaurantRating();
            rating.Value = restaurantInfo.Rating;
            rating.max = 5;
            rating.min = 1;
            restaurant.rating = rating;

            var price = new restaurantPrice();
            price.Value = restaurantInfo.Cost;
            price.max = 5;
            price.min = 1;
            restaurant.price = price;

            var address = new address();
            address.city = restaurantInfo.Address.City;
            address.street = restaurantInfo.Address.Street;
            address.province = (ProvinceType)Enum.Parse(typeof(ProvinceType), restaurantInfo.Address.ProvinceState);
            address.postalcode = restaurantInfo.Address.PostalCode;
            restaurant.address = address;
     
            restaurants.Add(restaurant);
            restaurantlist.restaurant = restaurants.ToArray();

            UpdateRestaurantsToXml(restaurantlist);
        }

        // PUT /<RestaurantReviewController>/5
        [HttpPut("{id}")]
        public void UpdateRestaurantReview(int id, [FromBody] RestaurantInfo restaurantInfo)
        {
            restaurantlist restaurantlist = GetRestaurantsFromXml();
            restaurant restaurant = restaurantlist.restaurant[id - 1];
            
            restaurant.name = restaurantInfo.Name;
            restaurant.rating.Value = restaurantInfo.Rating;
            restaurant.summary = restaurantInfo.Summary;
            restaurant.price.Value = restaurantInfo.Cost;
            restaurant.food = restaurantInfo.FoodType;
            restaurant.address.city = restaurantInfo.Address.City;
            restaurant.address.street = restaurantInfo.Address.Street;
            restaurant.address.province = (ProvinceType)Enum.Parse(typeof(ProvinceType), restaurantInfo.Address.ProvinceState);
            restaurant.address.postalcode = restaurantInfo.Address.PostalCode;

            restaurantlist.restaurant[id - 1] = restaurant;

            UpdateRestaurantsToXml(restaurantlist);
            //GetRestaurantReviewbyId(id);
        }

        // DELETE /<RestaurantReviewController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            restaurantlist restaurantlist = GetRestaurantsFromXml();
            List<restaurant> restaurants= restaurantlist.restaurant.ToList();
           
            restaurants.RemoveAt(id - 1);
            restaurantlist.restaurant = restaurants.ToArray();

            UpdateRestaurantsToXml(restaurantlist);
        }

        public restaurantlist GetRestaurantsFromXml()
        {
            string xmlFilePath = Path.GetFullPath("Data/restaurant_reviews.xml");

            FileStream xs = new FileStream(xmlFilePath, FileMode.Open);
            XmlSerializer serializor = new XmlSerializer(typeof(restaurantlist));
            restaurantlist restaurantlist = (restaurantlist)serializor.Deserialize(xs);
            xs.Close();

            return restaurantlist;
        }
        public RestaurantInfo GetRestaurantInfo(restaurant restaurant)
        {
            RestaurantInfo restaurantInfo = new RestaurantInfo();
            Address Address = new Address();

            restaurantInfo.Id = 0;
            restaurantInfo.Name = restaurant.name;
            restaurantInfo.FoodType = restaurant.food;
            restaurantInfo.Cost = Decimal.ToInt32(restaurant.price.Value);
            restaurantInfo.Rating = Decimal.ToInt32(restaurant.rating.Value);
            restaurantInfo.Summary = restaurant.summary;

            Address.City = restaurant.address.city;
            Address.ProvinceState = restaurant.address.province.ToString();
            Address.Street = restaurant.address.street;
            Address.PostalCode = restaurant.address.postalcode;

            restaurantInfo.Address = Address;

            return restaurantInfo;
        }
        public void UpdateRestaurantsToXml(restaurantlist restaurantlist)
        {     
            string xmlFilePath = Path.GetFullPath("Data/restaurant_reviews.xml");
            XmlTextWriter tw = new XmlTextWriter(xmlFilePath, Encoding.UTF8);
            XmlSerializer serializer = new XmlSerializer(typeof(restaurantlist));

            serializer.Serialize(tw, restaurantlist);
            tw.Close();

        }

    }
}
