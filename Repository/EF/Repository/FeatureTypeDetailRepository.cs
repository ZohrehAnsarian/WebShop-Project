using Model;

using Repository.EF.Base;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class FeatureTypeDetailRepository : EFBaseRepository<FeatureTypeDetail>
    {
        public IEnumerable<FeatureTypeDetail> GetAll()
        {
            var featureTypeDetailList = from s in Context.FeatureTypeDetails.AsNoTracking()
                                        select s;

            return featureTypeDetailList.OrderBy(A => A.Priority).ToArray();
        }
        public int GetFeatureDetailTypeCountByFeaturType(int id)
        {
            return (from s in Context.FeatureTypeDetails.AsNoTracking()
                    where s.FeatureTypeId == id
                    select s).Count();

        }
        public void BatchAddFeatureTypeDetail(List<FeatureTypeDetail> featureTypeDetailList)
        {
            foreach (var item in featureTypeDetailList)
            {
                Add(item);
            }
        }
        public void AddFeatureTypeDetail(FeatureTypeDetail featureTypeDetail)
        {
            Add(featureTypeDetail);
        }

        public void UpdateFeatureTypeDetail(FeatureTypeDetail featureTypeDetail)
        {
            var oldFeatureTypeDetail = (from s in Context.FeatureTypeDetails.AsNoTracking() where s.Id == featureTypeDetail.Id select s).FirstOrDefault();
            oldFeatureTypeDetail.Priority = featureTypeDetail.Priority;
            oldFeatureTypeDetail.BaseFeatureTypeDetailId = featureTypeDetail.BaseFeatureTypeDetailId;
            oldFeatureTypeDetail.FeatureTypeId = featureTypeDetail.FeatureTypeId;

            Update(oldFeatureTypeDetail);
        }

        public bool DeleteFeatureTypeDetail(int id)
        {
            var deleteable = Context.FeatureTypeDetails.Find(id);

            if (deleteable != null)
            {
                Delete(deleteable);
            }

            return true;
        }

        public void BatchDeleteFeatureTypeDetail(int[] ids)
        {
            var deletableFeatureTypeDetail = (from s in Context.FeatureTypeDetails.AsNoTracking() where ids.Contains(s.Id) select s).ToList();

            foreach (var item in deletableFeatureTypeDetail)
            {
                Delete(item);
            }
        }

        public int GetFeatureTypeDetailCountByBaseFeatureTypeDetail(int baseFeatureTypeDetailId)
        {
            return Context.FeatureTypeDetails.AsNoTracking().Where(p => p.BaseFeatureTypeDetailId == baseFeatureTypeDetailId).Count();

        }
        public void DeleteFeatureTypeDetailByParentIds(int[] parentIds)
        {
            var result = (from d in Context.FeatureTypeDetails.AsNoTracking() where parentIds.Contains(d.FeatureTypeId) select d).ToList();
            foreach (var item in result)
            {
                Delete(item);
            }

        }
    }
}
