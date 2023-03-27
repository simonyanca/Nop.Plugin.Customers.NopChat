
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Customers.NopChat.Models;

namespace Nop.Plugin.Customers.NopChat.Services
{
    public partial interface INopChatMessageService
	{
        Task<IPagedList<NopChatMessage>> GetAllNopChatMessagesAsync(int storeId = 0, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<NopChatMessage>> GetLastMessagesAsync(int storeId = 0, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<NopChatMessage> GetNopChatMessageByIdAsync(int messageId);

        Task InsertNopChatMessageAsync(NopChatMessage message);

        Task<NopChatModel> GetChat(int id);
        Task<NopChatModel> GetChat(string userId);

    }
}