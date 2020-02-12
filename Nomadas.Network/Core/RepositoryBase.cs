using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Nomadas.Network.Core
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext _context { get; set; }

        public RepositoryBase(ApplicationDbContext _context)
        {
            this._context = _context ?? throw new ArgumentNullException(nameof(_context));
        }

        public async Task<List<T>> FindAll()
        {
            return await this._context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetById(long id)
        {
            return await this._context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this._context.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task<T> Create(T entity)
        {
            await this._context.Set<T>().AddAsync(entity);
            await this._context.SaveChangesAsync();
            return entity;
        }

        public async Task Update(T entity)
        {
            this._context.Set<T>().Update(entity);
            await this._context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            this._context.Set<T>().Remove(entity);
            await this._context.SaveChangesAsync();
        }
    }
}