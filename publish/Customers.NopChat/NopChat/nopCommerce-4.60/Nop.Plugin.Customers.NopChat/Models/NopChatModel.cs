using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Customers.NopChat.Models
{
    public class NopChatModel
    { 
        public string UserId { get; set; }
        public IEnumerable<NopChatMessageModel> Messages { get; set; }
    }
}
