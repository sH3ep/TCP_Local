using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPC.Api.Features;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Tasks.Dto;
using TPC.Api.UserStories;

namespace TPC.Api.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IFeatureRepository _featureRepository;
        private readonly IUserStoryRepository _userStoryRepository;

        public TaskService(ITaskRepository taskRepository,IFeatureRepository featureRepository,IUserStoryRepository userStoryRepository, TcpContext context)
        {
            _taskRepository = taskRepository;
            _userStoryRepository = userStoryRepository;
            _featureRepository = featureRepository;
        }


        public async Task<IEnumerable<TaskItem>> GetFiltered(TaskFilterDto filter)
        {
            List<TaskItem> filteredTaskItems;

            if (filter.ProjectId > 0)
            {
                filteredTaskItems = (await GetTasksByProject(filter.ProjectId)).ToList();
            }
            else
            {
                filteredTaskItems = (await _taskRepository.GetAll()).ToList();
            }

            if (filter.AssignedUsersId != null && filter.AssignedUsersId.Any())
            {
                filteredTaskItems = filteredTaskItems.Where(x => filter.AssignedUsersId.Contains(x.AssignedUserId))
                    .ToList();
            }


            if (filter.StatusesId != null && filter.StatusesId.Any())
            {
                filteredTaskItems = filteredTaskItems.Where(x => filter.StatusesId.Contains(x.StatusId))
                    .ToList();
            }


            if (filter.TaskTypesId != null && filter.TaskTypesId.Any())
            {
                filteredTaskItems = filteredTaskItems.Where(x => filter.TaskTypesId.Contains(x.TaskTypeId))
                    .ToList();
            }

            return filteredTaskItems;
        }

        private async Task<IEnumerable<TaskItem>> GetTasksByProject(long projectId)
        {
            var features =  _featureRepository.GetAll().Result.Where(x => x.ProjectId == projectId).Select(x => x.Id).ToList();
            var userStories =  _userStoryRepository.GetAll().Result.Where(x => features.Contains(x.FeatureId)).Select(x => x.Id).ToList();
            return  _taskRepository.GetAll().Result.Where(x => userStories.Contains(x.UserStoryId)).ToList();
        }
    }
}
