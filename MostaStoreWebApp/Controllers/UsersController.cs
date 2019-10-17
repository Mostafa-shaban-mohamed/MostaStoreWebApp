using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MostaStoreWebApp.Models;
using System.Security.Cryptography;
using System.Web.Security;

namespace MostaStoreWebApp.Controllers
{
    public class UsersController : Controller
    {
        private MostaStoreDBEntities db = new MostaStoreDBEntities();

        // Index = Log In--------------------------------------------------------------------------------------------
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(User user)
        {
            string hashedpsswd = CreatePasswordHash(user.UPassword);
            var use = (from userlist in db.Users
                       where userlist.UName == user.UName && userlist.UPassword == hashedpsswd
                       select new
                       {
                           userlist.ID,
                           userlist.UName
                       }).ToList();
            if (use.FirstOrDefault() != null)
            {
                Session["UserName"] = use.FirstOrDefault().UName;
                Session["UserID"] = use.FirstOrDefault().ID;
                return Redirect("http://localhost:2215/Products/Index");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login credentials.");
            }
            return View(user);
        }
        //Login---------------------------------------------------------------------------------------------------------
        
        //Register---------------------------------------------------------------------
            // GET: Users/Create
            public ActionResult Create() //Register
            {
                return View();
            }

            // POST: Users/Create
            // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
            // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create([Bind(Include = "UName,UPassword")] User user)  //Register
            {
                if (ModelState.IsValid)
                {
                    user.ID = db.Users.Count();
                    string hashpsswd = CreatePasswordHash(user.UPassword);
                    user.UPassword = hashpsswd;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(user);
            }
        //Register------------------------------------------------------------------------    

        public virtual string CreatePasswordHash(string password, string saltkey = "Salt", string passwordFormat = "SHA1")
        {
            if (String.IsNullOrEmpty(passwordFormat))
                passwordFormat = "SHA1";
            string saltAndPassword = String.Concat(password, saltkey);
            string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, passwordFormat);
            return hashedPassword;
        }

    }
}
