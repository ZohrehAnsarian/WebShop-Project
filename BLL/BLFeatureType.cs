using BLL.Base;
using Model;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Model.ViewModels.FeatureType;
using Newtonsoft.Json;
using static Model.ApplicationDomainModels.ConstantObjects;

namespace BLL
{
    public class BLFeatureType : BLBase
    {
        FeatureTypeRepository featureTypeRepository;
        BaseFeatureTypeRepository baseFeatureTypeRepository;
        FeatureTypeDetailRepository featureTypeDetailRepository;
        BLProductFeature blProductFeature;

        public BLFeatureType(int currentLanguageId) : base(currentLanguageId)
        {
            featureTypeRepository = UnitOfWork.GetRepository<FeatureTypeRepository>();
            baseFeatureTypeRepository = UnitOfWork.GetRepository<BaseFeatureTypeRepository>();
            featureTypeDetailRepository = UnitOfWork.GetRepository<FeatureTypeDetailRepository>();
            blProductFeature = new BLProductFeature(currentLanguageId);
        }
        public IEnumerable<VmFeatureType> GetAll(string searchText = "")
        {
            var featureTypeList = featureTypeRepository.GetAll(searchText);

            return (from f in featureTypeList
                    select new VmFeatureType
                    {
                        Id = f.Id,
                        Name = f.Name,
                        Priority = f.Priority
                    }).OrderBy(d => d.Priority).ToArray();
        }
        public IEnumerable<VmFeatureType> GetFeatureTypeWithDetailsByCategory(int categoryId)
        {

            var featureTypeList = featureTypeRepository.GetFeatureTypeWithDetails(categoryId);

            var maxPriority = featureTypeList.Max(d => d.Priority);
            var maxDetailPriority = featureTypeList.Last().FeatureTypeDetailList.Max(d => d.Priority);

            var vmFeatureTypeList = (from f in featureTypeList
                                     select new VmFeatureType
                                     {

                                         Id = f.Id,
                                         Name = f.Name,
                                         Priority = f.Priority,
                                         FeatureTypeDetailList =
                                         (from d in f.FeatureTypeDetailList
                                          select new VmFeatureTypeDetail
                                          {
                                              Id = d.Id,
                                              Name = d.Name,
                                              FeatureTypeId = d.FeatureTypeId,
                                              Priority = d.Priority,
                                              IsLeaf = (f.Priority == maxPriority && d.Priority == maxDetailPriority),

                                          }).OrderBy(d => d.Priority).ToList()
                                     }).OrderBy(d => d.Priority).ToList();

            foreach (var item in vmFeatureTypeList.Last().FeatureTypeDetailList)
            {
                item.IsLeaf = true;
            }

            return vmFeatureTypeList;
        }
        public IEnumerable<VmFeatureType> GetAssignFeatureTypeWithDetailsByCategory(int categoryId)
        {
            var oldFeatureTypeList = featureTypeRepository.GetFeatureTypeWithDetails(categoryId);
            var baseFeatureTypeList = baseFeatureTypeRepository.GetBaseFeatureTypeWithDetails();

            var vmFeatureTypeList = (from f in baseFeatureTypeList

                                     let assignedFeatureType = oldFeatureTypeList.FirstOrDefault(b => b.BaseFeatureTypeId == f.Id)

                                     select new VmFeatureType
                                     {
                                         Id = (assignedFeatureType != null) ? assignedFeatureType.Id : f.Id,
                                         Name = f.Name,
                                         Priority = (assignedFeatureType != null) ? assignedFeatureType.Priority : int.MaxValue,
                                         Checked = (assignedFeatureType == null) ? "" : "checked",
                                         BaseFeatureTypeId = assignedFeatureType?.BaseFeatureTypeId,

                                         FeatureTypeDetailList =
                                         (from d in f.BaseFeatureTypeDetails

                                          let assignedFeatureTypeDetail = assignedFeatureType?.FeatureTypeDetailList
                                            .FirstOrDefault(b => b.BaseFeatureTypeDetailId == d.Id)

                                          select new VmFeatureTypeDetail
                                          {
                                              Id = (assignedFeatureTypeDetail != null) ? assignedFeatureTypeDetail.Id : d.Id,
                                              Name = d.Name,
                                              FeatureTypeId = (assignedFeatureTypeDetail != null) ? assignedFeatureTypeDetail.FeatureTypeId : d.BaseFeatureTypeId,
                                              Priority = (assignedFeatureTypeDetail != null) ? assignedFeatureTypeDetail.Priority : int.MaxValue,
                                              Checked = (assignedFeatureTypeDetail == null) ? "" : "checked",
                                              BaseFeatureTypeDetailId = assignedFeatureTypeDetail?.BaseFeatureTypeDetailId,

                                          }).OrderBy(d => d.Priority).ToList()
                                     }).OrderBy(d => d.Priority).ToList();

            return vmFeatureTypeList;
        }
        public IEnumerable<VmFeatureType> GetAssignedFeatureTypeByCategoryId(int categoryId)
        {
            var assignedFeatureTypeList = featureTypeRepository.GetAssignedFeatureTypeByCategoryId(categoryId);
            var vmFeatureTypeList = (from f in assignedFeatureTypeList
                                     select new VmFeatureType
                                     {

                                         Id = f.Id,
                                         Priority = f.Priority,
                                         BaseFeatureTypeId = f.BaseFeatureTypeId,
                                         FeatureTypeDetailList =
                                         (from d in f.FeatureTypeDetails
                                          select new VmFeatureTypeDetail
                                          {
                                              Id = d.Id,
                                              BaseFeatureTypeDetailId = d.BaseFeatureTypeDetailId,
                                              FeatureTypeId = d.FeatureTypeId,
                                              Priority = d.Priority,


                                          }).OrderBy(d => d.Priority).ToList()
                                     }).OrderBy(d => d.Priority).ToList();

            return vmFeatureTypeList;
        }
        public IEnumerable<VmCategoryFeatureType> GetFeatureTypeWithDetailsByAllCategory()
        {
            var categoryFeatureTypeList = featureTypeRepository.GetFeatureTypeWithDetailsByAllCategory();

            var vmCategoryFeatureTypeList =
                    (from c in categoryFeatureTypeList
                     group c by c.CategoryId into g
                     select new VmCategoryFeatureType
                     {
                         CategoryId = g.Key,
                         FeatureTypeList =
                            (from f in g.ToList()

                             let maxPriority = g.ToList().Max(d => d.Priority)

                             select new VmFeatureType
                             {
                                 Id = f.Id,
                                 Name = f.Name,
                                 Priority = f.Priority,
                                 BaseFeatureTypeId = f.BaseFeatureTypeId,
                                 FeatureTypeDetailList =
                                 (from d in f.FeatureTypeDetailList

                                  let maxDetailPriority = f.FeatureTypeDetailList.Max(fd => fd.Priority)

                                  select new VmFeatureTypeDetail
                                  {
                                      Id = d.Id,
                                      Name = d.Name,
                                      FeatureTypeId = d.FeatureTypeId,
                                      BaseFeatureTypeDetailId = d.BaseFeatureTypeDetailId,
                                      Priority = d.Priority,
                                      IsLeaf = (f.Priority == maxPriority && d.Priority == maxDetailPriority),

                                  }).OrderBy(d => d.Priority).ToList(),
                             }).OrderBy(d => d.Priority).ToList(),

                         FirstIconPriority = g.ToList().OrderByDescending(f => f.Priority).First().FeatureTypeDetailList.Min(d => d.Priority),

                     }).ToList();

            return vmCategoryFeatureTypeList;
        }

        public VmFeatureType GetFeatureTypeWithDetails(int id)
        {
            var featureTypeWithDetail = featureTypeRepository.GetFeatureTypeWithDetails(id).First();

            var featureTypeDetailIds = featureTypeWithDetail.FeatureTypeDetailList.Select(d => d.Id).ToArray();

            featureTypeWithDetail.FeatureTypeDetailIds = string.Join(",", featureTypeDetailIds);

            featureTypeWithDetail.FeatureTypeDetailPriorities =
                string.Join(",", featureTypeWithDetail.FeatureTypeDetailList.Select(d => d.Priority));

            featureTypeWithDetail.FeatureTypeDetailNames =
                string.Join(",", featureTypeWithDetail.FeatureTypeDetailList.Select(d => d.Name));


            List<int?> featureTypeDetailIdList = new List<int?>();
            foreach (var item in featureTypeDetailIds)
            {
                featureTypeDetailIdList.Add(item);
            }

            var usedFeatureTypeDetailIds = blProductFeature.GetFeatureTypeDetailIds(featureTypeDetailIdList);

            foreach (var item in featureTypeWithDetail.FeatureTypeDetailList)
            {
                if (usedFeatureTypeDetailIds.Contains(item.Id) == true)
                {
                    item.Deletable = false;
                }
                else
                {
                    item.Deletable = true;
                }

            }

            var Deletable = featureTypeWithDetail.FeatureTypeDetailList.Select(d => d.Deletable).ToArray();

            featureTypeWithDetail.FeatureTypeDetailDeletable = JsonConvert.SerializeObject(Deletable);
            return featureTypeWithDetail;

        }

        public IEnumerable<int> GetFeatureTypeDetailIds(List<int> baseFeatureTypeDetailIdList)
        {
            return featureTypeRepository.GetFeatureTypeDetailIds(baseFeatureTypeDetailIdList);
        }

        public IEnumerable<VmFeatureType> GetFeatureTypeWithDetails(IEnumerable<int> ids)
        {
            var vmFeatureTypeList = featureTypeRepository.GetFeatureTypeWithDetails(ids);

            return vmFeatureTypeList;

        }

        public IEnumerable<VmFeatureType> GetFeatureType(int id)
        {
            var featureTypeList = featureTypeRepository.GetFeatureType(id);

            return (from f in featureTypeList
                    select new VmFeatureType
                    {
                        Id = f.Id,
                        Name = f.Name,
                        Priority = f.Priority,

                    }).ToArray();
        }
        public int GetFirstIconPriority()
        {
            return featureTypeRepository.GetFirstIconPriority();


        }

        public static void UpdateStaticFeatureTypes(int CurrentLanguageId)
        {
            var blFeatureType = new BLFeatureType(CurrentLanguageId);
            StaticCategoryFeatureTypeList = blFeatureType.GetFeatureTypeWithDetailsByAllCategory().ToList();

        }
        public bool UpdateAssignFeatureTypeWithDetailsByCategory(int categoryId, List<VmFeatureType> assignedFeatureTypeList)
        {
            var oldFeatureTypeList = featureTypeRepository.GetFeatureTypeWithDetails(categoryId);

            foreach (var af in assignedFeatureTypeList)
            {
                var oldFeatureTypeResult = oldFeatureTypeList.FirstOrDefault(f => f.Id == af.Id);

                if (oldFeatureTypeResult == null && af.Checked == "checked")
                {
                    #region Add new feature Type with details

                    featureTypeRepository.AddFeatureType(
                        new FeatureType
                        {
                            BaseFeatureTypeId = af.Id,
                            Priority = af.Priority,
                            CategoryId = categoryId,
                            FeatureTypeDetails = (from d in af.FeatureTypeDetailList
                                                  where d.Checked == "checked"
                                                  select new FeatureTypeDetail
                                                  {
                                                      BaseFeatureTypeDetailId = d.Id,
                                                      Priority = d.Priority,

                                                  }).ToList(),
                        });

                    #endregion Add new feature Type with details
                }
                else
                if (oldFeatureTypeResult != null && af.Checked == "")
                {
                    #region Delete/Update feature type 

                    var deleted = 0;

                    foreach (var afd in af.FeatureTypeDetailList)
                    {
                        #region Delete feature type detail

                        var blProductFeature = new BLProductFeature(CurrentLanguageId);

                        if (blProductFeature.GetNoneEmptyProductFeatureCountByFeatureTypeDetail(afd.Id) == 0)
                        {
                            featureTypeDetailRepository.DeleteFeatureTypeDetail(afd.Id);
                            deleted++;
                        }

                        #endregion Delete feature type detail
                    }

                    if (deleted == af.FeatureTypeDetailList.Count)
                    {
                        featureTypeRepository.DeleteFeatureType(af.Id);
                    }
                    else
                    {
                        if (af.Priority != oldFeatureTypeResult.Priority)
                        {
                            featureTypeRepository.UpdateFeatureType(new FeatureType
                            {

                                Id = af.Id,
                                BaseFeatureTypeId = af.BaseFeatureTypeId.Value,
                                Priority = af.Priority,
                                CategoryId = af.CategoryId
                            });
                        }
                    }

                    #endregion Delete/Update feature type
                }
                else
                if (oldFeatureTypeResult != null && af.Checked == "checked")
                {
                    #region Update feature type Add/Delete details

                    if (af.Priority != oldFeatureTypeResult.Priority)
                    {
                        featureTypeRepository.UpdateFeatureType(new FeatureType
                        {
                            Id = af.Id,
                            BaseFeatureTypeId = af.BaseFeatureTypeId.Value,
                            Priority = af.Priority,
                            CategoryId = af.CategoryId
                        });
                    }
                    var deleted = 0;
                    foreach (var afd in af.FeatureTypeDetailList)
                    {
                        var featureTypeDetailResult = oldFeatureTypeResult.FeatureTypeDetailList.FirstOrDefault(d => d.Id == afd.Id);

                        if (featureTypeDetailResult == null && afd.Checked == "checked")
                        {
                            #region Add feature type detail

                            featureTypeDetailRepository.AddFeatureTypeDetail(
                                new FeatureTypeDetail
                                {
                                    FeatureTypeId = afd.FeatureTypeId,
                                    BaseFeatureTypeDetailId = afd.Id,
                                    Priority = afd.Priority,
                                });

                            #endregion Add feature type detail
                        }
                        else
                        if (featureTypeDetailResult != null && afd.Checked == "")
                        {
                            #region Delete feature type detail

                            var blProductFeature = new BLProductFeature(CurrentLanguageId);

                            if (blProductFeature.GetNoneEmptyProductFeatureCountByFeatureTypeDetail(afd.Id) == 0)
                            {
                                featureTypeDetailRepository.DeleteFeatureTypeDetail(afd.Id);
                                deleted++;
                            }

                            #endregion Delete feature type detail 
                        }
                        else
                        if (featureTypeDetailResult != null && afd.Checked == "checked")
                        {
                            #region Update feature type detail

                            if (afd.Priority != featureTypeDetailResult.Priority)
                            {
                                featureTypeDetailRepository.UpdateFeatureTypeDetail(new FeatureTypeDetail
                                {
                                    Id = afd.Id,
                                    Priority = afd.Priority,
                                    FeatureTypeId = afd.FeatureTypeId,
                                    BaseFeatureTypeDetailId = afd.BaseFeatureTypeDetailId.Value,
                                });
                            }

                            #endregion Update feature type detail 
                        }
                    }

                    if (deleted == af.FeatureTypeDetailList.Count)
                    {
                        featureTypeRepository.DeleteFeatureType(af.Id);
                    }

                    #endregion Update/Add/Delete details
                }

            }

            var result = UnitOfWork.Commit();

            if (result == true)
            {
                UpdateStaticFeatureTypes(CurrentLanguageId);
            }

            return result;
        }

        public int GetFeatureTypeDetailCountByBaseFeatureTypeDetail(int baseFeatureTypeDetailId)
        {
            return featureTypeDetailRepository.GetFeatureTypeDetailCountByBaseFeatureTypeDetail(baseFeatureTypeDetailId);

        }
        public int GetFeatureTypeCountByCategory(int categoryId)
        {
            return featureTypeRepository.GetFeatureTypeCountByCategory(categoryId);
        }

        public bool DeleteFeatureType(int id)
        {
            try
            {
                if (featureTypeDetailRepository.GetFeatureDetailTypeCountByFeaturType(id) == 0)
                {
                    featureTypeRepository.DeleteFeatureType(id);

                    var result = UnitOfWork.Commit();

                    if (result == true)
                    {
                        UpdateStaticFeatureTypes(CurrentLanguageId);
                    }

                    return result;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool DeleteFeatureTypeDetail(int id)
        {

            try
            {
                var blProductFeature = new BLProductFeature(CurrentLanguageId);

                if (blProductFeature.GetProductFeatureCountByFeatureTypeDetail(id) == 0)
                {
                    featureTypeDetailRepository.DeleteFeatureTypeDetail(id);

                    var result = UnitOfWork.Commit();

                    if (result == true)
                    {
                        UpdateStaticFeatureTypes(CurrentLanguageId);
                    }

                    return result;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}