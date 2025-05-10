using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.DBCore.Contexts;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.Infrastructure.Storages
{
    public class PersonRepository: GenericRepository<Person>, IPersonRepository
    {
        private readonly StudentContext _ctx;
        public PersonRepository(StudentContext context) : base(context)
        {
            this._ctx = context;
        }
    }
}
