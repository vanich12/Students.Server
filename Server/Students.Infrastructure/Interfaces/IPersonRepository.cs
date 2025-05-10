using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Models;

namespace Students.Infrastructure.Interfaces
{
    public interface IPersonRepository:IGenericRepository<Person> 
    {
    }
}
