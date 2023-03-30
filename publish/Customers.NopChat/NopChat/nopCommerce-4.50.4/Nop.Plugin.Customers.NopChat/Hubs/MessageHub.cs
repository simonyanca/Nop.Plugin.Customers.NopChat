


using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using LinqToDB.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Nop.Core;
using Nop.Plugin.Customers.NopChat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Nop.Plugin.Customers.NopChat.Hubs
{
    
    public class MessageHub : Hub
    {
        private readonly INopChatMessageService _nopChatService;
        private readonly IHubContext<MessageHub> _messageHubContext;
        private readonly IStoreContext _storeContext;
		private static Dictionary<string, string> userConIdMap = new Dictionary<string, string>();

        public MessageHub(IHubContext<MessageHub> messageHubContext, INopChatMessageService nopChatService, IStoreContext StoreContext)
        {
            _messageHubContext = messageHubContext;
            _nopChatService = nopChatService;
            _storeContext = StoreContext;
        }
        
        public override Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Request.Query["userId"];
            Groups.AddToGroupAsync(Context.ConnectionId, userId).Wait();
            userConIdMap[Context.ConnectionId] = userId;

            if (userId != "Admin")
                _messageHubContext.Clients.Group("Admin").SendAsync("ImAlive", userId).Wait();
            
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string userId;
            if (userConIdMap.TryGetValue(Context.ConnectionId, out userId))
                _messageHubContext.Clients.Group("Admin").SendAsync("Disconected", userId).Wait();
            
            return base.OnDisconnectedAsync(exception);
        }

        public async Task GetChatMessages(string chatId, int limit)
        {
			var storeId = _storeContext.GetCurrentStore().Id;
			var chat = await _nopChatService.GetChat(chatId, storeId);
            var messages = chat.Messages.Take(limit);
			await _messageHubContext.Clients.Client(this.Context.ConnectionId).SendAsync("ChatMessages", messages);
		}

        public async Task AreYouAlive(string userId)
        {
			await _messageHubContext.Clients.Group(userId).SendAsync("AreYouAlive");
		}

		public async Task ImAlive()
		{
            string userId;
            if (userConIdMap.TryGetValue(Context.ConnectionId, out userId))
                await _messageHubContext.Clients.Group("Admin").SendAsync("ImAlive", userId);
		}

		public async Task UserSendMessage(string message)
        {
            string UserId = userConIdMap[this.Context.ConnectionId];
            NopChatMessage m = new NopChatMessage()
            {
                FromUserId = UserId,
                ToUserId = "Admin",
                Message = message,
                ChatId = UserId,
                CreatedOnUtc = DateTime.Now
            };
            await _nopChatService.InsertNopChatMessageAsync(m);

            await _messageHubContext.Clients.Group("Admin").SendAsync("RefreshMessageList");
            await _messageHubContext.Clients.Group("Admin").SendAsync("Message", message, UserId);  
        }

        public async Task AdminSendMessage(string message, string userId)
        {
            string connId = string.Empty;
            
            NopChatMessage m = new NopChatMessage()
            {
                FromUserId = "Admin",
                ToUserId = userId,
                Message = message,
                ChatId = userId,
                CreatedOnUtc = DateTime.Now
            };
            await _nopChatService.InsertNopChatMessageAsync(m);

            await _messageHubContext.Clients.Group("Admin").SendAsync("RefreshMessageList");
            await _messageHubContext.Clients.Group(userId).SendAsync("Message", message, "Admin");
            
        }
    }
}
