using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Repositories
{
	public interface IBasketRepository
	{
		public Task<CustomerBasket?> GetBasketAsync(string BasketId);
		public Task<CustomerBasket?> UpdataBasketAsync(CustomerBasket Basket);
		public Task<bool> DeleteBasketAsync(string BasketId);
	}
}
