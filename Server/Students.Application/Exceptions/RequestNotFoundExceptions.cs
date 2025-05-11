using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Application.Exceptions
{
    public class RequestNotFoundExceptions : Exception
    {
        public RequestNotFoundExceptions(Guid requestId)
            : base($"Request with ID '{requestId}' not found.")
        {
            RequestId = requestId;
        }

        public Guid RequestId { get; }
    }
}