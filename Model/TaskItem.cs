using TPC.Api.Model.Base;

namespace TPC.Api.Model
{
    public class TaskItem : ModificationInfo
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public long SprintId { get; set; }
        public long UserStoryId { set; get; }
        public long AssignedUserId { set; get; }
        public long StatusId { set; get; }
        public long TaskTypeId { set; get; }
        public long PriorityId { set; get; }
    }
}
