using Model.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModels.FlashCard
{
    public class VmFlashCard : BaseViewModel
    {
        public int Id { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }


    }
}
