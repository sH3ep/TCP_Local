using System.Collections.Generic;
using System.Threading.Tasks;
using TPC.Api.Model;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.Users
{
    public interface IUserRepository:IRepository<User>
    {
        Task<IEnumerable<Project>> GetAllOwnedProjects(long userId);
        Task<bool> SetDeleted(long userId);
        Task<User> GetByEmail(string userEmail);
    }
}