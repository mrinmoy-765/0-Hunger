using _0_Hunger.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zero_Hunger.Models;
using System.Data.Entity.Migrations;

namespace _0_Hunger.Controllers
{
    public class employeeController : Controller
    {

        
        [HttpGet]
        
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult collectRequest(int id)
        {
            var db = new ZeroHungerEntities();
            request rq = db.requests.Find(id);
            if (rq == null)
            {
                TempData["msg"] = "Three is no request with id " + id.ToString();
            }
            else
            {
                rq.status = "collected";
                db.requests.AddOrUpdate(rq);
                TempData["msg"] = "Request of id " + id.ToString() + " is set as collected";
                db.SaveChanges();
            }
            return RedirectToAction("requestlist");
        }
       
        [HttpGet]
        public ActionResult requestlist()
        {
            var db = new ZeroHungerEntities();
            return View(db.employees.Find((int)Session["id"]).requests.ToList());
        }
        
        [HttpGet]
        public ActionResult requestdetails(int id)
        {
            var db = new ZeroHungerEntities();
            return View(db.requests.Find(id));
        }
       
        [HttpGet]
        public ActionResult restaurantDetails(int id)
        {
            var db = new ZeroHungerEntities();
            return View(db.restaurants.Find(id));
        }
        [HttpGet]
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(loginDTO obj)
        {
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerEntities();
                var user = (from u in db.employees
                            where
                                u.username.Equals(obj.username) &&
                                u.password.Equals(obj.password)
                            select u).FirstOrDefault();
                if (user != null)
                {
                    Session["user"] = user.username;
                    Session["id"] = user.id;
                    Session["type"] = "employee";
                    TempData["msg"] = "Successfully logged in";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["msg"] = "Invalid credential";
                }
            }
            return View(obj);
        }
        [HttpGet]
        public ActionResult logout()
        {
            Session.Clear();
            TempData["msg"] = "Successfully logged out";
            return RedirectToAction("login");
        }
    }
}