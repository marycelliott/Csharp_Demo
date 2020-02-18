using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProdsAndCats.Models
{
    public class Category
    {
        [Key]
        public int CategoryId {get;set;}
        
        [Required(ErrorMessage="Category name is required.")]
        public string Name {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<Association> Products {get;set;} = new List<Association>();
    }
}