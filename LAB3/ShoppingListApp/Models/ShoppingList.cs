using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Linq;

namespace ShoppingListApp.Models
{
    public class ShoppingList : Dictionary<string, List<Product>>
    {
    }
    public class Product
    {
        public Product()
        {
        }
        public Product(string name, string quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        [JsonProperty(PropertyName = "name")]
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The length of the line should be from 3 to 15")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "quantity")]
        [Required]
        [StringLength(6, ErrorMessage = "The length of the line should be 6 or less")]
        [RegularExpression(@"\d+kg|\d\.\d+kg|\d+", ErrorMessage = "Correct format is 1,1kg,0.1kg")]
        public string Quantity { get; set; }
    }
}