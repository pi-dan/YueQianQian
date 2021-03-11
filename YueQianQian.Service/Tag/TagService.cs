using AutoMapper;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Common.Attributes;
using YueQianQian.Common.Auth;
using YueQianQian.Common.Helpers;
using YueQianQian.Model;
using YueQianQian.Repository.Tag;
using YueQianQian.Repository.TagBookmark;
using YueQianQian.Repository.User;

namespace YueQianQian.Service.Tag
{
    public class TagService : ITagService
    {
        private readonly IMapper _mapper;
        private readonly ITagRepository _tagRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ITagBookmarkRepository _tagBookmarkRepository;

        public TagService(IMapper mapper, ITagRepository tagRepository, ICurrentUser currentUser, ITagBookmarkRepository tagBookmarkRepository)
        {
            _mapper = mapper;
            _tagRepository = tagRepository;
            _currentUser = currentUser;
            _tagBookmarkRepository = tagBookmarkRepository;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> Add(TagEntity entity)
        {
            entity.UserId = _currentUser.Id;
            entity.LastUse = DateTime.Now;
            entity.Sort = 100000;
            entity.Id = 0;
            await _tagRepository.InsertAsync(entity);
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var books = await _tagBookmarkRepository.Where(it => it.TagId == id).FirstAsync();
            if (books != null)
            {
                throw new Exception("目录非空，删除失败");
            }
            await _tagRepository.DeleteAsync(id);
            return true;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> Update(TagEntity entity)
        {
            var tagentity = await _tagRepository.GetAsync(entity.Id);
            tagentity.Name = entity.Name;
            tagentity.Pid = entity.Pid;
            await _tagRepository.UpdateAsync(tagentity);
            return true;
        }
        /// <summary>
        /// 获取书签目录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        async public Task<IEnumerable<TagOutDto>> GetTagAsync(int pid)
        {
            var tags = await _tagRepository.Where(it => it.UserId == _currentUser.Id && it.Type == "M" && it.Pid == pid).OrderBy(it => it.Sort).ToListAsync();
            return _mapper.Map<IEnumerable<TagOutDto>>(tags);
        }


    }
}
