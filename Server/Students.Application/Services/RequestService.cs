using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.Application.Services
{
    public class RequestService(IGenericRepository<Request> repository) : GenericService<Request>(repository), IRequestService
    {
    }
}