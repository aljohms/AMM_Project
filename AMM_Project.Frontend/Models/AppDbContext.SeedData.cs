using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMM_Project.Frontend.Models
{
    public static class AppDbContextSeedData
    {
       
            static object synchlock = new object();
            static volatile bool seeded = false;

            public static void EnsureSeedData(this AppDbContext context)
            {
                if (!seeded && context.Business.Count() == 0)
                {
                    lock (synchlock)
                    {
                        if (!seeded)
                        {
                            var businesses = GenerateBusinesses();
                            context.Business.AddRange(businesses);
                        var branches = GenerateBranches();
                        context.Branch.AddRange(branches);
                        context.SaveChanges();
                        seeded = true;
                        }
                    }
                }
        }
        public static Business[] GenerateBusinesses()
        {
            return new Business[] {
                new Business {
                    Id = 1,
                    Name = "Jutsu",
                    ActivityField ="Restaurants",
                },
                 new Business {
                    Id = 2,
                    Name = "Rose Ribbon",
                    ActivityField ="Sweets",
                },
            };
        }
        public static Branch[] GenerateBranches()
        {
            return new Branch[] {
                new Branch {
                    Id = 1,
                    BusinessId=1,
                    Name = "جتسو الفاخرية",
                    Location ="عنيزة",
                },
                 new Branch {
                    Id = 2,
                    BusinessId=1,
                    Name = "جتسو الفهد",
                    Location ="عنيزة",
                },
                 new Branch {
                    Id = 3,
                    BusinessId=1,
                    Name = "جتسو البخاري",
                    Location ="بريدة",
                },
                 new Branch {
                    Id = 4,
                    BusinessId=2,
                    Name = "روز ربن الفاخرية",
                    Location ="عنيزة",
                },
                  new Branch {
                    Id = 5,
                    BusinessId=2,
                    Name = "روز ربن البخاري",
                    Location ="بريدة",
                },
            };
        }

    }
    }
