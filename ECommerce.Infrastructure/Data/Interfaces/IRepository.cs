using ECommerce.Infrastructure.Specifications;
using ECommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Interfaces
{
   public interface IRepository<T> where T : class
    {
       Task<T> Get(int id);

       Task <IReadOnlyList<T>> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null, bool? isPaging = null, int? pageIndex = null, int? pageSize = null
                );
       Task< T> GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,

              string includeProperties = null
            );

        void Add(T entity);
        //void Update(T entity);
        void Remove(int id);
        void Remove(T entity);

        //Task<T> GetEntityWithSpec(ISpecification<T> spec);
        //Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        //Task<int> CountAsync(ISpecification<T> spec);
    }
}
