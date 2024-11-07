﻿using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
	public class OrderItemPictureURLResolver : IValueResolver<OrderItem, OrderItemDTO, string>
	{
		private readonly IConfiguration _configuration;

		public OrderItemPictureURLResolver(IConfiguration configuration)
        {
			_configuration = configuration;
		}
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
		{
			if(string.IsNullOrEmpty(source.Product.ProductUrl))
				return $"{_configuration["APIBaseUrl"]}{source.Product.ProductUrl}";
			return string.Empty;
		}
	}
}
