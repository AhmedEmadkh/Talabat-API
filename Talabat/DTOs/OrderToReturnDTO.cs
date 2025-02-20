﻿using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.APIs.DTOs
{
	public class OrderToReturnDTO
	{
		public string BuyerEmail { get; set; }
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
		public string Status { get; set; }
		public Address ShippingAddress { get; set; }
		public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
		public ICollection<OrderItemDTO> Items { get; set; } = new HashSet<OrderItemDTO>();
		public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public string PaymentIntentId { get; set; }
	}
}
