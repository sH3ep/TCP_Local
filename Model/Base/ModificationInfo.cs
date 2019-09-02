using System;

namespace TPC.Api.Model.Base
{
    public class ModificationInfo : Entity
    {
        public DateTime ModificationDate { set; get; }
        public long ModifiedBy { get; set; }
    }
}
