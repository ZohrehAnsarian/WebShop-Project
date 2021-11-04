using Model;

using Repository.EF.Base;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repository.EF.Repository
{
    public class BaseFeatureTypeRepository : EFBaseRepository<BaseFeatureType>
    {
        public IEnumerable<BaseFeatureType> GetAll(string searchText = "")
        {
            var baseBaseFeatureTypeList = from s in Context.BaseFeatureTypes.AsNoTracking()
                                          where (
                                    s.Name.Contains(searchText)
                                  
                                 )
                                  select s;

            return baseBaseFeatureTypeList.OrderBy(A => A.Name).ToArray();
        }
        public IEnumerable<BaseFeatureType> GetBaseFeatureType(int id)
        {
            var baseBaseFeatureTypeList = from s in Context.BaseFeatureTypes.AsNoTracking()
                                          where s.Id == id
                                  select s;

            return baseBaseFeatureTypeList.OrderBy(A => A.Name).ToArray();
        }
        public IEnumerable<BaseFeatureType> GetBaseFeatureTypeWithDetails()
        {
            var baseBaseFeatureType = (from s in Context.BaseFeatureTypes.AsNoTracking().Include(f => f.BaseFeatureTypeDetails)
                               select s);

            return baseBaseFeatureType.OrderBy(f => f.Name);
        }
        public BaseFeatureType GetBaseFeatureTypeWithDetails(int id)
        {
            var baseBaseFeatureType = (from s in Context.BaseFeatureTypes.AsNoTracking().Include(f => f.BaseFeatureTypeDetails)
                               where s.Id == id
                               select s).First();

            return baseBaseFeatureType;
        }
        public IEnumerable<BaseFeatureType> GetBaseFeatureTypeWithDetails(IEnumerable<int> ids)
        {
            var baseBaseFeatureType = (from s in Context.BaseFeatureTypes.AsNoTracking().Include(f => f.BaseFeatureTypeDetails)
                               where ids.Contains(s.Id)
                               select s).ToArray();

            return baseBaseFeatureType;
        }
        public void AddBaseFeatureType(BaseFeatureType baseBaseFeatureType)
        {
            Add(baseBaseFeatureType);
        }

        public void UpdateBaseFeatureType(BaseFeatureType baseBaseFeatureType)
        {
            Update(baseBaseFeatureType);
        }

        public bool DeleteBaseFeatureType(int id)
        {
            var deleteable = Context.BaseFeatureTypes.Find(id);
            Delete(deleteable);
            return true;
        }

       
    }
}
