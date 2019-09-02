using System.Collections.Generic;

namespace TPC.Api.UserStories.Dto
{
    public class UserStoryFilterDto
    {
        public IEnumerable<long> AssignedUsersId { set; get; }
        public IEnumerable<long> StatusesId { set; get; }
        public long FeatureId { set; get; }
    }
}
