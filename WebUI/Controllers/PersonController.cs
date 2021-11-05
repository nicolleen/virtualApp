using Contracts;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace WebUI.Controllers
{
    public class PersonController : Controller
    {

        IRepositoryBase<Person> _persons;
        
        //Constructor
        public PersonController(IRepositoryBase<Person> persons)
        {
            _persons = persons;
            

        }

        // GET: Person Details
        public ActionResult Index()
        {
            var model = _persons.GetAll();
            return View(model);
        }

        //Create new Person
        public ActionResult CreatePerson()
        {
            var model = new Person();
            return View(model);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePerson([Bind(Include = "code,name,surname,id_number")] Person person)
        {
            ValidatePersonId(person);
            if (ModelState.IsValid)
            {
                _persons.Insert(person);
                _persons.Save();
                return RedirectToAction("Index");
            }
            return View(person);
        }


        public ActionResult Edit(int Id)
        {
            var obj = _persons.GetByID(Id);
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "code,name,surname,id_number")] Person person)
        {
            ValidatePersonId(person);
            if (ModelState.IsValid)
            {
                _persons.Update(person);
                _persons.Save();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        void ValidatePersonId(Person person)
        {
            var index = _persons.GetAll().Where(x => x.id_number == person.id_number && x.code !=  person.code);
            if (index.Count() > 0)
            {
                ModelState.AddModelError("id_number", "Duplicate Id Number.");
            }
        }
    }
}