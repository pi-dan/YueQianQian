using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace YueQianQian.Repository
{
    public abstract class RepositoryBase<TEntity, TKey> : BaseRepository<TEntity, TKey>, IRepositoryBase<TEntity, TKey> where TEntity : class, new()
    {
        protected RepositoryBase(IFreeSql freeSql) : base(freeSql, null, null)
        {
        }

        public virtual Task<TDto> GetAsync<TDto>(TKey id)
        {
            return Select.WhereDynamic(id).ToOneAsync<TDto>();
        }

        public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync();
        }

        public virtual Task<TDto> GetAsync<TDto>(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync<TDto>();
        }

    }

    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, int> where TEntity : class, new()
    {
        //protected RepositoryBase(MyUnitOfWorkManager muowm) : base(muowm.Orm)
        //{
        //    muowm.Binding(this);
        //}
        protected RepositoryBase(IFreeSql fsql, UnitOfWorkManager uowManger) : base(fsql)
        {

            uowManger?.Binding(this);
        }
    }
}
