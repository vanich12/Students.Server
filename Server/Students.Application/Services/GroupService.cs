using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.Application.Services
{
    public class GroupService(
        IGroupStudentRepository studentInGroupRepository,
        IStudentRepository studentRepository,
        IGroupRepository groupRepository,
        IRequestRepository requestRepository,
        ILogger<Group> logger)
        : GenericService<Group>(groupRepository, logger)
    {
        /// <summary>
        /// Добавление Персоны в группу с превращением в студента, персона становится студентом , после добавления в группу
        /// </summary>
        /// <param name="requestsList"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Guid>?> AddStudentsToGroupByRequest(IEnumerable<Guid> requestsList, Guid groupId)
        {
            try
            {
                var group = await groupRepository.FindById(groupId);
                if (group is null)
                    return null;

                var bagRequestsIds = new List<Guid>();
                foreach (var requestId in requestsList)
                {
                    var request = await requestRepository.FindById(requestId);
                    if (request.StudentId is null)
                    {
                        var newStudent = new Student() { PersonId = request.PersonId };
                        var student = await studentRepository.Create(newStudent);
                        if (student is null)
                            throw new InvalidOperationException("Error while try create student");
                    }

                    if (request?.StudentId is null || request.EducationProgramId != group.EducationProgramId ||
                        await studentInGroupRepository.Create(request, groupId) is null)
                        bagRequestsIds.Add(requestId);
                }

                return bagRequestsIds;
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }
    }
}