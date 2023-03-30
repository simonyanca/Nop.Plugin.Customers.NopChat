

using Nop.Plugin.Customers.NopChat.Models;
using System.Threading.Tasks;

namespace Nop.Plugin.Customers.NopChat.Factories
{
    /// <summary>
    /// Represents the store pickup point models factory
    /// </summary>
    public interface INopChatMessageModelFactory
	{
        Task<NopChatSearchModel> PrepareNopChatsListModelAsync(NopChatSearchModel searchModel);
        Task<NopChatListModel> NopChatSearchModelAsync(NopChatSearchModel searchModel);


    }
}