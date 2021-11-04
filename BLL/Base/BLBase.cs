using Repository.Core;
using Repository.EF.UnitOfWork;
using System.ComponentModel;

namespace BLL.Base
{

    [DataObject(true)]
    public abstract class BLBase
    {
        public int CurrentLanguageId { get; private set; }
        public BLBase()
        {
        }
        public BLBase(int languageId)
        {
            CurrentLanguageId = languageId;

        }
        private IUnitOfWork _UnitOfWork;
        protected IUnitOfWork UnitOfWork
        {
            get
            {
                if (_UnitOfWork == null)
                {
                    _UnitOfWork = new EFUnitOfWork(CurrentLanguageId);
                }

                return _UnitOfWork;
            }
        }

    }
}
