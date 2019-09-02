using System.Collections.Generic;

namespace TPC.Api.Tasks.Dto
{
    public class TaskFilterDto
    {
        public IEnumerable<long> AssignedUsersId { set; get; }
        public IEnumerable<long> StatusesId { set; get; }
        public IEnumerable<long> TaskTypesId { set; get; }
        public long ProjectId { set; get; }
    }
}