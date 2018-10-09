using FluentAssertions;
using NUnit.Framework;
using ServiceGovernance.Registry.EntityFramework.Mapping;
using ServiceGovernance.Registry.Models;
using System;

namespace ServiceGovernance.Registry.EntityFramework.Tests
{
    [TestFixture]
    public class ServiceMapperTests
    {
        [Test]
        public void AutomapperConfigurationIsValid()
        {
            MappingExtensions.Mapper.ConfigurationProvider.AssertConfigurationIsValid<ServiceMapperProfile>();
        }

        [Test]
        public void Can_Convert_To_And_From_Entity()
        {
            var model = new Service();
            var entity = model.ToEntity();
            model = entity.ToModel();

            entity.Should().NotBeNull();
            model.Should().NotBeNull();
        }

        [Test]
        public void Maps_All_Properties()
        {
            var model = new Service();
            model.DisplayName = "DispayName";
            model.ServiceEndpoints = new[] { new Uri("http://test.com"), new Uri("http://localhost:531") };
            model.ServiceId = "myId";

            var entity = model.ToEntity();
            entity.DisplayName.Should().Be(model.DisplayName);
            entity.ServiceId.Should().Be(model.ServiceId);
            entity.Endpoints.Should().HaveCount(2);
            entity.Endpoints[0].EndpointUri.Should().Be(model.ServiceEndpoints[0].ToString());
            entity.Endpoints[0].ServiceId.Should().Be(entity.Id);

            entity.Endpoints[1].EndpointUri.Should().Be(model.ServiceEndpoints[1].ToString());
            entity.Endpoints[1].ServiceId.Should().Be(entity.Id);

            model = entity.ToModel();

            model.DisplayName.Should().Be(entity.DisplayName);
            model.ServiceId.Should().Be(entity.ServiceId);
            model.ServiceEndpoints.Should().HaveCount(2);
            model.ServiceEndpoints[0].Should().Be(entity.Endpoints[0].EndpointUri);
            model.ServiceEndpoints[1].Should().Be(entity.Endpoints[1].EndpointUri);
        }
    }
}
