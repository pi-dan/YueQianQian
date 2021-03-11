using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YueQianQian.Common.Auth;
using YueQianQian.Common.Extensions;
using YueQianQian.Common.Helpers;
using YueQianQian.Model;
using YueQianQian.Service.User;

namespace YueQianQian.WebApi.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Authorize]
    [Route("Api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        public UserController(IMapper mapper, ICurrentUser currentUser, IUserService userService)
        {
            this._userService = userService;
            this._mapper = mapper;
            this._currentUser = currentUser;
        }

        /// <summary>
        /// 所有用户
        /// </summary>
        /// <returns></returns>
        [Route(nameof(Users))]
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            if (_currentUser.IsAdmin == "N")
            {
                return toResponse(StatusCodeType.Forbidden);
            }
            var users = await _userService.GetUserAsync();

            return toResponse(users);
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="loginParameter"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginParaDto loginParameter)
        {

            var user = await _userService.GetUserAsync(loginParameter.UserName, loginParameter.Password);

            if (user != null)
            {
                var token = AppTokenHelper.Instance.GetToken(user);

                UserLoginOutputDto logininfo = _mapper.Map<UserLoginOutputDto>(user);
                logininfo.Token = token;
                return toResponse(logininfo);
            }
            return toResponse(StatusCodeType.Error, "用户名或密码错误");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [Route(nameof(ModifyPwd))]
        [HttpPost]
        public async Task<IActionResult> ModifyPwd([FromBody] string newpwd)
        {
            if (newpwd.IsNull())
            {
                return toResponse(false);
            }
            var success = await _userService.ModifyPwdAsync(newpwd);

            return toResponse(success);
        }

    }
}
