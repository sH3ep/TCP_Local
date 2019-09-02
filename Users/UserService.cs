using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPC.Api.Features;
using TPC.Api.Model;
using TPC.Api.Model.Dto;
using TPC.Api.Persistence;
using TPC.Api.Projects;
using TPC.Api.Shared;
using TPC.Api.Sprints;
using TPC.Api.Tasks;
using TPC.Api.Tasks.Comments;
using TPC.Api.UserStories;

namespace TPC.Api.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectService _projectService;
        private readonly IRoleCheck _roleCheck;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskService _taskService;
        private readonly ITaskRepository _taskRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserStoryRepository _userStoryRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ISprintRepository _sprintRepository;
        private readonly IFeatureRepository _featureRepository;

        public UserService(IUserRepository userRepository,
            IProjectService projectService,
            IRoleCheck roleCheck,
            IUnitOfWork unitOfWork,
            ITaskService taskService,
            ITaskRepository taskRepository,
            ICommentRepository commentRepository,
            IUserStoryRepository userStoryRepository,
            IProjectRepository projectRepository,
            ISprintRepository sprintRepository,
            IFeatureRepository featureRepository)
        {
            _userStoryRepository = userStoryRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _projectService = projectService;
            _roleCheck = roleCheck;
            _unitOfWork = unitOfWork;
            _taskService = taskService;
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _sprintRepository = sprintRepository;
            _featureRepository = featureRepository;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var usersList = await _userRepository.GetAll();
            return usersList.Where(x=>x.Email != null).ToList();
        }

        public async Task<User> Get(long userId)
        {
            var user = await _userRepository.Get(userId);
            return user;
        }

        public async Task<User> EditNameAndLastName(long userId, UserDto userDto)
        {
            var user = await _userRepository.Get(userId);
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            _userRepository.Update(user);
            return user;

        }


        public async Task<ActionEffect> DeleteOwnAccount(long userId)
        {
            await _taskRepository.SetUnassignedByUserId(userId);
            await _userStoryRepository.SetUnassignedByUserId(userId);
            await _featureRepository.SetUnassignedByUserId(userId);
            var user = await _userRepository.Get(userId);
            if (user == null)
            {
                return new ActionEffect("Nie znaleziono uzytkownika o podanym id");
            }
            if (await _userRepository.SetDeleted(userId))
            {
                await _unitOfWork.Complete();
                return new ActionEffect();
            }
            return new ActionEffect("Nie udalo sie usunac uzytkownika");
        }

        public async Task<User> GetUserByEmail(string userEmail)
        {
            var user = await _userRepository.GetByEmail(userEmail);
            return user;
        }
    }
}
