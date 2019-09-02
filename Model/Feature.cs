using TPC.Api.Model.Base;

namespace TPC.Api.Model
{
    public class Feature : ModificationInfo
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public long ProjectId { get; set; }
        public long AssignedUserId { set; get; }
        public long StatusId { set; get; }
        public long PriorityId { set; get; }
    }
}