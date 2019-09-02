using System;
using TPC.Api.Model.Base;

namespace TPC.Api.Model
{
    public class Sprint : ModificationInfo
    {
        public long ProjectId { get; set; }
        public string SprintName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
