﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedIn.Domain.Entities;
using MedIn.Libs;
using MedIn.Web.Models;

namespace MedIn.Web.Core
{
    public interface ITabService
    {
        IList<ProductTabViewModel> GetProductTabs(Product currentProduct, ControllerContext context);
    }
    public class TabService : ITabService
    {
        public IList<ProductTabViewModel> GetProductTabs(Product currentProduct, ControllerContext context)
        {
            Debug.Assert(currentProduct != null, "currentProduct!=null");
            var res = new List<ProductTabViewModel>();
            var decriptionViewData = new ViewDataDictionary(new ReviewDetailsViewModel
            {
                DescGroups = currentProduct.DescGroups.Where(x=>x.Visibility).OrderBy(x=>x.Sort).ToList(),
                Files = currentProduct.Gallery.Files.ToList()
            });
            res.Add(new ProductTabViewModel
            {
                Name = "Описание",
                Content = ViewHelpers.RenderPartialViewToString(MVC.Products.Views.TabPartialDetails._ReviewDetails, decriptionViewData, context)
            });
            if (currentProduct.TechProperties.Any())
            {
                var techViewData = new ViewDataDictionary(currentProduct.TechProperties.ToList());
                res.Add(new ProductTabViewModel
                {
                    Name = "Технические характеристики",
                    Content = ViewHelpers.RenderPartialViewToString(MVC.Products.Views.TabPartialDetails._TechDetails, techViewData, context)
                });
            }
            return res;
        }
    }
}