using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Students.Application.Services.Interfaces;
using Students.DBCore.Contexts;
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
        StudentContext context,
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
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                if (form.KindOrderId == null)
                {
                    // Если здесь просто нужно вернуть результат маппинга,
                    // то транзакция не нужна, но для консистентности можно оставить.
                    return await Mapper.OrderDTOToOrder(form, kindOrderRepository);
                }

                // --- Все ваши проверки и получение данных ---
                var kindOrder = await kindOrderRepository.FindById(form.KindOrderId.Value);
                if (kindOrder is null) throw new KeyNotFoundException("Вид приказа не найден."); 

                var request = await requestRepository.FindById(form.RequestId);
                if (request is null) throw new KeyNotFoundException($"Не удалось найти заявку по id {form.RequestId}");
                if (request.PersonId is null) throw new InvalidOperationException("Не нашлось персоны у заявки");

                var group = await groupRepository.FindById(form.GroupId);
                if (group is null) throw new KeyNotFoundException($"Не удалось найти группу по Id {form.GroupId}");

                var person = request.Person;

                if (kindOrder?.Name?.Equals("о зачислении", StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (request.StudentId is null)
                    {
                        var newStudent = new Student { PersonId = person.Id };
                        var student = await studentRepository.Create(newStudent); 
                        request.StudentId = student.Id;
                    }

                    request.StatusRequestId = (await requestStatusRepository.GetOne(x => x.Name == "Обучение"))?.Id;
                    var groupStudent = await groupStudentRepository.FindById(request.Id);
                    if (groupStudent is null)
                    {
                        groupStudent = await groupStudentRepository.Create(request, group.Id); 
                    }
                    groupStudent.IsArchive = false;
                }

              
                if (kindOrder?.Name?.Equals("об отчислении", StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (request.GroupStudent is null) throw new NullReferenceException("У заявки отсутствует GroupStudent.");

                    var groupStudent = await groupStudentRepository.FindById(request.Id);

                    if (groupStudent is null) throw new KeyNotFoundException($"Не удалось найти группу по Id {form.GroupId}");
                    
                    groupStudent.IsArchive = true; 
                    var expelledStatus = await requestStatusRepository.GetOne(s => s.Name == "Отчислен");

                    if (expelledStatus != null)
                        request.StatusRequestId = expelledStatus.Id; 
                }

                if (request.GroupStudent != null)
                    await groupStudentRepository.Update(request.Id, request.GroupStudent);
                

                var newOrder = await Mapper.OrderDTOToOrder(form, kindOrderRepository);
                var order = await orderRepository.Create(newOrder);

                await requestRepository.Update(request.Id, request);

                // Здесь EF Core сам сохраняет все изменения.
                // Если у вас нет явного SaveChangesAsync() в репозиториях,
                // то его нужно вызвать здесь.
                // await dbContext.SaveChangesAsync();

                // Если мы дошли до сюда без ошибок, применяем все изменения в базе данных
                await transaction.CommitAsync();

                return order;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Произошла ошибка при создании приказа. Откатываем транзакцию.");

                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}