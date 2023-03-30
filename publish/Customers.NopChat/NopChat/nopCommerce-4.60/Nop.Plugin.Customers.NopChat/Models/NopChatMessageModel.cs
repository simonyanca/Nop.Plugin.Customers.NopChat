using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;
using System;

namespace Nop.Plugin.Customers.NopChat.Models
{
    public record NopChatMessageModel : BaseNopEntityModel
    {
        public NopChatMessageModel()
        {
            
        }

        [NopResourceDisplayName("Plugins.Customers.NopChat.Fields.ConnectionId")]
        public string ConnectionId { get; set; }


        [NopResourceDisplayName("Plugins.Customers.NopChat.Fields.Message")]
		public string Message { get; set; }

		
		[NopResourceDisplayName("Plugins.Customers.NopChat.Fields.Closed")]
		public bool Closed { get; set; }


        public string FromUserId { get; set; }
        public string ToUserId { get; set; }


        [DataType(DataType.DateTime)]
		[NopResourceDisplayName("Plugins.Customers.NopChat.Fields.CreatedDateTime")]
		public DateTime CreatedOnUtc { get; set; }
    }

   
}