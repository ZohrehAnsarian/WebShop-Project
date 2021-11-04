using Model.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels.FlashCard
{
    public class VmFlashCardCollection : BaseViewModel
    {
        public IEnumerable<VmFlashCard> FlashCardList { get; set; }
        public bool AllWords { get; set; }
        public bool ByFront { get; set; }
    }
}
