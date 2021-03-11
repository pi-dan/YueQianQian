using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YueQianQian.Service.User
{
    public class UserLoginOutputDto
    {


        /// <summary>
        /// 主键
        /// </summary>

        public int Uuid { get; set; }


        public DateTime? CreatedAt { get; set; }


        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 是否管理员
        /// </summary>

        public string IsAdmin { get; set; } = "N";


        public DateTime? LastLogin { get; set; }



        public string Name { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
