using Model;
using Model.ViewModels.FeatureType;

using Repository.EF.Base;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repository.EF.Repository
{
    public class FeatureTypeRepository : EFBaseRepository<FeatureType>
    {
        public IEnumerable<ViewFeatureType> GetAll(string searchText = "")
        {
            var featureTypeList = from s in Context.ViewFeatureTypes
                                  where (
                                    s.Name.Contains(searchText)
                                 || s.Priority.ToString().Contains(searchText)
                                 )
                                  select s;

            return featureTypeList.OrderBy(A => A.Priority).ToArray();
        }
        public IEnumerable<ViewFeatureType> GetFeatureType(int id)
        {
            var featureTypeList = from s in Context.ViewFeatureTypes
                                  where s.Id == id
                                  select s;

            return featureTypeList.OrderBy(A => A.Priority).ToArray();
        }
        public IEnumerable<FeatureType> GetAssignedFeatureTypeByCategoryId(int categoryId)
        {
            var featureTypeList = from s in Context.FeatureTypes
                                  where s.CategoryId == categoryId
                                  select s;

            return featureTypeList.OrderBy(A => A.Priority).ToArray();
        }

        public int GetFeatureTypeCountByCategory(int categoryId)
        {
            return Context.ViewFeatureTypes.Count(f => f.CategoryId == categoryId);
        }

        public IEnumerable<VmFeatureType> GetFeatureTypeWithDetails(int categoryId)
        {
            var featureType = (from f in Context.ViewFeatureTypes.AsNoTracking()
                               where f.CategoryId == categoryId
                               join d in Context.ViewFeatureTypeDetails.AsNoTracking() on f.Id equals d.FeatureTypeId
                               group d by f into g
                               select new VmFeatureType
                               {
                                   Id = g.Key.Id,
                                   Name = g.Key.Name,
                                   Priority = g.Key.Priority,
                                   CategoryId = g.Key.CategoryId,
                                   BaseFeatureTypeId = g.Key.BaseFeatureTypeId,
                                   FeatureTypeDetailList = (from fd in g.ToList()
                                                            select new VmFeatureTypeDetail
                                                            {
                                                                Id = fd.Id,
                                                                FeatureTypeId = fd.FeatureTypeId,
                                                                Priority = fd.Priority,
                                                                Name = fd.Name,
                                                                BaseFeatureTypeDetailId = fd.BaseFeatureTypeDetailId,

                                                            }).OrderBy(d => d.Priority).ToList(),
                               }).OrderBy(f => f.Priority).ToArray();

            return featureType;
        }
        public IEnumerable<VmFeatureType> GetFeatureTypeWithDetailsByAllCategory()
        {
            var featureType = (from f in Context.ViewFeatureTypes
                               join d in Context.ViewFeatureTypeDetails on f.Id equals d.FeatureTypeId
                               group d by f into g
                               select new VmFeatureType
                               {
                                   Id = g.Key.Id,
                                   Name = g.Key.Name,
                                   Priority = g.Key.Priority,
                                   CategoryId = g.Key.CategoryId,
                                   BaseFeatureTypeId = g.Key.BaseFeatureTypeId,
                                   FeatureTypeDetailList = (from fd in g.ToList()
                                                            select new VmFeatureTypeDetail
                                                            {
                                                                Id = fd.Id,
                                                                FeatureTypeId = fd.FeatureTypeId,
                                                                Priority = fd.Priority,
                                                                Name = fd.Name,
                                                                BaseFeatureTypeDetailId = fd.BaseFeatureTypeDetailId,

                                                            }).OrderBy(d => d.Priority).ToList(),
                               });

            return featureType.OrderBy(f => f.Priority);
        }

        public IEnumerable<int> GetFeatureTypeDetailIds(IEnumerable<int> baseFeatureTypeDetailIds)
        {
            return Context.FeatureTypeDetails.Where(p => baseFeatureTypeDetailIds.Contains(p.BaseFeatureTypeDetailId))
                .Select(d => d.Id).Distinct().ToArray();
        }

        public IEnumerable<VmFeatureType> GetFeatureTypeWithDetails(IEnumerable<int> ids)
        {

            var featureType = (from f in Context.ViewFeatureTypes
                               where ids.Contains(f.Id)
                               join d in Context.ViewFeatureTypeDetails on f.Id equals d.FeatureTypeId
                               group d by f into g
                               select new VmFeatureType
                               {
                                   Id = g.Key.Id,
                                   Name = g.Key.Name,
                                   Priority = g.Key.Priority,
                                   CategoryId = g.Key.CategoryId,
                                   FeatureTypeDetailList = (from fd in g.ToList()
                                                            select new VmFeatureTypeDetail
                                                            {
                                                                Id = fd.Id,
                                                                FeatureTypeId = fd.FeatureTypeId,
                                                                Priority = fd.Priority,
                                                                Name = fd.Name,
                                                            }).OrderBy(fd => fd.Priority).ToList(),
                               }).ToList();

            return featureType.OrderBy(f => f.Priority);
        }
        public void AddFeatureType(FeatureType featureType)
        {
            Add(featureType);
        }

        public void UpdateFeatureType(FeatureType featureType)
        {
            var oldFeatureType = (from s in Context.FeatureTypes where s.Id == featureType.Id select s).AsNoTracking().FirstOrDefault();
            oldFeatureType.Priority = featureType.Priority;
            oldFeatureType.BaseFeatureTypeId = featureType.BaseFeatureTypeId;
            oldFeatureType.CategoryId = featureType.CategoryId;

            Update(featureType);
        }

        public bool DeleteFeatureType(int id)
        {
            var deleteable = Context.FeatureTypes.Find(id);

            if (deleteable != null)
            {
                Delete(deleteable);
            }

            return true;
        }

        public int GetFirstIconPriority()
        {
            return Context.FeatureTypes.Include(f => f.FeatureTypeDetails).OrderByDescending(f => f.Priority).First().FeatureTypeDetails.Min(d => d.Priority);
        }

        public void DeleteFeatureTypeByIds(int[] ids)
        {
            var result = (from d in Context.FeatureTypes.AsNoTracking() where ids.Contains(d.Id) select d).ToList();
            foreach (var item in result)
            {
                Delete(item);
            }
        }
    }
}
