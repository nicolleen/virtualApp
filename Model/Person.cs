using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Persons")]
    public class Person
    {
        [Key]
        public int code { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        [Required, StringLength(13)]
        public string id_number { get; set; }

    }


    public class personAccount
    {
        public virtual Person person { get; set; }
        [Key]
        public string account { get; set; }
    }
}
