using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Tasks.Comments;

namespace TPC.Api.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(ICommentRepository commentRepository,IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<Comment>> GetAllByTaskId(long taskId)
        {
            var comments = await _commentRepository.GetAll();
            return comments.Where(x => x.TaskItemId == taskId).ToList();
        }
    }
}