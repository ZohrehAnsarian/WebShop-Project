using Model.ViewModels;
using Repository.EF.Repository;
using System.Collections.Generic;
using System.Linq;
using BLL.Base;
using Model.ToolsModels.DropDownList;

namespace BLL
{
    public class BLQuantityUnit : BLBase
    {
        QuantityUnitRepository quantityUnitRepository;
        public BLQuantityUnit(int currentLanguageId) : base(currentLanguageId)
        {
            quantityUnitRepository = UnitOfWork.GetRepository<QuantityUnitRepository>();
        }
        public IEnumerable<VmSelectListItem> GetQuantityUnitSelectListItem(int index, int count)
        {

            var quantityUnitList = quantityUnitRepository.GetAll();

            var vmSelectListItem = from unit in quantityUnitList
                                    select new VmSelectListItem
                                    {
                                        Value = unit.Id.ToString(),
                                        Text = unit.Name,
                                    };

            return vmSelectListItem;
        }
    }
}
