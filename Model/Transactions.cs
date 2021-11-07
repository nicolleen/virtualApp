using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Transactions
    {
        [Key]
        public int code { get; set; }
        public DateTime transaction_date { get; set; }
        public DateTime capture_date { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public decimal amount { get; set; }
        public string description { get; set; }

        // Foreign key 
        [Display(Name = "account_number")]
        public int account_code { get; set; }

        [ForeignKey("account_code")]
        public virtual Accounts account{ get; set; }

    }
}
