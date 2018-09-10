using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.stading.Models
{
    public class SamplyData
    {
        public static void Initialize(MobileContext context)
        {
            if (!context.Phones.Any())
            {
                context.Phones.AddRange(
                    new Phone
                    {
                        Name = "iPhone 7s",
                        Company = "Apple",
                        Price = 600
                    },
                    new Phone
                    {
                        Name = "Samsung Galaxy Edge",
                        Company = "Sumsung",
                        Price = 550,
                    },
                    new Phone
                    {
                        Name = "Nokia Lumia 950",
                        Company = "Nokia",
                        Price = 450
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
