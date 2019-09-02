using System.Collections.Generic;
using System.Threading.Tasks;
using TPC.Api.Model;

namespace TPC.Api.Comments
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllByTaskId(long taskId);
    }
}