using TPC.Api.Model.Base;

namespace TPC.Api.Model
{
    public class UserStory : ModificationInfo
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public long FeatureId { set; get; }
        public long AssignedUserId { set; get; }
        public long StatusId { set; get; }
        public long PriorityId { set; get; }
    }
}