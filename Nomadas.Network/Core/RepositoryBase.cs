using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Nomadas.Network.Core
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected DBContext DBContext { get; set; }

        public RepositoryBase(DBContext DBContext)
        {
            this.DBContext = DBContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.DBContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.DBContext.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task Create(T entity)
        {
            await this.DBContext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            this.DBContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.DBContext.Set<T>().Remove(entity);
        }

        public async Task Save()
        {
            await this.DBContext.SaveChangesAsync();
        }
    }
}