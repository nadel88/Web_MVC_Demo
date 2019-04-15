using SImpleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;

namespace SImpleWebApp.Controllers
{
    public class ProductsManageController : Controller
    {
        //simpleEntities2 DB = new simpleEntities2();
        demo_appEntities1 DB = new demo_appEntities1();
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       
        public ActionResult SearchProductAJAX(string searchText)
        {
            ViewBag.Message = "AJAX Search.";
            List<SImpleWebApp.Models.T_products> plist = new List<SImpleWebApp.Models.T_products>();
          
            var query = from st in DB.T_products
                        where st.P_name.Contains(searchText)
                        select st;


            plist = query.ToList();

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
                               
            return View(plist);
        }
        public PartialViewResult SearchProductPVAJAX(string searchText)
        {
            
            ViewBag.Message = "Product Search Result Page.";
            
            List<SImpleWebApp.Models.T_products> plist = new List<SImpleWebApp.Models.T_products>();

            var query = from st in DB.T_products
                        where st.P_name.Contains(searchText)
                        select st;

            plist = query.ToList();
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
            return PartialView(plist);
        }
    
        public PartialViewResult AddProductPVAJAX(string pName,string pDesc,string pStock)
        {            
            List<SImpleWebApp.Models.T_products> plist = new List<SImpleWebApp.Models.T_products>();

            if (pName == "" || pDesc == "" || pStock == null)
            {
                Response.Write("Fields cannot be empty, please fill in all detils");
                return PartialView(plist);
            }


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

            return PartialView("SearchProductAJAX", plist);
        }

        public PartialViewResult RemoveProductPVAJAX(int [] pids)
        {

            List<SImpleWebApp.Models.T_products> plist = new List<SImpleWebApp.Models.T_products>();
            List<SImpleWebApp.Models.T_products> plistToRem = new List<SImpleWebApp.Models.T_products>();

            //remove duplicates that arrive from the view jquery method 
            //TODO: (need to check why duplicates arrive to controller)
            int[] pidDistinct = pids.Distinct().ToArray();


            for (int i = 0; i < pidDistinct.Length; i++)
            {
                T_products tp = DB.T_products.Find(pidDistinct[i]);                
                plistToRem.Add(tp);
            }

          
            DB.T_products.RemoveRange(plistToRem);
            int res = DB.SaveChanges();

            if (res > 0)
            {
                Response.Write("new product was removed successfuly");
            }
            else
            {
                Response.Write("Operation Failed");
            }

            var query = from st in DB.T_products
                        where st.P_name.Contains("")
                        select st;

            plist = query.ToList();

            return PartialView("SearchProductPVAJAX", plist);
        }

        public PartialViewResult EditProductPVAJAX(int pid,string altName,string altDesc,int altStock)
        {
            List<SImpleWebApp.Models.T_products> plist = new List<SImpleWebApp.Models.T_products>();

            T_products tp = DB.T_products.Find(pid);

            if (altName != "")
            {
                tp.P_name = altName;
            }            
            if(altDesc != "")
            {
                tp.P_description = altDesc;
            }
            
            if(altStock != -1)
            {
                tp.P_stock = altStock;
            }
            
            
            /*if(altStock < 0)
            {

                tp.P_stock = 0;
            }
            else
            {
                tp.P_stock = altStock;
            }*/

            DB.T_products.AddOrUpdate(tp);
            int res = DB.SaveChanges();

            if (res > 0)
            {
                Response.Write("Selected Product was edited successfuly");
            }
            else
            {
                Response.Write("Operation Failed");
            }

            var query = from st in DB.T_products
                        where st.P_name.Contains("")
                        select st;

            plist = query.ToList();



            return PartialView("SearchProductPVAJAX", plist);
        }


    }
}