using RailwayReservationAndManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace RailwayReservationAndManagement.Controllers
{
    public class HomeController : Controller
    {
        private dbRailwayReservationAndManagementSystemEntities _db = new dbRailwayReservationAndManagementSystemEntities();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your login page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string name, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = _db.Users.Where(s => s.Name.Equals(name) && s.Password.Equals(f_password)).FirstOrDefault();

                if (data != null)
                {
                    //add session
                    Session["LoginID"] = data.LoginID;
                    Session["Name"] = data.Name;
                    Session["Password"] = data.Password;
                    Session["UserType"] = data.UserType;

                    //Admin type = 0
                    if (data.UserType.Equals(0))
                    {
                        return RedirectToAction("Index", "Admin");
                    }

                    //User type
                    else if (data.UserType.Equals(1))
                    {
                        return RedirectToAction("Index", "User");
                    }
                }
                else 
                {
                    ViewBag.Error = "The username or password is incorrect";
                    return View("Login");
                }
            }
                return View();
         }

        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index");
        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }



    }
}