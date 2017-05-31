using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using MyWebSite.Model;

namespace MyWebSite.Controllers
{
    public class ProductsController : Controller
    {

        public IActionResult Products()
        {
            var p = new List<Product>();
            var host = this.HttpContext.Request.Host;
            //        var path = VirtualPathUtility.ToAbsolute("~/");

            //   string host = HttpContext.Current.Request.Url.Host;
            //  api/ProductsApi/GetAll
            var products = "Http://" + host + "/api/ProductsApi/GetAll";
            using (var httpClient = new HttpClient())
            {
                var json = httpClient.GetStringAsync(products);
                var results = json.Result;

                var result = results.ToList();
                // Now parse with JSON.Net
            }


            return View(p);
        }
    }
}