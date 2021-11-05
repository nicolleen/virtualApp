using Contracts;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace Model
{
    public class PersonService
    {

        IRepositoryBase<Person> _persons;
       

        public PersonService(IRepositoryBase<Person> persons,ModelStateDictionary modelState)
        {
            _persons = persons;
            
        }

        public bool ValidatePersonId(string id)
        {
            var index = _persons.GetAll().Where(x => x.id_number == id);
            return index.Count() == 0;
        }

    }
}
