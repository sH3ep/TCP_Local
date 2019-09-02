using System.Collections.Generic;
using System.Threading.Tasks;
using TPC.Api.Model;
using TPC.Api.Model.Dto;
using TPC.Api.Shared;

namespace TPC.Api.Users
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> Get(long userId);
        Task<User> EditNameAndLastName(long userId,UserDto userDto);
        Task<ActionEffect> DeleteOwnAccount(long userId);

        Task<User> GetUserByEmail(string userEmail);
    }
}