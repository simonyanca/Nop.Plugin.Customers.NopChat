
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Customers.NopChat.Models;

namespace Nop.Plugin.Customers.NopChat.Services
{
    public partial interface INopChatMessageService
	{
        Task<IPagedList<NopChatMessage>> GetAllNopChatMessagesAsync(int storeId, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<NopChatMessage>> GetLastMessagesAsync(int storeId, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<NopChatMessage> GetNopChatMessageByIdAsync(int messageId, int storeId);

        Task InsertNopChatMessageAsync(NopChatMessage message);

        Task<NopChatModel> GetChat(int id, int storeId);
        Task<NopChatModel> GetChat(string userId, int storeId);

    }
}