using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YueQianQian.Common.Auth;
using YueQianQian.Common.Helpers;
using YueQianQian.Model;
using YueQianQian.Service.Bookmark;
using YueQianQian.Service.Tag;
using YueQianQian.Service.User;

namespace YueQianQian.WebApi.Controllers
{
    /// <summary>
    /// 书签
    /// </summary>
    [Authorize]
    [Route("Api/[controller]")]
    [ApiController]
    public class BookmarkController : BaseController
    {
        private readonly IBookmarkService _bookmarkService;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        public BookmarkController(IMapper mapper, ICurrentUser currentUser, IBookmarkService bookmarkService)
        {
            this._bookmarkService = bookmarkService;
            this._mapper = mapper;
            this._currentUser = currentUser;
        }

        /// <summary>
        /// 分页查询书签
        /// </summary>
        /// <returns></returns>
        [Route(nameof(GetBookmarks))]
        [HttpPost]
        public async Task<IActionResult> GetBookmarks([FromBody] BmInputPageDto para)
        {

            var books = await _bookmarkService.GetBookmarkAsync(para);


            return toResponse(books);
        }

        /// <summary>
        /// 更新所有书签favicon
        /// </summary>
        /// <returns></returns>
        [Route(nameof(UpdateBookmarksFavicon))]
        [HttpGet]
        public IActionResult UpdateBookmarksFavicon()
        {
            var re = _bookmarkService.UpdateBookmarkFavicon();
            if (re)
            {
                return toResponse(StatusCodeType.Success);
            }
            return toResponse(StatusCodeType.Error);
        }
        /// <summary>
        /// 书签点击
        /// </summary>
        /// <returns></returns>
        [Route(nameof(BookmarkClick))]
        [HttpPost]
        public async Task<IActionResult> BookmarkClick([FromQuery] int id)
        {
            var re = await _bookmarkService.BookmarkClick(id);
            return toResponse(re);
        }
        /// <summary>
        /// 获取网页标题
        /// </summary>
        /// <returns></returns>
        [Route(nameof(GetWebTitle))]
        [HttpPost]
        public async Task<IActionResult> GetWebTitle([FromBody] string url)
        {
            var re = await _bookmarkService.GetWebTitle(url);
            return toResponse(re);
        }
        /// <summary>
        /// 新增书签
        /// </summary>
        /// <returns></returns>
        [Route(nameof(InsertBookmark))]
        [HttpPost]
        public async Task<IActionResult> InsertBookmark([FromBody] BmInsertOrUpdateDto bookmark)
        {
            bookmark.UserId = _currentUser.Id;
            var re = await _bookmarkService.Add(bookmark);
            return toResponse(re);
        }
        /// <summary>
        /// 修改书签
        /// </summary>
        /// <returns></returns>
        [Route(nameof(UpdateBookmark))]
        [HttpPost]
        public async Task<IActionResult> UpdateBookmark([FromBody] BmInsertOrUpdateDto bookmark)
        {
            var re = await _bookmarkService.Update(bookmark);
            return toResponse(re);
        }
        /// <summary>
        /// 删除书签
        /// </summary>
        /// <returns></returns>
        [Route(nameof(DeleteBookmark))]
        [HttpPost]
        public async Task<IActionResult> DeleteBookmark([FromBody] int id)
        {
            var re = await _bookmarkService.Delete(id);
            return toResponse(re);
        }
        /// <summary>
        /// 根据书签id查询书签所在的目录
        /// </summary>
        /// <returns></returns>
        [Route(nameof(GetTagFromBookmarkid))]
        [HttpGet]
        public async Task<IActionResult> GetTagFromBookmarkid(int id)
        {
            var tag = await _bookmarkService.GetTagFromBookmarkid(id);
            return toResponse(tag);
        }
        /// <summary>
        /// 搜索书签
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        [Route(nameof(SearchBookmark))]
        [HttpPost]
        public async Task<IActionResult> SearchBookmark([FromBody] string searchText)
        {
            var books = await _bookmarkService.SearchBookmark(searchText);
            return toResponse(books);
        }
    }
}

