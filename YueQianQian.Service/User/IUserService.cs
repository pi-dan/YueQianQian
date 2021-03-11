using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Model;

namespace YueQianQian.Service.User
{
    public interface IUserService
    {
        Task<IEnumerable<UserListOutputDto>> GetUserAsync();


        Task<UserEntity> GetUserAsync(string username, string password);

        Task<bool> ModifyPwdAsync(string newpwd);

    }
}
