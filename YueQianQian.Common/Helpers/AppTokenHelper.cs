using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Common.Auth;
using YueQianQian.Model;

namespace YueQianQian.Common.Helpers
{
    public class AppTokenHelper
    {
        public readonly static AppTokenHelper Instance = new AppTokenHelper();

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetToken(UserEntity user)
        {
            //创建用户身份标识，可按需要添加更多信息
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimAttributes.UserId, user.Id.ToString(), ClaimValueTypes.Integer32), // 用户id
                new Claim(ClaimAttributes.UserName, user.UserName), // 用户名
                new Claim(ClaimAttributes.Email, user.Email), // 邮箱
                new Claim(ClaimAttributes.IsAdmin, user.IsAdmin), // 是否管理员
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettingsHelper.Configuration["JwtSetting:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //创建令牌
            var token = new JwtSecurityToken(
              issuer: AppSettingsHelper.Configuration["JwtSetting:Issuer"],
              audience: AppSettingsHelper.Configuration["JwtSetting:Audience"],
              signingCredentials: creds,
              claims: claims,
              notBefore: DateTime.Now
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
