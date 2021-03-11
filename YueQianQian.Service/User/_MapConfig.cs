
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Model;

namespace YueQianQian.Service.User
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<UserEntity, UserListOutputDto>();
            CreateMap<UserEntity, UserLoginOutputDto>().ForMember(d => d.Uuid, opt => opt.MapFrom(s => s.Id)).ForMember(d => d.Name, opt => opt.MapFrom(s => s.UserName));
        }

    }
}
