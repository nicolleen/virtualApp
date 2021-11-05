﻿using Contracts;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class AccountsController : Controller
    {
        IRepositoryBase<Accounts> _accounts;
        IRepositoryBase<Person> _persons;

        //Constructor
        public AccountsController(IRepositoryBase<Accounts> accounts, IRepositoryBase<Person> persons)
        {
            _accounts = accounts;
            _persons = persons;
        }

        // GET: Accounts
        public ActionResult Index(int id)
        {
            var person = _persons.GetByID(id);
            var account = _accounts.GetAll().Where(x => x.person_code == id);
            ViewData["Person"] = person.name+" "+person.surname;
            ViewData["PersonId"] = person.code;
            return View(account);
        }

        public ActionResult Edit(int id)
        {
            var obj = _accounts.GetByID(id);
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "code,account_number,outstanding_balance,person_code")] Accounts accounts)
        {
            ValidateAccountNumber(accounts);
            if (ModelState.IsValid)
            {
                _accounts.Update(accounts);
                _accounts.Save();
                return RedirectToAction("Index", new { id = accounts.person_code });
            }
            return View(accounts);
        }

        //Create new account
        public ActionResult CreateAccount(int id)
        {
            var model = new Accounts();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateAccount([Bind(Include = "account_number,outstanding_balance,person_code")] Accounts account)
        {
            ValidateAccountNumber(account);
            if (ModelState.IsValid)
            {
                _accounts.Insert(account);
                _accounts.Save();
                return RedirectToAction("Index", new { id = account.person_code });
            }
            return View(account);
        }

        void ValidateAccountNumber(Accounts account)
        {
            var index = _accounts.GetAll().Where(x => x.account_number == account.account_number && x.code != account.code);

            if (index.Count() > 0)
            {
                ModelState.AddModelError("account_number", "Duplicate Account Number.");
            }
        }

       

    }
}