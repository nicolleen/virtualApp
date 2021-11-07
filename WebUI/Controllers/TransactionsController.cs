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
            accountObj = _accounts.GetByID(id);
            var model = _transactions.GetAll().Where(x => x.account_code == id);
            ViewData["PersonId"] = accountObj.person_code;
            ViewData["AccountId"] = id;
            ViewData["isActive"] = accountObj.active ?? false;
            return View(model);
        }

        //Create New Transactions
        public ActionResult CreateTransaction(int id)
        {
            var model = new Transactions();
            model.account_code = accountObj.code;
            ViewData["PersonId"] = accountObj.person_code;
            ViewData["AccountId"] = accountObj.code;
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateTransaction([Bind(Include = "account_code,transaction_date,capture_date,amount,description")] Transactions transactions)
        {
            SetCaptureDate(transactions);
            _transactions.Insert(transactions);
            _transactions.Save();
            TransactionBuilder(transactions);
            return RedirectToAction("Index", new { id = transactions.account_code });
        }

        //Create Edit Transactions
        public ActionResult Edit(int id)
        {
            var obj = _transactions.GetByID(id);
            ViewData["PersonId"] = accountObj.person_code;
            ViewData["AccountId"] = accountObj.code;
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "code,account_code,transaction_date,capture_date,amount,description")] Transactions transactions)
        {
            SetCaptureDate(transactions);
            _transactions.Update(transactions);
            _transactions.Save();
            TransactionBuilder(transactions);
            return RedirectToAction("Index", new { id = transactions.account_code });
        }

        public void TransactionBuilder(Transactions transactions)
        {
            try
            {
                var accountModel = _accounts.GetByID(transactions.account_code);
                var transactionlist = _transactions.GetAll().Where(x => x.account_code == transactions.account_code);
                decimal total = 0;
                foreach (var item in transactionlist)
                {
                    total += item.amount;
                }
                accountModel.outstanding_balance = total;
                _accounts.Update(accountModel);
                _accounts.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

        }

        public void SetCaptureDate(Transactions transactions)
        {
            DateTime now = DateTime.Now;
            transactions.capture_date = now;
        }

    }
}