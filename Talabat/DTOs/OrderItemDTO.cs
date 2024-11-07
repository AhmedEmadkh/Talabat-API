using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.APIs.DTOs
{
	public class OrderItemDTO
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public string ProductUrl { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}
}
