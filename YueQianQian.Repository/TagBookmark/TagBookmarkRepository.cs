using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Model;
using YueQianQian.Repository.TagBookmark;

namespace YueQianQian.Repository.Bookmark
{
    public class TagBookmarkRepository : RepositoryBase<TagBookmarkEntity>, ITagBookmarkRepository
    {
        public TagBookmarkRepository(IFreeSql fsql, UnitOfWorkManager uowm) : base(fsql, uowm)
        {
        }
    }
}
