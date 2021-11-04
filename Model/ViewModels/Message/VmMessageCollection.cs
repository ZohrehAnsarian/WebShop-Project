using Model.Base;
using System;
using System.Collections.Generic;

namespace Model.ViewModels.Message
{
    public class VmMessageCollection : BaseViewModel
    {
        public IEnumerable<VmMessage> MessageList { get; set; }

    }
}
