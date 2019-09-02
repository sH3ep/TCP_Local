using System;
using TPC.Api.Model.Base;

namespace TPC.Api.Shared
{
    public class ModificationService : IModificationService
    {
        private readonly ICurrentUserService _currentUserService;
        public ModificationService(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public void UpdateModifiedByModifiedDate(ModificationInfo entity)
        {
            var userId = _currentUserService.GetCurrentUserId();

            entity.ModifiedBy = userId;
            entity.ModificationDate = DateTime.Now;
        }
    }
}
