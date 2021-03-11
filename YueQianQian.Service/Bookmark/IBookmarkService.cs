using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Model;

namespace YueQianQian.Service.Bookmark
{
    public interface IBookmarkService : IBaseService<BmInsertOrUpdateDto>
    {
        Task<PagedInfo<BookmarkEntity>> GetBookmarkAsync(BmInputPageDto parm);

        bool UpdateBookmarkFavicon();
        Task<int> BookmarkClick(int id);
        Task<String> GetWebTitle(String url);
        Task<TagEntity> GetTagFromBookmarkid(int id);
        Task<List<BmSearchDto>> SearchBookmark(string searchText);
    }
}
