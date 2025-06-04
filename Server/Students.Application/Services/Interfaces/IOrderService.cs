using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Infrastructure.DTO;
using Students.Models;

namespace Students.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(OrderDTO form);
    }
}
