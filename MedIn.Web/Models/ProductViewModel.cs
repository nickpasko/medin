using System.Collections;
using System.Collections.Generic;
using MedIn.Domain.Entities;

namespace MedIn.Web.Models
{
    public class ProductViewModel
    {
        public IList<Category> Categories { get; set; }

        public IList<ProductTabViewModel> Tabs { get; set; }

    }
}