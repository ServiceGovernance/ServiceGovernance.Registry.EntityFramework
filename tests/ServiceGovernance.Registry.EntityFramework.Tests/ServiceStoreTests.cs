using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ServiceGovernance.Registry.EntityFramework.Mapping;
using ServiceGovernance.Registry.EntityFramework.Options;
using ServiceGovernance.Registry.EntityFramework.Stores;
using ServiceGovernance.Registry.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceGovernance.Registry.EntityFramework.Tests
{
    [TestFixture]
    public class ServiceStoreTests : DbAwareTests<RegistryDbContext, RegistryStoreOptions>
    {
        public ServiceStoreTests() : base("ServiceStoreTests")
        {
            using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                context.Database.EnsureCreated();
        }

        public class FindByServiceIdAsyncMethod : ServiceStoreTests
        {
            [Test]
            public async Task Returns_Item_When_Service_Exists()
            {
                var serviceModel = new Service
                {
                    ServiceId = "Returns_Item_When_Service_Exists",
                    DisplayName = "Test Service"
                };

                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    context.Services.Add(serviceModel.ToEntity());
                    context.SaveChanges();
                }

                Service service;
                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ServiceStore(context, new Mock<ILogger<ServiceStore>>().Object);
                    service = await store.FindByServiceIdAsync(serviceModel.ServiceId);
                }

                service.Should().NotBeNull();
            }

            [Test]
            public async Task Returns_Null_When_Service_Not_Exists()
            {
                Service service;
                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ServiceStore(context, new Mock<ILogger<ServiceStore>>().Object);
                    service = await store.FindByServiceIdAsync("someServiceId");
                }

                service.Should().BeNull();
            }

            [Test]
            public async Task Returns_Item_With_All_Properties_When_Service_Exists()
            {
                var serviceModel = new Service
                {
                    ServiceId = "Returns_Item_With_All_Properties_When_Service_Exists",
                    DisplayName = "Test Service",
                    ServiceEndpoints = new Uri[] { new Uri("http://test.com") }
                };

                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    context.Services.Add(serviceModel.ToEntity());
                    context.SaveChanges();
                }

                Service service;
                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ServiceStore(context, new Mock<ILogger<ServiceStore>>().Object);
                    service = await store.FindByServiceIdAsync(serviceModel.ServiceId);
                }

                service.Should().NotBeNull();
                service.ServiceId.Should().Be(serviceModel.ServiceId);
                service.DisplayName.Should().Be(serviceModel.DisplayName);
                service.ServiceEndpoints.Should().HaveCount(1);
                service.ServiceEndpoints[0].Should().Be(serviceModel.ServiceEndpoints[0]);
            }
        }

        public class GetAllAsyncMethod : ServiceStoreTests
        {
            [Test]
            public async Task Returns_All_Items()
            {
                var model1 = new Service
                {
                    ServiceId = "AllItem1",
                    DisplayName = "Test Service"
                };

                var model2 = new Service
                {
                    ServiceId = "AllItem2",
                    DisplayName = "Test Service 2"
                };

                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    context.Services.Add(model1.ToEntity());
                    context.Services.Add(model2.ToEntity());
                    context.SaveChanges();
                }

                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ServiceStore(context, new Mock<ILogger<ServiceStore>>().Object);
                    var services = (await store.GetAllAsync()).ToList();

                    services.Should().HaveCount(2);
                    services[0].ServiceId.Should().Be(model1.ServiceId);
                    services[0].DisplayName.Should().Be(model1.DisplayName);
                    services[1].ServiceId.Should().Be(model2.ServiceId);
                    services[1].DisplayName.Should().Be(model2.DisplayName);
                }
            }
        }

        public class RemoveAsyncMethod : ServiceStoreTests
        {
            [Test]
            public async Task Removes_Existing_Item()
            {
                var model = new Service
                {
                    ServiceId = "Removes_Existing_Item",
                    DisplayName = "Test Service"
                };

                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    context.Services.Add(model.ToEntity());
                    context.SaveChanges();
                }

                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ServiceStore(context, new Mock<ILogger<ServiceStore>>().Object);
                    await store.RemoveAsync(model.ServiceId);
                }

                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    context.Services.SingleOrDefault(s => s.ServiceId == model.ServiceId).Should().BeNull();
                }
            }

            [Test]
            public void Does_Not_Throw_On_NonExisting_Item()
            {
                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ServiceStore(context, new Mock<ILogger<ServiceStore>>().Object);
                    Func<Task> action = async () => await store.RemoveAsync("anyserviceid");

                    action.Should().NotThrow();
                }
            }
        }

        public class StoreAsyncMethod : ServiceStoreTests
        {
            [Test]
            public async Task Saves_New_Item()
            {
                var model = new Service
                {
                    ServiceId = "Saves_New_Item",
                    DisplayName = "Test Service"
                };

                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ServiceStore(context, new Mock<ILogger<ServiceStore>>().Object);
                    await store.StoreAsync(model);
                }

                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    context.Services.SingleOrDefault(s => s.ServiceId == model.ServiceId).Should().NotBeNull();
                }
            }

            [Test]
            public async Task Updates_Existing_Item()
            {
                var model = new Service
                {
                    ServiceId = "Updates_Existing_Item",
                    DisplayName = "Test Service"
                };

                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    context.Services.Add(model.ToEntity());
                    context.SaveChanges();
                }

                model.DisplayName = "New name";

                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ServiceStore(context, new Mock<ILogger<ServiceStore>>().Object);
                    await store.StoreAsync(model);
                }

                using (var context = new RegistryDbContext(DbContextOptions, StoreOptions))
                {
                    var item = context.Services.SingleOrDefault(s => s.ServiceId == model.ServiceId);
                    item.Should().NotBeNull();
                    item.DisplayName.Should().Be(model.DisplayName);
                }
            }
        }
    }
}
