using Model;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class BaseFeatureTypeDetailRepository : EFBaseRepository<BaseFeatureTypeDetail>
    {
        public IEnumerable<BaseFeatureTypeDetail> GetAll()
        {
            var baseFeatureTypeDetailList = from s in Context.BaseFeatureTypeDetails.AsNoTracking()
                                            select s;

            return baseFeatureTypeDetailList.OrderBy(A => A.Name).ToArray();
        }
        public int GetFeatureDetailTypeCountByFeaturType(int id)
        {
            return (from s in Context.BaseFeatureTypeDetails.AsNoTracking()
                    where s.BaseFeatureTypeId == id
                    select s).Count();

        }
        public void BatchAddBaseFeatureTypeDetail(List<BaseFeatureTypeDetail> baseFeatureTypeDetailList)
        {
            foreach (var item in baseFeatureTypeDetailList)
            {
                Add(item);
            }
        }
        public void AddBaseFeatureTypeDetail(BaseFeatureTypeDetail baseFeatureTypeDetail)
        {
            Add(baseFeatureTypeDetail);
        }

        public void UpdateBaseFeatureTypeDetail(BaseFeatureTypeDetail baseFeatureTypeDetail)
        {
            var oldBaseFeatureTypeDetail = (from s in Context.BaseFeatureTypeDetails.AsNoTracking() where s.Id == baseFeatureTypeDetail.Id select s).FirstOrDefault();
            oldBaseFeatureTypeDetail.Name = baseFeatureTypeDetail.Name;
            oldBaseFeatureTypeDetail.BaseFeatureTypeId = baseFeatureTypeDetail.BaseFeatureTypeId;

            Update(oldBaseFeatureTypeDetail);
        }

        public bool DeleteBaseFeatureTypeDetail(int id)
        {
            var deleteable = Context.BaseFeatureTypeDetails.Find(id);
            Delete(deleteable);
            return true;
        }

        public void BatchDeleteBaseFeatureTypeDetail(int[] ids)
        {
            var deletableBaseFeatureTypeDetail = (from s in Context.BaseFeatureTypeDetails.AsNoTracking() where ids.Contains(s.Id) select s).ToList();

            foreach (var item in deletableBaseFeatureTypeDetail)
            {
                Delete(item);
            }
        }
    }
}
