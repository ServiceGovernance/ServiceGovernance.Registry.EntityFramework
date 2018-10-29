using AutoMapper;
using System;

namespace ServiceGovernance.Registry.EntityFramework.Mapping
{
    /// <summary>
    /// Defines mapping for services
    /// </summary>    
    public class ServiceMapperProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the service mapper profile
        /// </summary>
        public ServiceMapperProfile()
        {
            CreateMap<Entities.Service, Models.Service>()
                .ForMember(dest => dest.Endpoints, opt => opt.MapFrom(src => src.Endpoints))
                .ForMember(dest => dest.IpAddresses, opt => opt.MapFrom(src => src.IpAddresses))
                .ForMember(dest => dest.PublicUrls, opt => opt.MapFrom(src => src.PublicUrls));

            CreateMap<Models.Service, Entities.Service>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Endpoints, opt => opt.MapFrom(src => src.Endpoints))
                .ForMember(dest => dest.IpAddresses, opt => opt.MapFrom(src => src.IpAddresses))
                .ForMember(dest => dest.PublicUrls, opt => opt.MapFrom(src => src.PublicUrls));

            CreateMap<Uri, Entities.ServiceEndpoint>()
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.Service, opt => opt.Ignore())
                 .ForMember(dest => dest.ServiceId, opt => opt.Ignore())
               .ForMember(dest => dest.EndpointUri, opt => opt.MapFrom(src => src.ToString()));

            CreateMap<Uri, Entities.ServicePublicUrl>()
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.Service, opt => opt.Ignore())
                 .ForMember(dest => dest.ServiceId, opt => opt.Ignore())
               .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.ToString()));

            CreateMap<string, Entities.ServiceIpAddress>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.Service, opt => opt.Ignore())
               .ForMember(dest => dest.ServiceId, opt => opt.Ignore())
             .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src));

            CreateMap<Entities.ServiceEndpoint, Uri>()
                .ConstructUsing(src => new Uri(src.EndpointUri))
                 .ForMember(dest => dest.Segments, opt => opt.Ignore());

            CreateMap<Entities.ServicePublicUrl, Uri>()
              .ConstructUsing(src => new Uri(src.Url))
               .ForMember(dest => dest.Segments, opt => opt.Ignore());

            CreateMap<Entities.ServiceIpAddress, string>()
            .ConstructUsing(src => src.IpAddress);
        }
    }
}
