using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MedIn.Domain.Entities;

namespace MedIn.Web.Models
{
    public class ReviewDetailsViewModel
    {
        public List<DescGroup> DescGroups { get; set; }
        public List<File> Files { get; set; }
    }
}