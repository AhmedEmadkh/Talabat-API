using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;

namespace Talabat.Repository
{
	public class BasketRepository : IBasketRepository
	{
		private IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis) // Ask CLR for object from class Implements Interface IConnectionMultiplexer
		{
            _database = redis.GetDatabase();
        }
        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
		{
			var Basket = await _database.StringGetAsync(BasketId);
			return Basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(Basket);
		}

		public async Task<CustomerBasket?> UpdataBasketAsync(CustomerBasket Basket)
		{
			var JsonBasket = JsonSerializer.Serialize(Basket);
			var CreatedOrUpdated = await _database.StringSetAsync(Basket.Id, JsonBasket,TimeSpan.FromDays(1));
			return !CreatedOrUpdated ? null : await GetBasketAsync(Basket.Id);
		}
		public async Task<bool> DeleteBasketAsync(string BasketId)
		{
			return await _database.KeyDeleteAsync(BasketId);
		}

	}
}
