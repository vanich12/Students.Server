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
    public class StudentHistoryRepository:GenericRepository<PersonHistory>, IPersonHistory
    {
        public StudentHistoryRepository(StudentContext context) : base(context)
        {
        }
    }
}
