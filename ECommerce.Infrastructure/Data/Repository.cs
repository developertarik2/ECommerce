using ECommerce.Infrastructure.Data.Interfaces;
using ECommerce.Infrastructure.Specifications;
using ECommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly DbContext Context;
        internal DbSet<T> dbSet;

        public Repository(DbContext context)
        {
            Context = context;
            this.dbSet = context.Set<T>();
        }
        public void Add(T entity)
        {
          //  _context.Set<T>().Add(entity);
            dbSet.Add(entity);
        }
        //public void Update(T entity)
        //{
        //    _context.Set<T>().Attach(entity);
        //    _context.Entry(entity).State = EntityState.Modified;
        //}
        public async Task<T> Get(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAll( Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
                                     string includeProperties = null, bool? isPaging = null, int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            //include properties will be comma seperated
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query= orderBy(query);
            }
          //  int skip = 0;
           // int take = 0;
            if(isPaging==true)
            {
                query= query.Skip(pageSize.GetValueOrDefault()*(pageIndex.GetValueOrDefault()-1)).Take(pageSize.GetValueOrDefault());
            }
           
            return await query.ToListAsync();
        }

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            //include properties will be comma separeted
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public void Remove(int id)
        {
            T entityToRemove = dbSet.Find(id);
            Remove(entityToRemove);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }


        //public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        //{
        //    return await ApplySpecification(spec).FirstOrDefaultAsync();
        //}
        //public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        //{
        //    return await ApplySpecification(spec).ToListAsync();
        //}
        //public async Task<int> CountAsync(ISpecification<T> spec)
        //{
        //    return await ApplySpecification(spec).CountAsync();
        //}
        //private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        //{
        //    return SpecificationEvaluator<T>.GetQuery(Context.Set<T>().AsQueryable(), spec);
        //}
    }
}
