using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Model;

namespace YueQianQian.Repository.Tag
{
    public class TagRepository : RepositoryBase<TagEntity>, ITagRepository
    {
        public TagRepository(IFreeSql fsql, UnitOfWorkManager uowm) : base(fsql, uowm)
        {
        }
    }
}
