
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Plugin.Customers.NopChat.Components;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Customers.NopChat
{   
    public class NopChat : BasePlugin, IMiscPlugin, IWidgetPlugin
    { 
        private readonly IWebHelper _webHelper;
        private readonly WidgetSettings _widgetSettings;
        private readonly ISettingService _settingService;
		private readonly ILocalizationService _localizationService;

		public bool HideInWidgetList => true;

        public NopChat(  IWebHelper webHelper, WidgetSettings widgetSettings, ISettingService settingService, ILocalizationService localizationService)
        {
            _webHelper = webHelper;
            _widgetSettings = widgetSettings;
            _settingService = settingService;
            _localizationService = localizationService;
		}

        public Type GetWidgetViewComponent(string widgetZone)
        {
            if (widgetZone == null)
                throw new ArgumentNullException(nameof(widgetZone));

			return typeof(ChatViewComponent);
		}

        public async Task<IList<string>> GetWidgetZonesAsync()
        {
            return new List<string> { PublicWidgetZones.Footer };
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/NopChat/Configure";
        }

        public override async Task InstallAsync()
        {
            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(NopChatDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(NopChatDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Customers.NopChat.Fields.OpenChat"] = "Open",
                ["Plugins.Customers.NopChat.Fields.ChatId"] = "Id",
                ["Plugins.Customers.NopChat.Fields.LastMessage"] = "Last Message",
                ["Plugins.Customers.NopChat.Fields.DateTime"] = "Date Time",
				["Plugins.Customers.NopChat.Fields.UpdateDateTime"] = "Date Time",
				["Plugins.Customers.NopChat.Fields.Name"] = "Name",

				["Plugins.Customers.NopChat.Text.Chat"] = "Chat",
                ["Plugins.Customers.NopChat.Text.Close"] = "Close",
                ["Plugins.Customers.NopChat.Text.Send"] = "Send",
				["Plugins.Customers.NopChat.Text.WriteMessage"] = "Write message...",
				["Plugins.Customers.NopChat.Text.OpenChat"] = "Open",
				["Plugins.Customers.NopChat.Text.Disconnected"] = "Disconnected"
			});

			await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await base.UninstallAsync();
        }

		public string GetWidgetViewComponentName(string widgetZone)
		{
            return NopChatDefaults.VIEW_COMPONENT_NAME;
		}
	}
}