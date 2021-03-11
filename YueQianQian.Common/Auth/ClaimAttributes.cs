using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YueQianQian.Common.Auth
{
    /// <summary>
    /// Claim属性
    /// </summary>
    public static class ClaimAttributes
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public const string UserId = "UserId";

        /// <summary>
        /// 用户名
        /// </summary>
        public const string UserName = "UserName";

        /// <summary>
        /// 刷新有效期
        /// </summary>
        public const string Email = "Email";

        /// <summary>
        /// 租户Id
        /// </summary>
        public const string IsAdmin = "IsAdmin";
    }
}
