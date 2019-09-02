using TPC.Api.Model.Base;

namespace TPC.Api.Projects.Dto
{
    public class ProjectDto : ModificationInfo
    {
        public string Title { get; set; }
        public bool Active { get; set; }
    }
}
