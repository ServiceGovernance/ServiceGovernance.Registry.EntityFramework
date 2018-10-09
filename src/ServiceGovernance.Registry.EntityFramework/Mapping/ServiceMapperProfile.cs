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
                .ForMember(dest => dest.ServiceEndpoints, opt => opt.MapFrom(src => src.Endpoints));

            CreateMap<Models.Service, Entities.Service>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Endpoints, opt => opt.MapFrom(src => src.ServiceEndpoints));

            CreateMap<Uri, Entities.ServiceEndpoint>()
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.Service, opt => opt.Ignore())
                 .ForMember(dest => dest.ServiceId, opt => opt.Ignore())
               .ForMember(dest => dest.EndpointUri, opt => opt.MapFrom(src => src.ToString()));

            CreateMap<Entities.ServiceEndpoint, Uri>()
                .ConstructUsing(src => new Uri(src.EndpointUri))
                 .ForMember(dest => dest.Segments, opt => opt.Ignore());
        }
    }
}
