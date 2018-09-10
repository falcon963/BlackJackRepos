using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASP.stading.Models;

namespace ASP.stading.Controllers
{
    public class HomeController : Controller
    {
        MobileContext db;
       public HomeController(MobileContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View(db.Phones.ToList());
        }
        
        [HttpGet]
        public IActionResult Buy(int id)
        {
            ViewBag.PhoneId = id;
            return View();
        }

        [HttpPost]
        public string Buy(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return "Gl hf " + order.User + "!";
        }


    }
}
