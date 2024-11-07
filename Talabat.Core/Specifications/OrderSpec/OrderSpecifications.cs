using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Specifications.OrderSpec
{
	public class OrderSpecifications : BaseSpecifications<Order>
	{
        public OrderSpecifications(string email):base(O => O.BuyerEmail == email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

            SetOrderByDescending(O => O.OrderDate);
        }
        public OrderSpecifications(string email,int Id):base(O => O.BuyerEmail == email && O.Id == Id)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
