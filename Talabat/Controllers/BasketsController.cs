using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
	public class BasketsController : APIBaseController
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;

		public BasketsController(IBasketRepository basketRepository, IMapper mapper)
        {
			_basketRepository = basketRepository;
			_mapper = mapper;
		}
        // Get Or ReCreate Basket

        [HttpGet]
		public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string BasketId)
		{
			var Basket = await _basketRepository.GetBasketAsync(BasketId);
			return Basket is null ? new CustomerBasket(BasketId) : Basket;
		}


		// Update Or Craete New Basket
		[HttpPost]
		public async Task<ActionResult<CustomerBasketDTO>> UpdateCustomerBasket(CustomerBasketDTO Basket)
		{
			var MappedBasket = _mapper.Map<CustomerBasket>(Basket);
			var CreatedORUpdated = await _basketRepository.UpdataBasketAsync(MappedBasket);
			if (CreatedORUpdated is null)
				return BadRequest(new APIResponse(400));
			return Ok(CreatedORUpdated);
		}

		// Delete Basket
		[HttpDelete]
		public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
		{
			return await _basketRepository.DeleteBasketAsync(BasketId);
		}
	}
}
