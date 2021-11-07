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
        IRepositoryBase<Accounts> _accounts;
        
        //

        public PersonController(IRepositoryBase<Person> persons, IRepositoryBase<Accounts> accounts)
        {
            _persons = persons;
            _accounts = accounts;
            
        }

        // GET: Person Details
        public ActionResult Index()
        {
            var model = _persons.GetAll();
           
            return View(model);
        }

        //Person List
        public ActionResult PersonList()
        {
            var model = _persons.GetAll();
            var accModel = _accounts.GetAll();

            List<personAccount> termsList = new List<personAccount>();

            foreach (var user in model)
            {
                var item = accModel.Where(x => x.person_code == user.code)?.Select(x => x.account_number);
                var accountNumber = string.Join(",", item);
                var paItem = new personAccount();
                paItem.account = accountNumber;
                paItem.person = user;
                termsList.Add(paItem);
            };

            return View(termsList);
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


        
        public ActionResult Delete(int id)
        {
            var obj = _persons.GetByID(id);
            return View(obj);
        }

        // Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(Include = "code,name,surname,id_number")] Person person)
        {
            ValidatePersonDelete(person.code);
            if (ModelState.IsValid)
            {
                _persons.Delete(person);
                _persons.Save();
               return RedirectToAction("Index");
            }
            return View(person);
        }

        void ValidatePersonDelete(int code)
        {
            var isAccountActive = _accounts.GetAll().Where(x => x.person_code == code);
            if (isAccountActive.Count() > 0) {
                ModelState.AddModelError("name", "Account is Active.");
            }
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