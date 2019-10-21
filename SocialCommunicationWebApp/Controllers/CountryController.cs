using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialCommunicationWebApp.Models;

namespace SocialCommunicationWebApp.Controllers
{
    public class CountryController : Controller
    {
        private ApplicationDbContext _context;

        public CountryController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Country
        public ActionResult Index()
        {
            var c = _context.Countries.ToList();
            return View(c);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(Country country)
        {
            if (!ModelState.IsValid)
            {
                return View("New");
            }
            _context.Countries.Add(country);
            _context.SaveChanges();
            return RedirectToAction("Index", "Country");

        }

        public ActionResult Edit(int? id)
        {
            var country = _context.Countries.SingleOrDefault(x => x.Id == id);
            return View(country);
        }

        [HttpPost]
        public ActionResult Edit(Country country)
        {
            var countryInDb = _context.Countries.Single(x => x.Id == country.Id);

            countryInDb.Name = country.Name;
            _context.SaveChanges();
            return RedirectToAction("Index", "Country");
        }

        public ActionResult Delete(int id)
        {
            var countryInDb = _context.Countries.SingleOrDefault(c => c.Id == id);
            if (countryInDb != null)
            {
                _context.Countries.Remove(countryInDb);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}