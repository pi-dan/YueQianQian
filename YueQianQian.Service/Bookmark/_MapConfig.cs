
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Model;
using YueQianQian.Service.Bookmark;

namespace YueQianQian.Service.Bookmark
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<BmInsertOrUpdateDto, BookmarkEntity>();
            CreateMap<BmInsertOrUpdateDto, TagBookmarkEntity>();
        }

    }
}
