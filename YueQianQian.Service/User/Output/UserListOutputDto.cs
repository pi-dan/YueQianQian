using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YueQianQian.Service.User
{
    public class UserListOutputDto
    {


        /// <summary>
        /// 主键
        /// </summary>

        public int Id { get; set; }


        public DateTime? CreatedAt { get; set; }


        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 是否管理员
        /// </summary>

        public string IsAdmin { get; set; } = "N";


        public DateTime? LastLogin { get; set; }



        public string UserName { get; set; } = string.Empty;

    }
}
