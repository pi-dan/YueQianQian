using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Model;
using YueQianQian.Repository.Bookmark;

namespace YueQianQian.Repository.TagBookmark
{
    public class BookmarkRepository : RepositoryBase<BookmarkEntity>, IBookmarkRepository
    {
        public BookmarkRepository(IFreeSql fsql, UnitOfWorkManager uowm) : base(fsql, uowm)
        {
        }
    }
}
