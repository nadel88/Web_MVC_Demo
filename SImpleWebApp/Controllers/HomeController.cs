using SImpleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SImpleWebApp.Controllers
{
    public class HomeController : Controller
    {
        //simpleEntities2 DB = new simpleEntities2();
        demo_appEntities DB = new demo_appEntities();
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Products()
        {
            
            List<SImpleWebApp.Models.T_products> plist = new List<SImpleWebApp.Models.T_products>();

            plist = DB.T_products.ToList();

            ViewBag.Message = "Your contact page.";

            return View(plist);
        }
        public ActionResult SearchProduct(string search)
        {
            ViewBag.Message = "Product Search Result Page.";
            List<SImpleWebApp.Models.T_products> plist = new List<SImpleWebApp.Models.T_products>();
            var query = from st in DB.T_products
                        where st.P_name.Contains(search) 
                       select st;

            plist = query.ToList();


            return View(plist);
        }
        public ActionResult SearchProductAJAX(string searchText)
        {
            ViewBag.Message = "AJAX Search.";
            List<SImpleWebApp.Models.T_products> plist = new List<SImpleWebApp.Models.T_products>();
          
            var query = from st in DB.T_products
                        where st.P_name.Contains(searchText)
                        select st;

            if (plist.Count == 0)
            {
                query = from st in DB.T_products
                        where st.P_description.Contains(searchText)
                        select st;
            }

            plist = query.ToList();

            if (plist.Count == 0)
            {
                plist = DB.T_products.ToList();
            }
            else
            {                               
                plist = query.ToList();
            }
                   
            return View(plist);
        }
        public PartialViewResult SearchProductPVAJAX(string searchText)
        {
            
            ViewBag.Message = "Product Search Result Page.";
            
            List<SImpleWebApp.Models.T_products> plist = new List<SImpleWebApp.Models.T_products>();

            var query = from st in DB.T_products
                        where st.P_name.Contains(searchText)
                        select st;

            //check if search value did not match in the P_name table
            //and check the P_description table
            if (plist.Count == 0)
            {
                query = from st in DB.T_products
                        where st.P_description.Contains(searchText)
                        select st;
            }

            plist = query.ToList();

            if (plist.Count == 0)
            {
                plist = DB.T_products.ToList();
            }
            else
            {
                plist = query.ToList();
            }
            
            return PartialView(plist);
        }
    
        public PartialViewResult AddProductPVAJAX(string pName,string pDesc,string pStock)
        {
            List<SImpleWebApp.Models.T_products> plist = new List<SImpleWebApp.Models.T_products>();

            T_products newProduct = new T_products();
            newProduct.P_name = pName;
            newProduct.P_description = pDesc;
            try
            {
                newProduct.P_stock = int.Parse(pStock);
            }
            catch
            {
                throw new Exception();
            }

            DB.T_products.Add(newProduct);
            int res = DB.SaveChanges();

            if(res > 0)
            {
                Response.Write("new product was added successfuly");
            }
            else
            {
                Response.Write("Operation Failed");
            }

            var query = from st in DB.T_products
                        where st.P_name.Contains("")
                        select st;

            plist = query.ToList();

            return PartialView(plist);
        }
    }
}