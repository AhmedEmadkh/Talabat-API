using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications
{
    public interface ISepecifications<T> where T : BaseEntity
    {
        // Signature for property for where condition [Where(P => P.id == id)]
        public Expression<Func<T,bool>> Criteria { get; set; }
        //Signature for the list of Includes [Include(P => P.ProductBrand).Include(P => P.ProductType)]
        public List<Expression<Func<T,object>>> Includes { get; set; }

        // Signature for OrderBy
        public Expression<Func<T,object>> OrderBy { get; set; }

        // Signature For OrderByDesc
        public Expression<Func<T,object>> OrderByDescending { get; set; }


        // Skip()

        public int Skip { get; set; }

        // Take()
        public int Take { get; set; }

        public bool IsPaginationEnabled { get; set; }

    }
}
