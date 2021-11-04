using BLL.Base;
using Model;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Model.ViewModels.BaseFeatureType;
using Newtonsoft.Json;
using static Model.ApplicationDomainModels.ConstantObjects;
using Model.ApplicationDomainModels;

namespace BLL
{
    public class BLBaseFeatureType : BLBase
    {
        BaseFeatureTypeRepository baseFeatureTypeRepository;
        FeatureTypeDetailRepository featureTypeDetailRepository;
        BaseFeatureTypeDetailRepository baseFeatureTypeDetailRepository;
        BLProductFeature blProductFeature;

        public BLBaseFeatureType(int currentLanguageId) : base(currentLanguageId)
        {
            baseFeatureTypeRepository = UnitOfWork.GetRepository<BaseFeatureTypeRepository>();
            baseFeatureTypeDetailRepository = UnitOfWork.GetRepository<BaseFeatureTypeDetailRepository>();
            featureTypeDetailRepository = UnitOfWork.GetRepository<FeatureTypeDetailRepository>();
            blProductFeature = new BLProductFeature(currentLanguageId);
        }
        public IEnumerable<VmBaseFeatureType> GetAll(string searchText = "")
        {
            var baseFeatureTypeList = baseFeatureTypeRepository.GetAll(searchText);

            return (from f in baseFeatureTypeList
                    select new VmBaseFeatureType
                    {
                        Id = f.Id,
                        Name = f.Name,
                    }).OrderBy(d => d.Name).ToArray();
        }
        public IEnumerable<VmBaseFeatureTypeDetail> GetAllDetail()
        {
            var baseFeatureTypeDeatlList = baseFeatureTypeDetailRepository.GetAll();

            return (from f in baseFeatureTypeDeatlList
                    select new VmBaseFeatureTypeDetail
                    {
                        Id = f.Id,
                        BaseFeatureTypeId = f.BaseFeatureTypeId,
                        Name = f.Name,

                    }).ToArray();
        }
        public IEnumerable<VmBaseFeatureType> GetBaseFeatureTypeWithDetails()
        {
            var baseFeatureTypeList = baseFeatureTypeRepository.GetBaseFeatureTypeWithDetails();

            var vmBaseFeatureTypeList = (from f in baseFeatureTypeList
                                         select new VmBaseFeatureType
                                         {

                                             Id = f.Id,
                                             Name = f.Name,
                                             BaseFeatureTypeDetailList =
                                             (from d in f.BaseFeatureTypeDetails
                                              select new VmBaseFeatureTypeDetail
                                              {
                                                  Id = d.Id,
                                                  Name = d.Name,
                                                  BaseFeatureTypeId = d.BaseFeatureTypeId,

                                              }).OrderBy(d => d.Name).ToList()
                                         }).OrderBy(d => d.Name).ToList();

            return vmBaseFeatureTypeList;
        }

        public VmBaseFeatureType GetBaseFeatureTypeWithDetails(int id)
        {
            var baseFeatureType = baseFeatureTypeRepository.GetBaseFeatureTypeWithDetails(id);

            var baseFeatureTypeWithDetail = new VmBaseFeatureType
            {

                Id = baseFeatureType.Id,
                Name = baseFeatureType.Name,
                BaseFeatureTypeDetailList = (from d in baseFeatureType.BaseFeatureTypeDetails
                                             select new VmBaseFeatureTypeDetail
                                             {
                                                 Id = d.Id,
                                                 Name = d.Name,
                                                 BaseFeatureTypeId = d.BaseFeatureTypeId,
                                             }).OrderBy(d => d.Name).ToList()
            };

            var baseFeatureTypeDetailIds = baseFeatureTypeWithDetail.BaseFeatureTypeDetailList.Select(d => d.Id).ToArray();

            baseFeatureTypeWithDetail.BaseFeatureTypeDetailIds = string.Join(",", baseFeatureTypeDetailIds);

            baseFeatureTypeWithDetail.BaseFeatureTypeDetailNames =
                string.Join(",", baseFeatureTypeWithDetail.BaseFeatureTypeDetailList.Select(d => d.Name));


            var baseFeatureTypeDetailIdList = new List<int>();
            foreach (var item in baseFeatureTypeDetailIds)
            {
                baseFeatureTypeDetailIdList.Add(item);
            }

            var blFeatureType = new BLFeatureType(CurrentLanguageId);
            var usedBaseFeatureTypeDetailIds = blFeatureType.GetFeatureTypeDetailIds(baseFeatureTypeDetailIdList);

            foreach (var item in baseFeatureTypeWithDetail.BaseFeatureTypeDetailList)
            {
                if (usedBaseFeatureTypeDetailIds.Contains(item.Id) == true)
                {
                    item.Deletable = false;
                }
                else
                {
                    item.Deletable = true;
                }

            }

            var Deletable = baseFeatureTypeWithDetail.BaseFeatureTypeDetailList.Select(d => d.Deletable).ToArray();

            baseFeatureTypeWithDetail.BaseFeatureTypeDetailDeletable = JsonConvert.SerializeObject(Deletable);
            return baseFeatureTypeWithDetail;

        }

        public IEnumerable<VmBaseFeatureType> GetBaseFeatureTypeWithDetails(IEnumerable<int> ids)
        {
            var baseFeatureTypeList = baseFeatureTypeRepository.GetBaseFeatureTypeWithDetails(ids);

            var vmBaseFeatureTypeList = (from baseFeatureType in baseFeatureTypeList
                                         select new VmBaseFeatureType
                                         {

                                             Id = baseFeatureType.Id,
                                             Name = baseFeatureType.Name,
                                             BaseFeatureTypeDetailList = (from d in baseFeatureType.BaseFeatureTypeDetails
                                                                          select new VmBaseFeatureTypeDetail
                                                                          {
                                                                              Id = d.Id,
                                                                              Name = d.Name,
                                                                              BaseFeatureTypeId = d.BaseFeatureTypeId,
                                                                          }).OrderBy(d => d.Name).ToList()
                                         }).OrderBy(f => f.Name);


            return vmBaseFeatureTypeList;

        }

        public IEnumerable<VmBaseFeatureType> GetBaseFeatureType(int id)
        {
            var baseFeatureTypeList = baseFeatureTypeRepository.GetBaseFeatureType(id);

            return (from f in baseFeatureTypeList
                    select new VmBaseFeatureType
                    {
                        Id = f.Id,
                        Name = f.Name,

                    }).ToArray();
        }

        public bool AddBaseFeatureType(VmBaseFeatureType baseFeatureType)
        {
            try
            {
                var baseFeatureTypeDetailNames = baseFeatureType.BaseFeatureTypeDetailNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var newBaseFeatureType = new BaseFeatureType()
                {
                    Name = baseFeatureType.Name,
                };

                newBaseFeatureType.BaseFeatureTypeDetails = new List<BaseFeatureTypeDetail>();

                for (var i = 0; i < baseFeatureTypeDetailNames.Length; i++)
                {
                    newBaseFeatureType.BaseFeatureTypeDetails.Add(
                        new BaseFeatureTypeDetail
                        {
                            Name = baseFeatureTypeDetailNames[i],
                        });
                }

                baseFeatureTypeRepository.AddBaseFeatureType(newBaseFeatureType);

                var result = UnitOfWork.Commit();

                if (result == true)
                {
                    BLFeatureType.UpdateStaticFeatureTypes(CurrentLanguageId);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool EditBaseFeatureType(VmBaseFeatureType baseFeatureType)
        {
            try
            {
                var blBaseFeatureType = new BLBaseFeatureType(CurrentLanguageId);

                var clientBaseFeatureTypeDetail = JsonConvert.DeserializeObject<List<VmBaseFeatureTypeDetail>>(baseFeatureType.JSONBaseFeatureTypeDetail);

                var updatableBaseFeatureType = blBaseFeatureType.GetBaseFeatureTypeWithDetails(baseFeatureType.Id);

                updatableBaseFeatureType.Name = baseFeatureType.Name;
                baseFeatureTypeRepository.UpdateBaseFeatureType(new BaseFeatureType
                {
                    Id = updatableBaseFeatureType.Id,
                    Name = updatableBaseFeatureType.Name,
                });

                var editableList = (from a in clientBaseFeatureTypeDetail
                                    where a.RowState == "edited"
                                    select new BaseFeatureTypeDetail
                                    {
                                        Id = a.Id,
                                        BaseFeatureTypeId = baseFeatureType.Id,
                                        Name = a.Name,
                                    }).ToList();

                foreach (var item in editableList)
                {
                    var edited = updatableBaseFeatureType.BaseFeatureTypeDetailList.First(d => d.Id == item.Id);
                    edited.Name = item.Name;
                    baseFeatureTypeDetailRepository.UpdateBaseFeatureTypeDetail(new BaseFeatureTypeDetail
                    {
                        Id = edited.Id,
                        Name = edited.Name,
                        BaseFeatureTypeId = edited.BaseFeatureTypeId,
                    });
                }

                // baseFeatureTypeRepository.UpdateBaseFeatureType(updatableBaseFeatureType);

                var addedList = (from a in clientBaseFeatureTypeDetail
                                 where a.RowState == "added"
                                 select new BaseFeatureTypeDetail
                                 {
                                     BaseFeatureTypeId = baseFeatureType.Id,
                                     Name = a.Name,
                                 }).ToList();


                baseFeatureTypeDetailRepository.BatchAddBaseFeatureTypeDetail(addedList);


                foreach (var category in StaticCategoryList)
                {
                    var blFeatureType = new BLFeatureType(CurrentLanguageId);
                    var categoryFeatureList = blFeatureType.GetAssignedFeatureTypeByCategoryId(category.Id);
                    var nextPriority = categoryFeatureList.Max(f => f.Priority) + 1;

                    foreach (var item in addedList)
                    {
                        var featureTypeId = categoryFeatureList.First(f => f.BaseFeatureTypeId == updatableBaseFeatureType.Id).Id;

                        featureTypeDetailRepository.AddFeatureTypeDetail(
                            new FeatureTypeDetail
                            {
                                FeatureTypeId = featureTypeId,
                                BaseFeatureTypeDetailId = item.Id,
                                Priority = nextPriority,
                            }
                        );

                        nextPriority++;
                    }

                }


                var deletedIds = clientBaseFeatureTypeDetail.Where(d => d.RowState == "deleted").Select(d => d.Id);
                baseFeatureTypeDetailRepository.BatchDeleteBaseFeatureTypeDetail(deletedIds.ToArray());

                var result = UnitOfWork.Commit();
                var detailsCount = baseFeatureTypeDetailRepository.GetFeatureDetailTypeCountByFeaturType(baseFeatureType.Id);
                if (detailsCount == 0)
                {
                    DeleteBaseFeatureType(baseFeatureType.Id);
                }
                if (result == true)
                {
                    BLFeatureType.UpdateStaticFeatureTypes(CurrentLanguageId);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DeleteBaseFeatureType(int id)
        {
            try
            {
                if (baseFeatureTypeDetailRepository.GetFeatureDetailTypeCountByFeaturType(id) == 0)
                {
                    baseFeatureTypeRepository.DeleteBaseFeatureType(id);

                    var result = UnitOfWork.Commit();

                    if (result == true)
                    {
                        BLFeatureType.UpdateStaticFeatureTypes(CurrentLanguageId);
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
        public bool DeleteBaseFeatureTypeDetail(int id)
        {

            try
            {
                var blFeatureType = new BLFeatureType(CurrentLanguageId);

                if (blFeatureType.GetFeatureTypeDetailCountByBaseFeatureTypeDetail(id) == 0)
                {
                    baseFeatureTypeDetailRepository.DeleteBaseFeatureTypeDetail(id);

                    var result = UnitOfWork.Commit();

                    if (result == true)
                    {
                        BLFeatureType.UpdateStaticFeatureTypes(CurrentLanguageId);
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