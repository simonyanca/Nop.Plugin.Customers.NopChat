using Nop.Web.Framework.Models;

namespace Nop.Plugin.Customers.NopChat.Models
{
    public record NopChatSearchModel : BaseSearchModel
    {
        public int StoreId { get; set; }
    }
}
