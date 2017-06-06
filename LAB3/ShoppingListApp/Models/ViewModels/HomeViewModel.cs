using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ShoppingListApp.Models.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {

        }

        public HomeViewModel(string actionListName, Product actionProduct)
        {
            ActionListName = actionListName;
            ActionProduct = actionProduct;
        }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The length of the line should be from 3 to 15")]
        public string ActionListName { get; set; }

        [Required]
        public Product ActionProduct {get; set; }
    }
}