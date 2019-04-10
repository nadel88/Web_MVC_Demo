using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SImpleWebApp.Models
{

    public class ProductsDataObject
    {
        public string searchValue { get; set; }
        public List<SImpleWebApp.Models.T_products> productList { get; set; }

    }
}