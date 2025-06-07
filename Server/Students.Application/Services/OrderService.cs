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
        IGroupRepository groupRepository,
        IGenericRepository<KindOrder> kindOrderRepository,
        IGenericRepository<StatusRequest> requestStatusRepository,
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
            if (form.KindOrderId != null)
            {
                var kindOrder = await kindOrderRepository.FindById(form.KindOrderId.Value);

                var request = await requestRepository.FindById(form.RequestId);
                if (request is null)
                    throw new ArgumentException($"Не удалось найти заявку по id {form.RequestId}");
                if (request.PersonId is null)
                    throw new Exception("Не нашлось персоны у заявки");

                var group = await groupRepository.FindById(form.GroupId);
                if (group is null)
                    throw new Exception($"Не удалось найти группу по Id {form.GroupId}");
                var person = request.Person;

                if (kindOrder?.Name?.ToLower() == "о зачислении")
                {
                    if (request.StudentId is null)
                    {
                        var newStudent = new Student()
                        {
                            PersonId = person.Id
                        };

                        var student = await studentRepository.Create(newStudent);
                        if (student is null)
                            throw new Exception("Не удалось создать студента");
                        request.StudentId = student.Id;
                    }

                    request.StatusRequestId = (await requestStatusRepository.GetOne(x => x.Name == "Обучение"))?.Id;
                    var groupStudent = await groupStudentRepository.FindById(request.Id);
                    if (groupStudent is null)
                        groupStudent = await groupStudentRepository.Create(request, group.Id);
                    request.GroupStudent.IsArchive = false;

                    if (groupStudent is null)
                        throw new ArgumentException("Не удалось создать groupStudent");
                }

                if (kindOrder?.Name?.ToLower() == "об отчислении")
                {
                    //var groupStudent = await groupStudentRepository.FindByStudentInGroup(request.);
                    if (request.GroupStudent is null)
                        throw new NullReferenceException(
                            "у заявки остутсвует GroupStudent (по крайней мере на уровне EF)");

                    request.GroupStudent.IsArchive = true;
                    var requestStatusId = request.StatusRequestId.Value;
                    request.StatusRequestId =
                        (await requestStatusRepository.GetOne(s => s.Name != null && s.Name == "Отчислен"))
                        ?.Id ??
                        requestStatusId;
          
                }
                var newGroupStudent = await groupStudentRepository.Update(request.Id, request.GroupStudent);
                if (newGroupStudent is null)
                    throw new InvalidOperationException("Не удалось создать приказ");

                var newOrder = await Mapper.OrderDTOToOrder(form, kindOrderRepository);
                var order = await orderRepository.Create(newOrder);
                if (order is null)
                    throw new InvalidOperationException("Не удалось создать приказ");

                var newRequest = await requestRepository.Update(request.Id, request);
                if (newRequest is null)
                    throw new InvalidOperationException("Не удалось обновить заявку");

                return order;
            }

            return await Mapper.OrderDTOToOrder(form, kindOrderRepository);
        }
    }
}