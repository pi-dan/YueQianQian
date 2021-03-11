using AutoMapper;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Common.Attributes;
using YueQianQian.Common.Auth;
using YueQianQian.Common.Helpers;
using YueQianQian.Model;
using YueQianQian.Repository.User;

namespace YueQianQian.Service.User
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public UserService(IMapper mapper, IUserRepository userRepository, ICurrentUser currentUser)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }


        public async Task<UserEntity> GetUserAsync(string username, string password)
        {
            string md5pass = EncryHelper.MD5Encry(password);

            return await _userRepository.Where(it => it.UserName == username && it.Password == md5pass).FirstAsync();

        }
        //[Transaction(Propagation = Propagation.Nested)]
        public async Task<IEnumerable<UserListOutputDto>> GetUserAsync()
        {

            var userlist = _userRepository.Select.ToList();
            return _mapper.Map<IEnumerable<UserListOutputDto>>(userlist);
        }

        public async Task<bool> ModifyPwdAsync(string newpwd)
        {
            var user = await _userRepository.GetAsync(_currentUser.Id);
            user.Password = EncryHelper.MD5Encry(newpwd);
            return await _userRepository.UpdateAsync(user) >= 0;
        }
    }
}
