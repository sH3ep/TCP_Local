using TPC.Api.Model.Base;

namespace TPC.Api.Model
{
    public class Comment:ModificationInfo  
    {
        public string Text { set; get; }

        public long TaskItemId { set; get; }
    }
}