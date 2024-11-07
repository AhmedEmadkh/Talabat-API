using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandTypeSpecifications : BaseSpecifications<Product>
    {
        // CTOR For Get All Products
        public ProductWithBrandTypeSpecifications(ProductSpecParams Params) 
            :base(P =>
                (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search))
                &&
                (!Params.BrandId.HasValue || P.ProductBrand.Id == Params.BrandId)
                &&
                (!Params.TypeId.HasValue || P.ProductType.Id == Params.TypeId)
            )
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);

            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        SetOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        SetOrderByDescending(P => P.Price);
                        break;
                    default:
                        SetOrderBy(P => P.Name);
                        break;
                }
            }

            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);


        }
        // CTOR For Get Products By Id
        public ProductWithBrandTypeSpecifications(int id):base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
        }
    }
}
