using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery, ISepecifications<T> Spec)
        {
            var Query = InputQuery; // _dbContext.Set<T>()
            if(Spec.Criteria is not null)
            {
                Query = Query.Where(Spec.Criteria); // _dbContext.Set<T>().Where(P => P.Id == Id)
            }

            if(Spec.OrderBy is not null)
            {
                Query = Query.OrderBy(Spec.OrderBy);
            }
            if(Spec.OrderByDescending is not null)
            {
                Query = Query.OrderByDescending(Spec.OrderByDescending);
            }

            if (Spec.IsPaginationEnabled)
            {
                Query = Query.Skip(Spec.Skip).Take(Spec.Take);
            }

            Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression)); // _dbContext.Set<T>().Where(P => P.Id == Id).Include(P => P.ProductType)

            return Query;
        }
    }
}
