using Contracts;
using DAL.Data;
using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class AccountsRepository : RepositoryBase<Accounts>
    {
        //Inherit the Repository base

        //Contructor implements Repository base
        public AccountsRepository(DataContext context)
            : base(context)
        {

            //Check for context is defined
            if (context == null)
                throw new ArgumentException("Database context is not defined.");
        }

    }
}
