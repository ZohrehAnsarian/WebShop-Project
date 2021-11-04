using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;

namespace Repository.EF.Repository
{
    public class QuantityUnitRepository : EFBaseRepository<QuantityUnit>
    {
        public IEnumerable<QuantityUnit> GetAll()
        {
            var quantityUnitList = from unit in Context.QuantityUnits
                              select unit;

            return quantityUnitList.OrderBy(A => A.Name).ToArray();
        }
    }
}
