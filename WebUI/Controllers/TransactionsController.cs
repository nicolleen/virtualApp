using Contracts;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class TransactionsController : Controller
    {

        IRepositoryBase<Transactions> _transactions;
        IRepositoryBase<Accounts> _accounts;
        public static Accounts accountObj;

        //Constructor
        public TransactionsController(IRepositoryBase<Transactions> transactions, IRepositoryBase<Accounts> accounts)
        {
            _transactions = transactions;
            _accounts = accounts;
        }

        // GET: Transactions
        public ActionResult Index(int id)
        {
            var account = _accounts.GetByID(id);
            var model = _transactions.GetAll().Where(x => x.account_code == id);
            ViewData["AccountId"] = id; 
            return View(model);
        }

        //Create New Transactions
        public ActionResult CreateTransaction(int id)
        {
            var model = new Transactions();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateTransaction([Bind(Include = "account_code,transaction_date,capture_date,amount,description")] Transactions transactions)
        {
            _transactions.Insert(transactions);
            _transactions.Save();

            var accountModel = _accounts.GetByID(transactions.account_code);
            decimal total = 0;
            foreach (var item in _transactions.GetAll())
                total += item.amount;
            accountModel.outstanding_balance = total;
            _accounts.Update(accountModel);
            _accounts.Save();

            return RedirectToAction("Index", new { id = transactions.account_code });
        }

        //Create Edit Transactions
        public ActionResult Edit(int id)
        {
            var obj = _transactions.GetByID(id);
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "code,account_code,transaction_date,capture_date,amount,description")] Transactions transactions)
        {
            _transactions.Update(transactions);
            _transactions.Save();

            var accountModel = _accounts.GetByID(transactions.account_code);
            var latestTransactions = _transactions.GetAll().Where(x => x.code == transactions.code);
            decimal total = 0;
            foreach (var item in latestTransactions)
                total += item.amount;
            accountModel.outstanding_balance = total;
            _accounts.Update(accountModel);
            _accounts.Save();

            return RedirectToAction("Index", new { id = transactions.account_code });
        }

    }
}