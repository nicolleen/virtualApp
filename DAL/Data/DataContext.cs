using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class DataContext : DbContext
    {
        /// <summary>
        /// Pass the name of connection string in the web.config
        /// or
        /// Explicitly declare the connection string to DB
        /// </summary>
        public DataContext()
            : base("DefaultConnection")
        {

        }

        /// <summary>
        /// Entities need to be persisted, declared here 
        /// i.e. physical tables of the DB
        /// </summary>
        public DbSet<Person> Persons { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Transactions> Transactions { get; set; }

        public System.Data.Entity.DbSet<Model.personAccount> personAccounts { get; set; }
    }
}
