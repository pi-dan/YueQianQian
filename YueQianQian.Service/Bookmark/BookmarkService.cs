using AutoMapper;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Common.Attributes;
using YueQianQian.Common.Auth;
using YueQianQian.Common.Extensions;
using YueQianQian.Common.Helpers;
using YueQianQian.Model;
using YueQianQian.Repository.Bookmark;
using YueQianQian.Repository.TagBookmark;
using YueQianQian.Repository.User;

namespace YueQianQian.Service.Bookmark
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IMapper _mapper;
        // private readonly IBookmarkRepository _bookmarkRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ITagBookmarkRepository _tagBookmarkRepository;
        private readonly IBookmarkRepository _bookmarkRepository;

        public BookmarkService(IMapper mapper, ICurrentUser currentUser, ITagBookmarkRepository tagBookmarkRepository, IBookmarkRepository bookmarkRepository)
        {
            _mapper = mapper;
            //_bookmarkRepository = bookmarkRepository;
            _currentUser = currentUser;
            _tagBookmarkRepository = tagBookmarkRepository;
            _bookmarkRepository = bookmarkRepository;
        }
        /// <summary>
        /// 更新所有书签的favicon地址
        /// </summary>
        /// <returns></returns>
        public bool UpdateBookmarkFavicon()
        {
            var bookmarks = _bookmarkRepository.Where(it => it.UserId == _currentUser.Id).ToList();

            foreach (var item in bookmarks)
            {
                item.Favicon = FaviconHelper.GetFaviconAddr(item.Url);
                _bookmarkRepository.Update(item);
            }
            return true;
        }

        /// <summary>
        /// 查询书签
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<PagedInfo<BookmarkEntity>> GetBookmarkAsync(BmInputPageDto parm)
        {
            PagedInfo<BookmarkEntity> pagedInfo = new PagedInfo<BookmarkEntity>();
            pagedInfo.PageIndex = parm.PageIndex;
            pagedInfo.PageSize = parm.PageSize;

            if (parm.TagId != -1)
            {
                var bookmarks = await _tagBookmarkRepository.WhereIf(parm.TagId != -1, it => it.TagId == parm.TagId && it.Bookmark.UserId == _currentUser.Id)
                    .Count(out long total).Page(parm.PageIndex, parm.PageSize)
                    .OrderByPropertyNameIf(EntityHelper.ContainProperty(new BookmarkEntity(), parm.OrderBy), parm.OrderBy, parm.Ascending)
                    .ToListAsync<BookmarkEntity>();

                pagedInfo.TotalCount = (int)total;
                pagedInfo.DataSource = bookmarks;
            }
            else
            {
                var bookmarks = await _bookmarkRepository.Select
                     .Count(out long total).Page(parm.PageIndex, parm.PageSize)
                    .OrderByPropertyNameIf(EntityHelper.ContainProperty(new BookmarkEntity(), parm.OrderBy), parm.OrderBy, parm.Ascending)
                    .ToListAsync<BookmarkEntity>();
                pagedInfo.TotalCount = (int)total;
                pagedInfo.DataSource = bookmarks;
            }
            return pagedInfo;
        }
        /// <summary>
        /// 点击书签， 点击次数加一，并修改最后点击事件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> BookmarkClick(int id)
        {
            var mark = await _bookmarkRepository.Where(it => it.Id == id).FirstAsync();
            if (mark == null)
            {
                throw new Exception("id错误，未找到书签");
            }
            mark.ClickCount++;
            mark.LastClick = DateTime.Now;
            return await _bookmarkRepository.UpdateAsync(mark);
        }
        /// <summary>
        /// 获取网页标题
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GetWebTitle(string url)
        {
            return await Task.Run(() => UrlHelper.GetWebTitle(url));
        }

        /// <summary>
        /// 添加书签
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction(Propagation = Propagation.Nested)]
        public async Task<bool> Add(BmInsertOrUpdateDto entity)
        {
            var book = _mapper.Map<BookmarkEntity>(entity);
            book.ClickCount = 1;
            book.CreatedAt = book.LastClick = DateTime.Now;
            book.Favicon = FaviconHelper.GetFaviconAddr(book.Url);
            var tagboook = _mapper.Map<TagBookmarkEntity>(entity);
            book = await _bookmarkRepository.InsertAsync(book);
            tagboook.BookmarkId = book.Id;
            tagboook = await _tagBookmarkRepository.InsertAsync(tagboook);
            return true;
        }
        /// <summary>
        /// 根据书签id查询书签所在的目录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TagEntity> GetTagFromBookmarkid(int id)
        {
            return await _tagBookmarkRepository.Where(it => it.BookmarkId == id).Where(it => it.TagEntity != null).ToOneAsync<TagEntity>();
        }
        /// <summary>
        /// 修改书签
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction(Propagation = Propagation.Nested)]
        public async Task<bool> Update(BmInsertOrUpdateDto entity)
        {
            var bookmark = await _bookmarkRepository.Where(it => it.Id == entity.id).ToOneAsync();
            bookmark.Url = entity.Url;
            bookmark.Title = entity.Title;
            bookmark.Description = entity.Description;
            var bc = await _bookmarkRepository.UpdateAsync(bookmark);

            var tag = await _tagBookmarkRepository.Where(it => it.BookmarkId == bookmark.Id).ToOneAsync();

            TagBookmarkEntity tagBookmark = new TagBookmarkEntity();
            tagBookmark.BookmarkId = bookmark.Id;
            tagBookmark.TagId = entity.TagId;

            var dc = await _tagBookmarkRepository.DeleteAsync(tag);
            var ic = await _tagBookmarkRepository.InsertAsync(tagBookmark);
            return true;
        }
        /// <summary>
        /// 删除书签
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Transaction(Propagation = Propagation.Nested)]
        public async Task<bool> Delete(int id)
        {
            var tagbookmark = await _tagBookmarkRepository.Where(it => it.BookmarkId == id).ToOneAsync();
            await _tagBookmarkRepository.DeleteAsync(tagbookmark);

            var bookmark = _bookmarkRepository.DeleteAsync(id);

            return true;
        }
        /// <summary>
        /// 根据句关键字搜索书签
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public async Task<List<BmSearchDto>> SearchBookmark(string searchText)
        {
            if (searchText.IsNull())
            {
                return new List<BmSearchDto>();
            }
            var book = await _bookmarkRepository.Where(it => it.UserId == _currentUser.Id).Where(it => it.Title.Contains(searchText))
            .From<TagBookmarkEntity, TagEntity, TagEntity>((s, b, c, d) =>
            s.LeftJoin(a => a.Id == b.BookmarkId)
            .LeftJoin(a => b.TagId == c.Id)
            .LeftJoin(a => c.Pid == d.Id))
            .ToListAsync((a, b, c, d) => new BmSearchDto()
            {
                Id = a.Id,
                ClickCount = a.ClickCount,
                CreatedAt = a.CreatedAt,
                Description = a.Description,
                LastClick = a.LastClick,
                Title = a.Title,
                Url = a.Url,
                Favicon = a.Favicon,
                Path = "/" + (d == null ? "" : d.Name + "/") + c.Name
            });

            return book;
        }
    }
}
