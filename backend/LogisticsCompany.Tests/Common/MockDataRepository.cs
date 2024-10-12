using LogisticsCompany.Data.Entity;
using LogisticsCompany.Entity;
using LogisticsCompany.Services.Authorization.Dto;
using LogisticsCompany.Services.Dto;
using LogisticsCompany.Services.Offices.Dto;
using LogisticsCompany.Services.Package.Dto;
using LogisticsCompany.Services.Reports.Dto;
using LogisticsCompany.Services.Users.Dto;

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

        public static IncomeAggregateModel GetIncome()
        {
            return new IncomeAggregateModel
            {
                TotalPrice = 125M
            };
        }

        public static IEnumerable<PackageReportDto> GetAllPackageReports()
        {
            return new List<PackageReportDto>()
            {
                new PackageReportDto
                {
                    FromUser = "Bai Ivan",
                    ToUser = "Bai Pesho",
                    Address = "ul. Geo Milev 1",
                    PackageStatusName = "Registered",
                    ToOffice = true,
                    Weight = 125
                },
                new PackageReportDto
                {
                    FromUser = "Bai Pesho",
                    ToUser = "Bai Pesho",
                    Address = "ul. Geo Milev 2",
                    PackageStatusName = "InDelivery"
                }
            };
        }

        public static IEnumerable<LoginDto> GetAllLogins()
        {
            return new List<LoginDto>()
            {
                new LoginDto
                {
                    Id = 1,
                    Email = "admin@gmail.com",
                    PasswordHash = "HASH123",
                    RoleId = 1
                },
                new LoginDto
                {
                    Id = 2,
                    Email = "test@gmail.com",
                    PasswordHash = "HASH123",
                    RoleId = 2
                }
            };
        }

        public static IEnumerable<UserDto> GetAllUsers()
        {
            return new List<UserDto>()
            {
                new UserDto
                {
                    Id = 1,
                    Email = "admin@gmail.com",
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    OfficeName = "OfficeName1",
                    RoleName = "Admin",
                    Username ="admin123"
                },
                new UserDto
                {
                    Id = 2,
                    Email = "client@gmail.com",
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    OfficeName = "OfficeName2",
                    RoleName = "Client",
                    Username = "client123"
                },
                  new UserDto
                {
                    Id = 3,
                    Email = "employee@gmail.com",
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    OfficeName = "OfficeName2",
                    RoleName = "Employee",
                    Username = "employee123"
                }
            };
        }

        public static IEnumerable<SentReceivedPackageDto> GetAllSentReceivedPackages()
        {
            return new List<SentReceivedPackageDto>()
            {
                new SentReceivedPackageDto
                {
                    Id = 1,
                    Address = "ul.Geo Milev",
                    FromUser = "From",
                    PackageStatusName = "Delivered",
                    ToOffice =  true,
                    ToUser = "To",
                    Weight = 125
                }
            };
        }

        public static IEnumerable<PackageDto> GetAllPackages()
        {
            return new List<PackageDto>()
            {
                new PackageDto
                {
                    Id = 1,
                    DeliveryId = 1,
                    Address = "ul.Geo Milev",
                    FromId = 2,
                    ToId = 2,
                    OfficeId = 1,
                    ToOffice = true,
                    Weight = 125
                },
                new PackageDto
                {
                    Id = 2,
                    DeliveryId = 2,
                    Address = "ul.Geo Milev 2",
                    FromId = 2,
                    ToId = 1,
                    OfficeId = 2,
                    ToOffice = true,
                    Weight = 125
                }

            };
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
