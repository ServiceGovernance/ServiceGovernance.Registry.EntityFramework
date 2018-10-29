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
            model.Endpoints = new[] { new Uri("http://myservice01-qa.com"), new Uri("http://myservice02-qa.com") };
            model.ServiceId = "myId";
            model.PublicUrls = new[] { new Uri("http://myservice-qa.com") };
            model.IpAddresses = new[] { "10.10.0.1", "10.10.0.2" };

            var entity = model.ToEntity();
            entity.DisplayName.Should().Be(model.DisplayName);
            entity.ServiceId.Should().Be(model.ServiceId);
            entity.Endpoints.Should().HaveCount(2);
            entity.Endpoints[0].EndpointUri.Should().Be(model.Endpoints[0].ToString());
            entity.Endpoints[0].ServiceId.Should().Be(entity.Id);
            entity.Endpoints[1].EndpointUri.Should().Be(model.Endpoints[1].ToString());
            entity.Endpoints[1].ServiceId.Should().Be(entity.Id);

            entity.IpAddresses.Should().HaveCount(2);
            entity.IpAddresses[0].IpAddress.Should().Be(model.IpAddresses[0]);
            entity.IpAddresses[0].ServiceId.Should().Be(entity.Id);
            entity.IpAddresses[1].IpAddress.Should().Be(model.IpAddresses[1]);
            entity.IpAddresses[1].ServiceId.Should().Be(entity.Id);

            entity.PublicUrls.Should().HaveCount(1);
            entity.PublicUrls[0].Url.Should().Be(model.PublicUrls[0].ToString());
            entity.PublicUrls[0].ServiceId.Should().Be(entity.Id);

            model = entity.ToModel();

            model.DisplayName.Should().Be(entity.DisplayName);
            model.ServiceId.Should().Be(entity.ServiceId);
            model.Endpoints.Should().HaveCount(2);
            model.Endpoints[0].Should().Be(entity.Endpoints[0].EndpointUri);
            model.Endpoints[1].Should().Be(entity.Endpoints[1].EndpointUri);

            model.IpAddresses.Should().HaveCount(2);
            model.IpAddresses[0].Should().Be(entity.IpAddresses[0].IpAddress);
            model.IpAddresses[1].Should().Be(entity.IpAddresses[1].IpAddress);

            model.PublicUrls.Should().HaveCount(1);
            model.PublicUrls[0].Should().Be(entity.PublicUrls[0].Url);
        }
    }
}
