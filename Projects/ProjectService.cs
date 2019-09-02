using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TPC.Api.Features;
using TPC.Api.Model;
using TPC.Api.Model.ManyToManyRelations;
using TPC.Api.Persistence;
using TPC.Api.Shared;

namespace TPC.Api.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleCheck _roleCheck;
        private readonly IFeatureRepository _featureRepository;

        public ProjectService(IProjectRepository projectRepository, IUnitOfWork unitOfWork, IRoleCheck roleCheck, IFeatureRepository featureRepository)
        {
            _projectRepository = projectRepository;
            _unitOfWork = unitOfWork;
            _roleCheck = roleCheck;
            _featureRepository = featureRepository;
        }

        public async Task<Project> CreateNewProject(long userId, Project project)
        {
            var addedItem = _projectRepository.Add(project);
            await _unitOfWork.Complete();

            var userProject = new UserProject
            {
                UserId = userId,
                ProjectId = project.Id,
                Active = false,
                RoleId = 1
            };
            await _projectRepository.AddUserProject(userProject);
            await _unitOfWork.Complete();

            await _projectRepository.Activate(addedItem.Id, userId);

            await _unitOfWork.Complete();
            return project;

        }

        public async Task<Project> Add(Project item, long userId)
        {
            var addedItem = _projectRepository.Add(item);
            await _unitOfWork.Complete();

            await _projectRepository.AddUserProject(addedItem.Id, userId);

            var activeProject = await _projectRepository.GetActiveByUserId(userId);

            await _unitOfWork.Complete();

            if (activeProject == null)
            {
                await _projectRepository.Activate(addedItem.Id, userId);
            }

            await _unitOfWork.Complete();

            return addedItem;
        }

        public async Task<UserProject> AddUserProject(long projectId, long userId)
        {
            var project = _projectRepository.Get(projectId);
            var userProjects = _projectRepository.GetAllByUserId(userId).Result.Where(x => x.Id == projectId);
            if (!userProjects.Any() && project != null)
            {
                var temp = await _projectRepository.AddUserProject(projectId, userId);
                await _unitOfWork.Complete();
                return temp;
            }
            else
            {
                throw new Exception("Taki uzytkownik juz istnieje w tym projekcie lub taki projekt nie istnieje");
            }

        }

        public async Task<IEnumerable<Project>> GetAllByUserId(long userId)
        {
            var temp = await _projectRepository.GetAllByUserId(userId);
            return temp.ToList();
        }

        public async Task DeleteProject(long projectId, long userId)
        {
            if (!await _roleCheck.IsAdmin(userId, projectId)) throw new InvalidOperationException("Only Administrator can delete project");

            var activeProject = await _projectRepository.GetActiveByUserId(userId);
            if (activeProject.Id == projectId) throw new InvalidOperationException("Cannot delete active project");



            await _projectRepository.DeleteProject(projectId, userId);
            await _unitOfWork.Complete();
        }

        public async Task<long> GetUserRole(long projectId, long userId)
        {
            var userProjects = await _projectRepository.GetUserRole(projectId, userId);
            return userProjects.RoleId;
        }

        public async Task<ActionEffect> DeleteUserFromProject(long projectId, long userIdToDelete, long loggedUserId)
        {
            if (await _roleCheck.IsAdmin(loggedUserId, projectId))
            {
                var projectUsers = await _projectRepository.GetUserProjectByProjectId(projectId);
                var amountOfAdmins = await _roleCheck.CountAdmins(projectUsers.ToList());
                if (!(userIdToDelete == loggedUserId && amountOfAdmins < 2))
                {
                    _projectRepository.DeleteUserFromProject(projectId, userIdToDelete);
                    await _featureRepository.SetUnassignedByUserId(userIdToDelete, projectId);
                    await _unitOfWork.Complete();
                    return new ActionEffect();
                }
                return new ActionEffect("Nie można usunąć ostatniego Administratora z projektu");

            }

            if (userIdToDelete == loggedUserId)
            {
                _projectRepository.DeleteUserFromProject(projectId, userIdToDelete);
                await _featureRepository.SetUnassignedByUserId(userIdToDelete, projectId);
                await _unitOfWork.Complete();
                return new ActionEffect();
            }

            return new ActionEffect("Brak uprawnien");
        }

        public async Task<ActionEffect> EditUserRole(long projectId, long userId, long roleId, long loggedUserId)
        {
            if (await _roleCheck.IsAdmin(loggedUserId, projectId))
            {
                if (userId == loggedUserId)
                    return new ActionEffect("Nie możesz zmienić swojej roli");

                _projectRepository.EditUserRole(projectId, userId, roleId);
                return new ActionEffect();
            }
            return new ActionEffect("Brak uprawnien do wykonania tej operacji");
        }

        public async Task<ActionEffect> CanLeaveProject(long userId, long projectId)
        {
            var projectUsers = await _projectRepository.GetUserProjectByProjectId(projectId);

            if (!await _roleCheck.IsAdmin(userId, projectId))
                return new ActionEffect();

            var amountOfAdmins = await _roleCheck.CountAdmins(projectUsers.ToList());
            if (amountOfAdmins > 1)
                return new ActionEffect();

            return new ActionEffect("Jesli opuscisz projekt zostanie on usuniety");
        }
    }
}