using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using PagedList;
using PagedList.Mvc;
using System.Web;
using System.Web.Mvc;
using MostaStoreWebApp.Models;
using System.IO;
using System.Text;
using System.Web.Helpers;

namespace MostaStoreWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private MostaStoreDBEntities db = new MostaStoreDBEntities();

        // GET: Products
        public ActionResult Index() //Datagrid
        {
            
            return View(db.Products.ToList().OrderBy(e => e.ID));
        }
        [HttpPost]
        public ActionResult Index(string SearchName, string sortOrder,FormCollection CurrentSort)
        {
            

            var product = from p in db.Products select p;
            //Sorting ---------------------------------
            sortOrder = Request.Form["Sorting"].ToString();
            if(sortOrder == "ModelName")
            {
                product = product.OrderBy(m => m.ModelName);
            }
            else if (sortOrder == "Quantity")
            {
                product = product.OrderBy(m => m.Quantity);
            }
            else if (sortOrder == "SIMCard")
            {
                product = product.OrderBy(m => m.SIMCard);
            }
            else if (sortOrder == "FrontCam")
            {
                product = product.OrderBy(m => m.FrontCam);
            }
            else if (sortOrder == "BackCam")
            {
                product = product.OrderBy(m => m.BackCam);
            }
            else if (sortOrder == "Screen")
            {
                product = product.OrderBy(m => m.Screen);
            }
            else if (sortOrder == "PhoneSize")
            {
                product = product.OrderBy(m => m.PhoneSize);
            }
            else if (sortOrder == "RAM")
            {
                product = product.OrderBy(m => m.RAM);
            }
            //Searching --------------------------------------------------
            if (!String.IsNullOrEmpty(SearchName))
            {
                product = product.Where(s => s.ModelName.Contains(SearchName));
            }
            

            return View(product);
        }
        //Exporting-----------------------------------------------------------
        public void ExportCSV()
        {
            MostaStoreDBEntities db = new MostaStoreDBEntities();
            StringWriter sw = new StringWriter();

            sw.WriteLine("\"Model Name\",\"Quantity\",\"SIMCard\",\"FrontCam\",\"BackCam\",\"Screen\",\"PhoneSize\",\"RAM\"");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Smartphones.csv");
            Response.ContentType = "text/csv";

            foreach (var line in db.Products.ToList())
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\"",
                                           line.ModelName,
                                           line.Quantity,
                                           line.SIMCard,
                                           line.FrontCam,
                                           line.BackCam,
                                           line.Screen,
                                           line.PhoneSize,
                                           line.RAM));
            }

            Response.Write(sw.ToString());

            Response.End();
        }
        
        

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return PartialView("Details", product);
        }

        // GET: Products/Register
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ModelName,Quantity,SIMCard,FrontCam,BackCam,Screen,PhoneSize,RAM")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ModelName,Quantity,SIMCard,FrontCam,BackCam,Screen,PhoneSize,RAM")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
