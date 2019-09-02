using System.Collections.Generic;
using System.Threading.Tasks;
using TPC.Api.Model;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.Tasks.Comments
{
    public interface ICommentRepository:IRepository<Comment>
    {
        Task<bool> SetUnassignedByUserId (long userId);
    }
}