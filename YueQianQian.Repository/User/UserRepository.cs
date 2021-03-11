using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Model;

namespace YueQianQian.Repository.User
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public UserRepository(IFreeSql fsql, UnitOfWorkManager uowm) : base(fsql, uowm)
        {
        }
    }
}
