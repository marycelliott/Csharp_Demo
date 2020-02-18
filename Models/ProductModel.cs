using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProdsAndCats.Models
{
    public class Product
    {
        [Key]
        public int ProductId {get;set;}

        public string Name {get;set;}

        public string Description {get;set;}

            public decimal Price {get;set;}

            public DateTime CreatedAt {get;set;} = DateTime.Now;
            public DateTime UpdatedAt {get;set;} = DateTime.Now;

            public List<Association> Associations {get;set;} = new List<Association>();  

        }
    }