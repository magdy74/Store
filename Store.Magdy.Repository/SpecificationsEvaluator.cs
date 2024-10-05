using Microsoft.EntityFrameworkCore;
using Store.Magdy.Core.Entities;
using Store.Magdy.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Repository
{
    public class SpecificationsEvaluator<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {

        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> InputQuery, ISpecifications<TEntity, TKey> spec)
        {
            var query = InputQuery;

            if(spec.Criteria is not null)
            {
                query = InputQuery.Where(spec.Criteria);
            }

            if(spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            

            return query;
        }

    }
}
