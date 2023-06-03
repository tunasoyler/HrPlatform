using Bilgeadam.HrPlatform.DAL.Abstract;
using Bilgeadam.HrPlatform.DAL.Context;
using Bilgeadam.HrPlatform.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bilgeadam.HrPlatform.DAL.Concrete
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        protected readonly DbContext context;

        public Repository(DbContext _context)
        {
            context = _context;
        }

        public async Task AddAsync(T Entity)
        {
            await context.Set<T>().AddAsync(Entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> Entity)
        {
            await context.Set<T>().AddRangeAsync(Entity);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Where(predicate).ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public void Remove(T Entity)
        {
            context.Set<T>().Remove(Entity);
        }

        public void RemoveRange(T Entity)
        {
            context.Set<T>().RemoveRange(Entity);
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public async Task Update(T Entity, int id)
        {
            var updateEntity = await context.Set<T>().FindAsync(id);
            context.Entry<T>(updateEntity).CurrentValues.SetValues(Entity);
        }
    }
}
