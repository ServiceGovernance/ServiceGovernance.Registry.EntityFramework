using AutoMapper;
using System.Collections.Generic;

namespace ServiceGovernance.Registry.EntityFramework.Mapping
{
    /// <summary>
    /// Extensions methods to map from or to entites/models
    /// </summary>
    public static class MappingExtensions
    {
        static MappingExtensions()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ServiceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Models.Service ToModel(this Entities.Service entity)
        {
            return Mapper.Map<Models.Service>(entity);
        }

        /// <summary>
        /// Maps an entity list to a model list.
        /// </summary>
        /// <param name="entityList">The entity list.</param>
        /// <returns></returns>
        public static List<Models.Service> ToModelList(this IEnumerable<Entities.Service> entityList)
        {
            return Mapper.Map<List<Models.Service>>(entityList);
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Entities.Service ToEntity(this Models.Service model)
        {
            return Mapper.Map<Entities.Service>(model);
        }

        /// <summary>
        /// Updates an entity from a model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="entity">The entity.</param>
        public static void UpdateEntity(this Models.Service model, Entities.Service entity)
        {
            Mapper.Map(model, entity);
        }
    }
}
