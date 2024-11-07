using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
	public class OrdersController : APIBaseController
	{
		#region Services
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public OrdersController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_orderService = orderService;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		} 
		#endregion
		#region CreateOrder
		[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Order>> CreateOrder(OrderDTO orderDTO)
		{
			var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
			var MappedAddress = _mapper.Map<AddressDTO, Address>(orderDTO.ShippingAddress);
			var Order = await _orderService.CreateOrderAsync(BuyerEmail, orderDTO.BasketId, orderDTO.DeliveryMethodId, MappedAddress);

			if (Order is null)
				return BadRequest(new APIResponse(400, "There Is A Problem With Your Order"));
			return Ok(Order);
		}
		#endregion

		#region GetOrderForUser

		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDTO>), StatusCodes.Status200OK)]
		[Authorize]
		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrderForUser()
		{
			var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
			var Orders = await _orderService.GetOrdersForSpecificUserAsync(BuyerEmail);
			if (Orders is null)
				return NotFound(new APIResponse(404, "No Orders Found"));
			var MappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>(Orders);
			return Ok(MappedOrders);
		}
		#endregion


		#region GetOrderByIdForUser
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
		[Authorize]
		[HttpGet("{id}")]
		public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdForUser(int id)
		{
			var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
			var Order = await _orderService.GetOrdersByIdForSpecificUserAsync(BuyerEmail, id);
			if (Order is null)
				return NotFound(new APIResponse(404, $"There Is No Order With Id {id} For This User"));
			var MappedOrder = _mapper.Map<Order, OrderToReturnDTO>(Order);
			return Ok(MappedOrder);
		}
		#endregion

		#region GetDeliveryMethods

		[HttpGet("DeliveryMethods")] // BaseUrl/Api/Orders/DeliveryMethods
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
		{
			var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
			return Ok(DeliveryMethods);
		} 
		#endregion
	}
}
