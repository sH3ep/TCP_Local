using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPC.Api.Comments;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Shared;
using TPC.Api.Tasks.Comments;

namespace TPC.Api.Users
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : CrudController<Comment, ICommentRepository>
    {
        private readonly ICommentService _commentService;
        private readonly IModificationService _modificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentController(IUnitOfWork unitOfWork, 
            ICommentRepository repository, 
            IMapper mapper,
            ICommentService commentService,
            IModificationService modificationService) : base(unitOfWork, repository, mapper, modificationService)
        {
            _commentService = commentService;
            _modificationService = modificationService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllByTaskId([FromRoute] long taskId)
        {
            try
            {
                var comments = await _commentService.GetAllByTaskId(taskId);

                return Ok(comments);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
           
        }
    }
}
        
