using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nomadas.Network.Core
{
    public interface IRepositoryBase<T>
    {
        Task<List<T>> FindAll();
        Task<T> GetById(long id);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task<T> Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}