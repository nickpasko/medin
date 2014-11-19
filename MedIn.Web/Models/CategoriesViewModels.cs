using System.Collections;
using System.Collections.Generic;
using MedIn.Domain.Entities;

namespace MedIn.Web.Models
{
    public class CategoriesViewModels
    {
        public IEnumerable<Product> Products { get; set; }

    }
}