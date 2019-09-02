using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPC.Api.Model;
using TPC.Api.Model.Lookups;
using TPC.Api.Model.ManyToManyRelations;
using TPC.Api.Persistence;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.Projects
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly DbSet<UserProject> _userProjectsSet;
        private readonly DbSet<Role> _roles;
        private readonly IUnitOfWork _unitOfWork;
        //  private readonly ISprintRepository _sprintRepository;
        private readonly DbSet<User> _users;
        private readonly DbSet<Project> _projects;
        private readonly DbSet<Sprint> _sprints;

        public ProjectRepository(
            TcpContext context,
            //  ISprintRepository sprintRepository,  TODO !!
            IUnitOfWork unitOfWork

            ) : base(context)
        {
            _userProjectsSet = context.Set<UserProject>();
            _roles = context.Set<Role>();
            _unitOfWork = unitOfWork;
            //  _sprintRepository = sprintRepository;
            _users = context.Set<User>();
            _projects = context.Set<Project>();
            _sprints = context.Set<Sprint>();
        }

        public async Task Activate(long projectId, long userId)
        {
            var newActiveUserProject = await _userProjectsSet.FirstOrDefaultAsync(x =>
                x.ProjectId == projectId &&
                x.UserId == userId);

            if (newActiveUserProject != null)
            {
                DeactiveAllUserProjects(userId);
                newActiveUserProject.Active = true;
            }
        }

        private void DeactiveAllUserProjects(long userId)
        {
            var userProjects = _userProjectsSet.Where(x => x.UserId == userId);
            foreach(var item in userProjects)
            {
                item.Active = false;
               
            }
          
        }

        public async Task<UserProject> GetUserRole(long projectId, long userId)
        {
            var userProject = _userProjectsSet.FirstOrDefaultAsync(x => x.ProjectId == projectId && x.UserId == userId);
            return await userProject;
        }

        public async Task<Project> GetActiveByUserId(long userId)
        {
            var userProject = await _userProjectsSet.FirstOrDefaultAsync(x => x.UserId == userId && x.Active);
            if (userProject == null)
            {
                return null;
            }

            return await Entities.FirstOrDefaultAsync(x => x.Id == userProject.ProjectId);
        }

        public async Task<IEnumerable<Project>> GetAllByUserId(long userId)
        {
            var userProjects = await _userProjectsSet.Where(x => x.UserId == userId).Select(x => x.ProjectId)
                .ToListAsync();

            return Entities.Where(x => userProjects.Contains(x.Id));
        }

        public async Task<IEnumerable<User>> GetProjectUsers(long projectId)
        {
            DbSet<User> users = Context.Set<User>();

            var userIds = await _userProjectsSet.Where(x => x.ProjectId == projectId).Select(x => x.UserId).ToListAsync();
            return await users.Where(x => x.Email != null && userIds.Contains(x.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Sprint>> GetProjectSprints(long projectId)
        {
            return await _sprints.Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public async Task<UserProject> AddUserProject(long projectId, long userId)
        {

            var project = _projects.Where(x => x.Id == projectId);
            var user = _users.Where(x => x.Id == userId);
            if (!project.Any() || !user.Any())
                throw new Exception("Nie ma uzytkownika lub projektu o podanym Id");
            
            
            if(_userProjectsSet.Any(x => x.ProjectId==projectId && x.UserId==userId))
                throw new Exception("Taki użytkownik juz jest w projekcie");

            var userProject = new UserProject()
            {
                ProjectId = projectId,
                UserId = userId,
                RoleId = GetDefaultRole()
            };

            await _userProjectsSet.AddAsync(userProject);

            return userProject;
        }

        public void DeleteUserFromProject(long projectId, long userId)
        {
            var userProject = _userProjectsSet.FirstOrDefault(x =>
                x.ProjectId == projectId &&
                x.UserId == userId);

            if (userProject == null)
            {
                return;
            }

            _userProjectsSet.Remove(userProject);

            if (userProject.Active)
            {
                var userProjectToActivate = _userProjectsSet.FirstOrDefault(x => x.UserId == userId);
                if (userProjectToActivate != null)
                {
                    userProjectToActivate.Active = true;
                }
            }
        }

        public void EditUserRole(long projectId, long userId, long roleId)
        {
            var userProject = _userProjectsSet.FirstOrDefault(x =>
                x.ProjectId == projectId &&
                x.UserId == userId);

            if (userProject != null)
            {
                userProject.RoleId = roleId;
            }
        }

        public async Task DeleteProject(long projectId, long userId)
        {
            await Task.Yield();
            var projectToRemove = _projects.FirstOrDefault(x => x.Id == projectId);
            if (projectToRemove == null) throw new InvalidOperationException("Project doesn't exists");

            var userProjects = _userProjectsSet.Where(x => x.ProjectId == projectId).ToList();
            foreach (var item in userProjects)
            {
                _userProjectsSet.Remove(item);
            }

            await _unitOfWork.Complete();

            _projects.Remove(projectToRemove);

        }

        private long GetDefaultRole()
        {
            try
            {
                var temp = _roles.FirstOrDefault(x => x.Name == "Gosc");
                return temp.Id;
            }
            catch
            {
                _roles.Add(new Role() { Name = "Gosc" });
                _unitOfWork.Complete();
                var temp = _roles.FirstOrDefault(x => x.Name == "Gosc");
                return temp.Id;
            }

        }

        public async Task<bool> HasManyAdmins(long projectId)
        {
            var adminRole = await _roles.SingleAsync(x => x.Name == "Administrator");

            var userProjects =
                await _userProjectsSet.Where(x => x.ProjectId == projectId).ToListAsync();

            return userProjects.Count(x => x.RoleId == adminRole.Id) > 1;
        }

        public async Task<bool> SetUnassignedByUserId(long userId)
        {
            await Task.Yield();
            var projects = Entities.Where(x => x.ModifiedBy == userId);

            foreach (var item in projects)
            {
                item.ModifiedBy = 0;
            }

            return true;
        }

        public async Task<IEnumerable<UserProject>> GetUserProjectByProjectId(long projectId)
        {
            var userProjects = _userProjectsSet.Where(x => x.ProjectId == projectId);
            return userProjects;
        }

        public async Task<UserProject> AddUserProject(UserProject userProject)
        {
            await _userProjectsSet.AddAsync(userProject);
            return userProject;
        
        }
    }
}
