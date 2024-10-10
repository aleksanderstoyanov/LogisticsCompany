using LogisticsCompany.Data.Entity;
using LogisticsCompany.Entity;
using LogisticsCompany.Services.Dto;
using LogisticsCompany.Services.Offices.Dto;

namespace LogisticsCompany.Tests.Common
{
    public static class MockDataRepository
    {
        public static IEnumerable<DeliveryDto> GetAllDeliveries()
        {
            return Enumerable.Range(1, 10)
            .Select(i => new DeliveryDto
            {
                Id = i,
                StartDate = DateTime.Now.AddDays(i),
                EndDate = DateTime.Now.AddDays(i),
                SelectedIds = new int[] { i }
            });
        }

        public static IEnumerable<OfficeDto> GetOffices()
        {
            return new List<OfficeDto>()
            {
                new OfficeDto
                {
                    Id = 1,
                    Address = "ul.Geo Milev"
                },
                new OfficeDto
                {
                    Id = 2,
                    Address = "ul.Shipchenski Prohod"
                }
            };
        }

        public static IEnumerable<Role> GetRoles()
        {
            return new List<Role>()
            {
                new Role
                {
                    Id = 1,
                    Name = "None"
                },
                new Role
                {
                    Id = 2,
                    Name = "OfficeEmployee"
                },
                new Role
                {
                    Id = 3,
                    Name = "Courier"
                },
                new Role
                {
                    Id = 4,
                    Name = "Client"
                },
                new Role
                {
                    Id = 5,
                    Name = "Admin"
                }
            };
        }

        public static IEnumerable<PackageStatus> GetPackageStatuses()
        {
            return new List<PackageStatus>()
            {
                new PackageStatus
                {
                    Id = 1,
                    Name = "NonRegistered"
                },
                new PackageStatus
                {
                    Id = 2,
                    Name = "Registered"
                },
                new PackageStatus
                {
                    Id = 3,
                    Name = "InDelivery"
                },
                new PackageStatus
                {
                    Id = 4,
                    Name = "Delivered"
                }

            };
        }
    }
}
