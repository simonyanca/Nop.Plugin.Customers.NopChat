using Nop.Web.Framework.Models;

namespace Nop.Plugin.Customers.NopChat.Models
{
    public partial record NopChatMessagesListModel : BasePagedListModel<NopChatMessageModel>
    {
    }

    public partial record NopChatListModel : BasePagedListModel<HeadNopChatModel>
    {
    }
}
