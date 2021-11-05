using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Accounts
    {
        [Key]
        public int code { get; set; }
        public string account_number { get; set; }
        public decimal outstanding_balance { get; set; }
  
        // Foreign key 
        [Display(Name = "Name")]
        public int person_code { get; set; }

        [ForeignKey("person_code")]
        public virtual Person person { get; set; }

    } 


}
