using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Plugin.Customers.NopChat.Models;
using Nop.Plugin.Customers.NopChat.Services;
using Nop.Plugin.Customers.NopChat.Factories;
using Nop.Services.Common;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Nop.Plugin.Customers.NopChat.Hubs;
using DocumentFormat.OpenXml.EMMA;
using System.Net.NetworkInformation;

namespace Nop.Plugin.Customers.NopChat.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    //[AutoValidateAntiforgeryToken]
    public class NopChatController : BasePluginController
    {
        #region Fields
        private readonly IPermissionService _permissionService;
        private readonly INopChatMessageModelFactory _NopChatMessageModelFactory;
        private readonly INopChatMessageService _NopChatMessageService;

        #endregion

        #region Ctor

        public NopChatController(
            IPermissionService permissionService,
			INopChatMessageModelFactory NopChatMessageModelFactory,
			INopChatMessageService NopChatMessageService
        )
        {
            _permissionService = permissionService;
			_NopChatMessageModelFactory = NopChatMessageModelFactory;
			_NopChatMessageService = NopChatMessageService;
        }

        #endregion

        #region Methods


        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            //prepare model
            var m = new NopChatSearchModel();
            var model = await _NopChatMessageModelFactory.PrepareNopChatsListModelAsync(m);
            return View("~/Plugins/Customers.NopChat/Views/Configure.cshtml", model);
        }

        public async Task<IActionResult> Chat(int id)
        {
            NopChatModel model = await _NopChatMessageService.GetChat(id);
            return View("~/Plugins/Customers.NopChat/Views/AdminChat.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> List(NopChatSearchModel searchModel)
        {
            //prepare model
            var model = await _NopChatMessageModelFactory.NopChatSearchModelAsync(searchModel);
            return Json(model);
        }

      
        

        #endregion
    }
}
