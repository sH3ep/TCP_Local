using System.Collections.Generic;

namespace TPC.Api.Features.Dto
{
    public class FeatureFilterDto
    {
        public IEnumerable<long> AssignedUsersId { set; get; }
        public IEnumerable<long> StatusesId { set; get; }
        public long ProjectId { set; get; }
    }
}
