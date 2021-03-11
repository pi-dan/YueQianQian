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
using YueQianQian.Service.Tag;
using YueQianQian.Service.User;

namespace YueQianQian.WebApi.Controllers
{
    /// <summary>
    /// 目录
    /// </summary>
    [Authorize]
    [Route("Api/[controller]")]
    [ApiController]
    public class TagController : BaseController
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        public TagController(IMapper mapper, ICurrentUser currentUser, ITagService tagService)
        {
            this._tagService = tagService;
            this._mapper = mapper;
            this._currentUser = currentUser;
        }

        /// <summary>
        /// 获取目录
        /// </summary>
        /// <returns></returns>
        [Route(nameof(Tags))]
        [HttpGet]
        public async Task<IActionResult> Tags([FromQuery] int pid)
        {

            var tags = await _tagService.GetTagAsync(pid);

            return toResponse(tags);
        }

        /// <summary>
        /// 新增书签目录
        /// </summary>
        /// <returns></returns>
        [Route(nameof(InsertBookmarkTag))]
        [HttpPost]
        public async Task<IActionResult> InsertBookmarkTag([FromBody] TagInsertOrUpdateDto entity)
        {
            var tagentity = _mapper.Map<TagEntity>(entity);
            tagentity.Type = "M";
            var tags = await _tagService.Add(tagentity);

            return toResponse(tags);
        }
        /// <summary>
        /// 新增笔记目录
        /// </summary>
        /// <returns></returns>
        [Route(nameof(InsertNoteTag))]
        [HttpPost]
        public async Task<IActionResult> InsertNoteTag([FromBody] TagInsertOrUpdateDto entity)
        {

            var tagentity = _mapper.Map<TagEntity>(entity);
            tagentity.Type = "N";
            var tags = await _tagService.Add(tagentity);

            return toResponse(tags);
        }
        /// <summary>
        /// 修改目录
        /// </summary>
        /// <returns></returns>
        [Route(nameof(UpdateTag))]
        [HttpPost]
        public async Task<IActionResult> UpdateTag([FromBody] TagInsertOrUpdateDto entity)
        {
            var tagentity = _mapper.Map<TagEntity>(entity);
            var tags = await _tagService.Update(tagentity);

            return toResponse(tags);
        }
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <returns></returns>
        [Route(nameof(DeleteTag))]
        [HttpPost]
        public async Task<IActionResult> DeleteTag([FromBody] int id)
        {

            var tags = await _tagService.Delete(id);

            return toResponse(tags);
        }
    }
}
