using System;
using System.Linq;
using System.Threading.Tasks;
using Nop.Plugin.Customers.NopChat.Models;
using Nop.Plugin.Customers.NopChat.Services;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Customers.NopChat.Factories
{
    /// <summary>
    /// Represents store pickup point models factory implementation
    /// </summary>
    public class NopChatMessageModelFactory : INopChatMessageModelFactory
	{
        #region Fields

        private readonly INopChatMessageService _NopChatMessageService;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public NopChatMessageModelFactory(INopChatMessageService NopChatMessageService, ICustomerService customerService)
        {
            _NopChatMessageService = NopChatMessageService;
            _customerService = customerService;
        }

        public async Task<NopChatListModel> NopChatSearchModelAsync(NopChatSearchModel searchModel)
        {
			var data = await _NopChatMessageService.GetLastMessagesAsync(searchModel.StoreId, pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);
            NopChatListModel model = await new NopChatListModel().PrepareToGridAsync(searchModel, data, () =>
            {
                return data.SelectAwait(async e =>
                {
                    HeadNopChatModel m = e.ToModel<HeadNopChatModel>();
                    if (e.FromUserId == "Admin")
                        m.Name = "Admin";
                    else
                    {
						var user = await _customerService.GetCustomerByEmailAsync(e.FromUserId);
						m.Name = user == null ? "Anonymous" : user.Username;
					}
                    
                    return m;
                });
            });

            return model;
        }

        public Task<NopChatSearchModel> PrepareNopChatsListModelAsync(NopChatSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return Task.FromResult(searchModel);
        }


        #endregion

        #region Methods


        #endregion
    }
}
