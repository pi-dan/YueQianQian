using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Common.Extensions;

namespace YueQianQian.Common.Auth
{
    /// <summary>
    /// 当前登录用户
    /// </summary>
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual int Id
        {
            get
            {
                var claimValue = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.UserId);
                if (claimValue != null && claimValue.Value.NotNull())
                {
                    return claimValue.Value.ToInt();
                }
                return 0;
            }
        }

        public string UserName
        {
            get
            {
                var claimValue = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.UserName);
                if (claimValue != null && claimValue.Value.NotNull())
                {
                    return claimValue.Value;
                }
                return "";
            }
        }

        public string Email
        {
            get
            {
                var claimValue = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.Email);
                if (claimValue != null && claimValue.Value.NotNull())
                {
                    return claimValue.Value;
                }
                return "";
            }
        }

        public string IsAdmin
        {
            get
            {
                var claimValue = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.IsAdmin);
                if (claimValue != null && claimValue.Value.NotNull())
                {
                    return claimValue.Value;
                }
                return "";
            }
        }


    }
}
