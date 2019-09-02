using TPC.Api.Model.Base;

namespace TPC.Api.Shared
{
    public interface IModificationService
    {
        void UpdateModifiedByModifiedDate(ModificationInfo entity);
    }
}
