using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            // Seeding Brands
            if (!dbContext.ProductBrands.Any())
            {
                var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json"); // Serialize
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData); // Deserialize


                if (Brands?.Count > 0)
                {
                    foreach (var Brand in Brands)
                    {
                        await dbContext.Set<ProductBrand>().AddAsync(Brand);
                    }
                }
            }

            // Seeding Types
            if (!dbContext.ProductTypes.Any())
            {

                var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/types.json"); // Serialize
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData); // Deserialize

                if (Types?.Count > 0)
                {
                    foreach (var Type in Types)
                    {
                        await dbContext.Set<ProductType>().AddAsync(Type);
                    }
                }
            }


            // Seeding Products
            if (!dbContext.Products.Any())
            {

                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json"); // Serialize
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData); // Deserialize

                if (Products?.Count > 0)
                {
                    foreach (var Product in Products)
                    {
                        await dbContext.Set<Product>().AddAsync(Product);
                    }
                }
            }

            // Seeding Delivery
            if (!dbContext.DeliveryMethods.Any())
            {
                var DeliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json"); // Serialize
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if(DeliveryMethods?.Count > 0)
                {
                    foreach(var DeliveryMethod in DeliveryMethods)
                    {
                        await dbContext.Set<DeliveryMethod>().AddAsync(DeliveryMethod);
                    }
                }
			} 


			await dbContext.SaveChangesAsync();
		}
    }
}
