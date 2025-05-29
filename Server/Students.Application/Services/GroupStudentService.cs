using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.Application.Services
{
    public class GroupStudentService(
        IGroupStudentRepository studentInGroupRepository,
        IStudentRepository studentRepository,
        IGroupRepository groupRepository,
        IRequestRepository requestRepository,
        ILogger<GroupStudent> logger)
        : GenericService<GroupStudent>(studentInGroupRepository, logger)
    {
        //public async Task<GroupStudent?> AddStudentsToGroupByRequest(IEnumerable<Guid> requestsList, Guid groupId)
        //{
        //    var group = await groupRepository.FindById(groupId);
        //    if (group is null)
        //        return null;

        //    var bagRequestsIds = new List<Guid>();
        //    foreach (var requestId in requestsList)
        //    {
        //        var request = await requestRepository.FindById(requestId);
                
        //        if (request?.StudentId is null || request.EducationProgramId != group.EducationProgramId ||
        //            await studentInGroupRepository.Create(request, groupId) is null)
        //            bagRequestsIds.Add(requestId);
        //    }


        //}
    }
}