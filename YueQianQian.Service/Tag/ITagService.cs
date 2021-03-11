using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Model;

namespace YueQianQian.Service.Tag
{
    public interface ITagService : IBaseService<TagEntity>
    {
        Task<IEnumerable<TagOutDto>> GetTagAsync(int pid);

    }
}
