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
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public NopChatMessageModelFactory(INopChatMessageService NopChatMessageService, ILocalizationService localizationService, IStoreService storeService, ICustomerService customerService)
        {
            _NopChatMessageService = NopChatMessageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _customerService = customerService;
        }

        public async Task<NopChatListModel> NopChatSearchModelAsync(NopChatSearchModel searchModel)
        {
            var pickupPoints = await _NopChatMessageService.GetLastMessagesAsync(0,pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);
            NopChatListModel model = await new NopChatListModel().PrepareToGridAsync(searchModel, pickupPoints, () =>
            {
                return pickupPoints.SelectAwait(async point =>
                {
                    var store = await _storeService.GetStoreByIdAsync(point.StoreId);
                    HeadNopChatModel m = point.ToModel<HeadNopChatModel>();
                    //m.CustomerName = (await _customerService.GetCustomerByIdAsync(point.ReceiverId)).Username;
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
