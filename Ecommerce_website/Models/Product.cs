using System.ComponentModel.DataAnnotations;

namespace Ecommerce_website.Models
{
    public class Product
    {
        [Key]
        public int product_id { get; set; }
        [Required]
        public string product_name { get; set; }
        [Required]
        public string product_price { get; set; }
        [Required]
        public string product_description { get; set; }
        
        public string? product_image { get; set; }
        [Required]
        public int cat_id { get; set; }
        public Category category { get; set; }
    }
}
