using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.Tasks.Comments
{
    public class CommentRepository:Repository<Comment>,ICommentRepository
    {
       
        public CommentRepository(TcpContext context) : base(context)
        {
            
        }

        public async Task<bool> SetUnassignedByUserId(long userId)
        {
            await Task.Yield();
            var comments = Entities.Where(x => x.ModifiedBy == userId);
            foreach (var item in comments)
            {
                item.ModifiedBy = 0;
            }
            
            return true;
        }
    }
}