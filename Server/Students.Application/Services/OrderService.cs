using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.DTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;
using Students.Models.ReferenceModels;

namespace Students.Application.Services
{
    public class OrderService(
        IOrderRepository orderRepository,
        IRequestRepository requestRepository,
        IStudentRepository studentRepository,
        IGroupStudentRepository groupStudentRepository,
        IGenericRepository<KindOrder> kindOrderRepository,
        ILogger<Order> logger)
        : GenericService<Order>(orderRepository, logger), IOrderService

    {
        /// <summary>
        /// Зачисление кандидатов по заявке, ЕСЛИ приказ о зачсилении
        /// </summary>
        /// <param name="form">Приказ с клиента</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<Order> CreateOrder(OrderDTO form)
        {
            var kindOrder = await kindOrderRepository.FindById(form.KindOrderId.Value);

            var request = await requestRepository.FindById(form.RequestId);
            if (request is null)
                throw new ArgumentException($"Не удалось найти заявку по id {form.RequestId}");
            if (request.PersonId is null)
                throw new Exception("Не нашлось персоны у заявки");

            var person = request.Person;
            if (kindOrder.Name.ToLower() == "о зачислении")
            {
                var newStudent = new Student()
                {
                    PersonId = person.Id
                };

                var student = await studentRepository.Create(newStudent);
                if (student is null)
                    throw new Exception("Не удалось создать студента");
                request.StudentId = student.Id;

                var groupStudent = await groupStudentRepository.Create(request, form.Groups!.FirstOrDefault()!.Id);
                if (groupStudent is null)
                    throw new ArgumentException("Не удалось создать groupStudent");

                var order = Mapper.OrderDTOToOrder(form);

                return order;
            }
            return Mapper.OrderDTOToOrder(form);
        }
    }
}