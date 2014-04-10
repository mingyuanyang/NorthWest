using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NorthWest.Domain.Entities;

namespace NorthWest.WebUI.Models
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}