using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ChineseSaleApi.Models;

namespace ChineseSaleApi.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ChineseSaleContextDb>();

            // Only seed when both Categories and Gifts are empty
            if (!context.Categories.Any() && !context.Gifts.Any())
            {
                var categories = new[]
                {
                    new Category { Name = "Art" },
                    new Category { Name = "Health" },
                    new Category { Name = "Home" }
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();

                // Create sample donors so Gift foreign keys are satisfied
                var donors = new[]
                {
                    new Donor { Name = "Donor A", Email = "donorA@example.com" },
                    new Donor { Name = "Donor B", Email = "donorB@example.com" },
                    new Donor { Name = "Donor C", Email = "donorC@example.com" }
                };

                context.Donors.AddRange(donors);
                context.SaveChanges();

                var gifts = new[]
                {
                    new Gift
                    {
                        Name = "Painting",
                        TicketCost = 10,
                        Description = "Sample painting",
                        PictureUrl = "10.jpg",
                        CategoryId = categories[0].Id,
                        DonorId = donors[0].Id
                    },
                    new Gift
                    {
                        Name = "Health Kit",
                        TicketCost = 15,
                        Description = "Sample health kit",
                        PictureUrl = "12.jpg",
                        CategoryId = categories[1].Id,
                        DonorId = donors[1].Id
                    },
                    new Gift
                    {
                        Name = "Coffee Maker",
                        TicketCost = 20,
                        Description = "Sample coffee maker",
                        PictureUrl = "13.jpg",
                        CategoryId = categories[2].Id,
                        DonorId = donors[2].Id
                    }
                };

                context.Gifts.AddRange(gifts);
                context.SaveChanges();
            }
        }
    }
}
