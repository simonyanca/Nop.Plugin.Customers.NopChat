
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;
using System;

namespace Nop.Plugin.Customers.NopChat.Models
{
    public record HeadNopChatModel : BaseNopEntityModel
    {
        public HeadNopChatModel()
        {
            
        }


		[NopResourceDisplayName("Plugins.Customers.NopChat.Fields.ChatId")]
		public string ChatId { get; set; }

		public string ToUserId { get; set; }

		public string FromUserId { get; set; }

		[NopResourceDisplayName("Plugins.Customers.NopChat.Fields.Message")]
		public string LastMessage { get; set; }


        [NopResourceDisplayName("Plugins.Customers.NopChat.Fields.UpdateDateTime")]
		public string UpdateOnUtc { get; set; }
    }

   
}