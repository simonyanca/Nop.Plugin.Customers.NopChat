
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Data;
using Nop.Plugin.Customers.NopChat.Models;
using Nop.Plugin.Customers.NopChat.Services;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using NUglify.Helpers;

namespace Nop.Plugin.Customers.NopChat.Services
{
    /// <summary>
    /// Store pickup point service
    /// </summary>
    public partial class NopChatMessageService : INopChatMessageService
    {
        #region Constants


        private readonly IStoreContext _storeContext;

        #endregion

        #region Fields

        private readonly IRepository<NopChatMessage> _NopChatMessageRepository;


        #endregion

        #region Ctor


        public NopChatMessageService(IRepository<NopChatMessage> NopChatMessageRepository, IStoreContext storeContext)
        {
            _NopChatMessageRepository = NopChatMessageRepository;
            _storeContext = storeContext;
        }

        #endregion

        #region Methods



        public virtual async Task<IPagedList<NopChatMessage>> GetLastMessagesAsync(int storeId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = (
                       from d in _NopChatMessageRepository.Table.AsEnumerable()
                       where d.StoreId == storeId
                       group d by d.ChatId into grp
                       select new { c = grp.Key, v = grp.OrderByDescending(r => r.Id).First() });

            int totalCount = query.Count();

            var rez = await query
                        .OrderByDescending(r => r.v.Id)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .Select(r => r.v).ToListAsync();

            return new PagedList<NopChatMessage>(rez, pageIndex, pageSize, totalCount);
        }

        public virtual async Task<IPagedList<NopChatMessage>> GetAllNopChatMessagesAsync(int storeId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _NopChatMessageRepository.Table.Where(point => point.StoreId == storeId);
            int totalCount = await query.CountAsync();

            query = query.OrderBy(r => r.CreatedOnUtc).Skip(pageIndex * pageSize).Take(pageSize);

            var rez = await query.ToListAsync();
            var p = new PagedList<NopChatMessage>(rez, pageIndex, pageSize, totalCount);

            return p;
        }


        public virtual async Task InsertNopChatMessageAsync(NopChatMessage message)
        {
            message.StoreId = _storeContext.GetCurrentStore().Id;
            message.CreatedOnUtc = DateTime.Now;   
            await _NopChatMessageRepository.InsertAsync(message, false);
        }

        public async Task<NopChatMessage> GetNopChatMessageByIdAsync(int messageId, int storeId)
        {
            NopChatMessage m = await _NopChatMessageRepository.GetByIdAsync(messageId);
            return m;
        }

        public async Task<NopChatModel> GetChat(int id, int storeId)
        {
            NopChatMessage m = await _NopChatMessageRepository.GetByIdAsync(id);
            return await GetChat(m.FromUserId, storeId);
        }

        public async Task<NopChatModel> GetChat(string userId, int storeId)
        {
            IEnumerable<NopChatMessage> messages = await _NopChatMessageRepository.Table.Where(r => r.StoreId == storeId &&( r.FromUserId == userId || r.ToUserId == userId))
                .OrderByDescending(r => r.Id).ToListAsync();
            NopChatModel model = new NopChatModel()
            {
                Messages = messages.Select(r => r.ToModel<NopChatMessageModel>()),
                UserId = userId
            };
            
            return model;
        }
	}
        #endregion
}