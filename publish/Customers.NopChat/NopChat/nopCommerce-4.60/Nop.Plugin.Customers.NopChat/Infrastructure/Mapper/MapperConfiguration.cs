
using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Customers.NopChat.Models;
using Nop.Plugin.Customers.NopChat.Services;
using Nop.Services.Customers;

namespace Nop.Plugin.Customers.NopChat.Infrastructure.Mapper
{
    /// <summary>
    /// Represents AutoMapper configuration for plugin models
    /// </summary>
    public class MapperConfiguration : Profile, IOrderedMapperProfile
    {
        #region Ctor

        public MapperConfiguration()
        {
            CreateMap<NopChatMessage, HeadNopChatModel>()
                .ForMember(model => model.UpdateOnUtc, options => options.MapFrom(o => o.CreatedOnUtc.ToString("dd/MM/yyyy hh:mm:ss")))
                .ForMember(model => model.LastMessage, options => options.MapFrom(o => o.Message));

            CreateMap<NopChatMessage, NopChatMessageModel>();

		}

        #endregion

        #region Properties

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 1;

        #endregion
    }
}