using Nop.Core;
using Nop.Core.Caching;

namespace Nop.Plugin.Customers.NopChat
{
    /// <summary>
    /// Represents plugin constants
    /// </summary>
    public static class NopChatDefaults
    {
        public static string SystemName => "Customers.NopChat";

		public const string VIEW_COMPONENT_NAME = "ChatView";

		public static CacheKey WidgetZoneHtmlCacheKey => new("Nop.Plugin.Customers.NopChat.WidgetZoneHtmlKey");

	}
}