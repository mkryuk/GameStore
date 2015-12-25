using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.WebUI.Models
{
    public class NavigationViewModel
    {
        public IEnumerable<string> Categories { get; set; }
        public string CurrentCategory { get; set; }
    }
}