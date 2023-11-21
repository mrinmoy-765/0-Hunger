using _0_Hunger.DataBase;
using _0_Hunger.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zero_Hunger.Models;

namespace _0_Hunger.Controllers
{
    public class adminController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult login()
        {
            return View();
        }
        [HttpGet]
        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("login");
        }

        [HttpPost]
        public ActionResult login(loginDTO obj)
        {
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerEntities();
                var user = (from u in db.admins
                            where
                                u.username.Equals(obj.username) &&
                                u.password.Equals(obj.password)
                            select u).SingleOrDefault();
                if (user != null)
                {
                    Session["user"] = user.username;
                    Session["id"] = user.id;
                    Session["type"] = "admin";
                    TempData["msg"] = "Successfully login";
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
        public ActionResult restaurantlist()
        {
            var db = new ZeroHungerEntities();
            return View(db.restaurants.ToList());
        }


        [HttpGet]
       
        public ActionResult addrestaurant()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult addrestaurant(restaurant s)
        {
            var db = new ZeroHungerEntities();
            db.restaurants.Add(s);
            db.SaveChanges();
            return RedirectToAction("restaurantlist");

        }


        [HttpGet]
        public ActionResult editrestaurant(int id)
        {
            var db = new ZeroHungerEntities();
            var ex = (from d in db.restaurants
                      where d.id == id
                      select d).SingleOrDefault();
            return View(ex);

        }

        [HttpPost]
        public ActionResult editrestaurant(restaurant s)
        {
            var db = new ZeroHungerEntities();
            var exdata = db.restaurants.Find(s.id);
           
            db.Entry(exdata).CurrentValues.SetValues(s);
            db.SaveChanges();
            return RedirectToAction("restaurantlist");
        }

        [HttpGet]
        public ActionResult deleterestaurant(int id)
        {
            var db = new ZeroHungerEntities();
            var ex = (from d in db.restaurants
                      where d.id == id
                      select d).SingleOrDefault();
            return View(ex);
        }


        [HttpPost]
        public ActionResult deleterestaurant(restaurant s)
        {
            var db = new ZeroHungerEntities();
            var ex = (from d in db.restaurants
                      where d.id == s.id
                      select d).FirstOrDefault();
            db.restaurants.Remove(ex);
            db.SaveChanges();
            return RedirectToAction("restaurantlist");
        }


        [HttpGet]
        public ActionResult restaurantDetails(int id)
        {
            var db = new ZeroHungerEntities();
            return View(db.restaurants.Find(id));
        }


        employee convert(addEmployeeDTO empDTO)
        {
            employee emp = new employee()
            {
                name = empDTO.name,
                email = empDTO.email,
                phone = empDTO.phone,
                address = empDTO.address,
                dob = empDTO.dob,
                username = empDTO.username,
                password = empDTO.password,
                admin_id = (int?)Session["id"]
            };
            return emp;
        }


        [HttpGet]
        public ActionResult employeeDetails(int id)
        {
            var db = new ZeroHungerEntities();
            return View(db.employees.Find(id));
        }


        [HttpGet]
        public ActionResult employeelist()
        {
            var db = new ZeroHungerEntities();
            return View(db.employees.ToList());
        }

        [HttpPost]
        public ActionResult deleteEmployee(int id)
        {
            var db = new ZeroHungerEntities();
            db.employees.Remove(db.employees.Find(id));
            db.SaveChanges();
            TempData["msg"] = "Employee of ID " + id + " has been deleted";
            return RedirectToAction("employeelist");
        }


        [HttpGet]
        
        public ActionResult addemployee()
        {
            return View();
        }
        [HttpPost]
        
        public ActionResult addemployee(addEmployeeDTO empModel)
        {
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerEntities();
                db.employees.Add(convert(empModel));
                db.SaveChanges();
                TempData["msg"] = "New employee is added successfully";
                return RedirectToAction("employeelist");
            }
            return View(empModel);
        }


        [HttpGet]
        
        public ActionResult editEmployee(int id)
        {
            var db = new ZeroHungerEntities();
            employee emp = db.employees.Find(id);
            editEmployeeDTO empDTO = new editEmployeeDTO()
            {
                id = emp.id,
                username = emp.username,
                name = emp.name,
                email = emp.email,
                phone = emp.phone,
                address = emp.address,
                dob = emp.dob
            };
            return View(empDTO);
        }
        [HttpPost]
       
        public ActionResult editEmployee(editEmployeeDTO empModel)
        {
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerEntities();
                employee emp = db.employees.Find(empModel.id);

                emp.username = empModel.username;
                emp.name = empModel.name;
                emp.email = empModel.email;
                emp.phone = empModel.phone;
                emp.address = empModel.address;
                emp.dob = empModel.dob;

                db.employees.AddOrUpdate(emp);
                db.SaveChanges();
                TempData["msg"] = "Information of employee of ID " + empModel.id.ToString() + " is edited successfully";
                return RedirectToAction("employeelist");
            }
            return View(empModel);
        }


        [HttpGet]
        public ActionResult requestlist()
        {
            var db = new ZeroHungerEntities();
            return View(db.requests.ToList());
        }

        [HttpGet]
        public ActionResult requestdetails(int id)
        {
            var db = new ZeroHungerEntities();
            ViewBag.empList = db.employees.ToList();
            return View(db.requests.Find(id));
        }

        [HttpPost]
        public ActionResult requestdetails(int id, assignEmpDTO obj)
        {
            var db = new ZeroHungerEntities();
            if (ModelState.IsValid)
            {
                employee emp = db.employees.Find(obj.emp_id);
                if (emp == null)
                {
                    TempData["msg"] = "Employee id does not exist";
                }
                else
                {
                    request rq = db.requests.Find(id);
                    rq.employee_id = obj.emp_id;
                    rq.admin_id = (int)Session["id"];
                    db.requests.AddOrUpdate(rq);
                    db.SaveChanges();
                    TempData["msg"] = emp.username + "has been assigned for request id " + id;
                    return RedirectToAction("requestlist");
                }
            }
            return View(db.requests.Find(id));
        }



    }

}
