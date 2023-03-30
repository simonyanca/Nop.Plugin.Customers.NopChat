
using System;
using System.ComponentModel.DataAnnotations;
using Nop.Core;


namespace Nop.Plugin.Customers.NopChat.Services
{
    public class NopChatMessage: BaseEntity
    {
        [Required]
        public int StoreId { get; set; }

        [Required]
        public string ChatId { get; set; }

        [Required]
        public string FromUserId { get; set; }

        [Required]
		public string ToUserId { get; set; }

		
		[Required]
        public string Message { get; set; }

		[Required]
		public bool Closed { get; set; } 


		[Required]
		public DateTime CreatedOnUtc { get; set; }

	}


}
