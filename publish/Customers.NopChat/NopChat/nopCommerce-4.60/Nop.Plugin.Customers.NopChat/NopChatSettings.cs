﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Nop.Core.Configuration;

namespace Nop.Plugin.Customers.NopChat
{
    /// <summary>
    /// Represents a plugin settings
    /// </summary>
    public class NopChatSettings : ISettings
    {
        public bool Debug { get; set; }
		public bool ViewAllZones { get; set; }
	}
}