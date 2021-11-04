using BLL.Base;
using BLL.SystemTools;

using BusinessLayer;

using CyberneticCode.Web.Mvc.Helpers;

using Model;
using Model.ApplicationDomainModels;
using Model.UIControls.Tree;
using Model.ViewModels;
using Model.ViewModels.FeatureType;
using Model.ViewModels.Product;
using Model.ViewModels.ProductFeature;
using Model.ViewModels.ProductImage;
using Model.ViewModels.SundryImage;

using Newtonsoft.Json;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace BLL
{
    public class BLProductFeature : BLBase
    {
        ProductFeatureRepository productFeatureRepository;
        FeatureTypeRepository featureTypeRepository;
        ViewProductFeatureRepository viewProductFeatureRepository;
        ViewProductFeatureFullInfoRepository viewProductFeatureFullInfoRepository;
        ViewShopProductRepository viewShopProductRepository;
        SpHandlerRepository spHandlerRepository;
        ImageRepository imageRepository;


        public BLProductFeature(int currentLanguageId) : base(currentLanguageId)
        {
            featureTypeRepository = UnitOfWork.GetRepository<FeatureTypeRepository>();
            productFeatureRepository = UnitOfWork.GetRepository<ProductFeatureRepository>();
            viewProductFeatureRepository = UnitOfWork.GetRepository<ViewProductFeatureRepository>();
            viewProductFeatureFullInfoRepository = UnitOfWork.GetRepository<ViewProductFeatureFullInfoRepository>();
            viewShopProductRepository = UnitOfWork.GetRepository<ViewShopProductRepository>();
            spHandlerRepository = UnitOfWork.GetRepository<SpHandlerRepository>();
            imageRepository = UnitOfWork.GetRepository<ImageRepository>();
        }

        public int GetProductFeatureCountByFeatureTypeDetail(int id)
        {
            return productFeatureRepository.GetProductFeatureCountByFeatureTypeDetail(id);
        }

        public int GetNoneEmptyProductFeatureCountByFeatureTypeDetail(int id)
        {
            return productFeatureRepository.GetNoneEmptyProductFeatureCountByFeatureTypeDetail(id);
        }

        public IEnumerable<int?> GetFeatureTypeDetailIds(IEnumerable<int?> featureTypeDetailIds)
        {
            return productFeatureRepository.GetFeatureTypeDetailIds(featureTypeDetailIds);
        }

        public TreeNode GetProductFeaturesByProduct(string parentId, int productId)
        {
            var productFeatureList = viewProductFeatureRepository.GetViewProductFeaturesByProduct(productId);
            var vmProductFeatureList = (from p in productFeatureList
                                        select new VmProductFeatureTree
                                        {
                                            Id = p.Id,
                                            ParentId = p.ParentId.Value,
                                            ProductId = p.ProductId,
                                            FeatureTypeId = p.FeatureTypeId,
                                            FeatureTypeDetailId = p.FeatureTypeDetailId,
                                            Price = p.Price,
                                            Quantity = p.Quantity,
                                            Name = p.FeatureTypeDetailName,
                                            AdditionalData = "",
                                            Priority = p.Priority,
                                            FeatureTypePriority = p.FeatureTypePriority,
                                            FeatureTypeDetailPriority = p.FeatureTypeDetailPriority,
                                            IconUrl = p.IconUrl,
                                            Showcase = p.Showcase,
                                            Description = p.Description,

                                        }).OrderBy(p => p.Priority).ToList();

            vmProductFeatureList.First().Name = new BLProduct(CurrentLanguageId).GetProductById(productId).Name;

            foreach (var item in vmProductFeatureList)
            {
                if (item.FeatureTypeDetailId != null)
                {
                    var productFeature = new VmProductFeature
                    {
                        Id = item.Id,
                        ParentId = item.ParentId.Value,
                        ProductId = item.ProductId,
                        FeatureTypeDetailId = item.FeatureTypeDetailId.Value,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        IconUrl = item.IconUrl,
                        Showcase = item.Showcase,
                        Description = item.Description?.Replace("\'", "\\'").Replace("\r", "").Replace("\n", "<br/>").Replace("\"", "\\\"")

                    };

                    item.AdditionalData = JsonConvert.SerializeObject(productFeature).Replace("\'", "\\'");
                }
            }

            BLTreeModelTools blTreeModelTools = new BLTreeModelTools();

            if (vmProductFeatureList != null && vmProductFeatureList.Count() > 0)
            {
                var tree = blTreeModelTools.GetTreeModel(vmProductFeatureList, parentId);
                return tree;
            }
            return null;
        }
        public bool IsProductFeaturesTreeChanged(int productId)
        {
            var productFeatureList = viewProductFeatureRepository.GetViewProductFeaturesByProduct(productId);
            var vmProductFeatureList = (from p in productFeatureList
                                        select new VmProductFeatureTree
                                        {
                                            Id = p.Id,
                                            ParentId = p.ParentId.Value,
                                            ProductId = p.ProductId,
                                            FeatureTypeId = p.FeatureTypeId,
                                            FeatureTypeDetailId = p.FeatureTypeDetailId,
                                            Price = p.Price,
                                            Quantity = p.Quantity,
                                            Name = p.FeatureTypeDetailName,
                                            AdditionalData = "",
                                            Priority = p.Priority,
                                            FeatureTypePriority = p.FeatureTypePriority,
                                            FeatureTypeDetailPriority = p.FeatureTypeDetailPriority,

                                        }).OrderBy(p => p.Priority).ToList();


            var featureTypeList = new List<VmFeatureType>();
            var bLFeatureType = new BLFeatureType(CurrentLanguageId);

            var groupParentId = (from p in vmProductFeatureList
                                 where p.ParentId == Guid.Empty
                                 select p.Id).First();

            featureTypeList = GetGroupedFeatures(groupParentId, vmProductFeatureList).ToList();

            ///Biz rule 
            ///Get list of Features that user selected. Not all from DB
            var featureTypeWithDetail = bLFeatureType.GetFeatureTypeWithDetailsByCategory(productFeatureList.First().CategoryId).ToList();


            return IsFeatureTypeListChanged(featureTypeList.OrderBy(f => f.Priority).ToList(), featureTypeWithDetail);

        }
        private bool IsFeatureTypeListChanged(List<VmFeatureType> list1, List<VmFeatureType> list2)
        {
            if (list1.Count() != list2.Count())
            {
                return true;
            }
            else
            {
                for (var i = 0; i < list1.Count(); i++)
                {
                    if (list1[i].Id != list2[i].Id)
                    {
                        return true;
                    }
                }

            }

            for (var i = 0; i < list1.Count(); i++)
            {
                if (list1[i].FeatureTypeDetailList.Count() != list2[i].FeatureTypeDetailList.Count())
                {
                    return true;
                }
                for (var j = 0; j < list1[i].FeatureTypeDetailList.OrderBy(d => d.Priority).Count(); j++)
                {
                    if (list1[i].FeatureTypeDetailList[j].Id != list2[i].FeatureTypeDetailList[j].Id)
                    {
                        return true;
                    }
                }
            }
            return false;

        }
        public List<VmFeatureType> GetGroupedFeatures(Guid parentId, List<VmProductFeatureTree> vmProductFeatureList)
        {
            List<VmFeatureType> featureTypeList = new List<VmFeatureType>();

            var featureTypeDetail = (from p in vmProductFeatureList
                                     where p.ParentId == parentId
                                     select new VmFeatureTypeDetail
                                     {
                                         Id = p.FeatureTypeDetailId.Value,
                                         FeatureTypeId = p.FeatureTypeId.Value,
                                         Priority = p.FeatureTypeDetailPriority.Value,

                                     }).OrderBy(f => f.Priority).ToList();

            if (featureTypeDetail.Count() == 0)
            {
                return featureTypeList;
            }

            VmFeatureType featureType =
                 new VmFeatureType
                 {
                     Id = featureTypeDetail.First().FeatureTypeId,
                     FeatureTypeDetailList = featureTypeDetail,
                     Priority = featureTypeDetail.First().Priority,

                 };

            var groupParentId = (from p in vmProductFeatureList
                                 where p.ParentId == parentId
                                 select p.Id).First();

            featureTypeList.Add(featureType);
            featureTypeList.AddRange(GetGroupedFeatures(groupParentId, vmProductFeatureList));
            return featureTypeList;

        }
        public void GenerateProductFeatureTree(int productId, int categoryId)
        {
            var StaticFeatureTypeList = StaticCategoryFeatureTypeList.First(c => c.CategoryId == categoryId).FeatureTypeList;

            int priority = 1;

            var newId = Guid.NewGuid();

            var parentIds = new List<Guid>
            {
                newId
            };

            List<VmProductFeature> vmProductFeatureList = new List<VmProductFeature>
            {
                new VmProductFeature
                {
                    Id = newId,
                    ProductId = productId,
                    FeatureTypeId = 0,
                    FeatureTypeDetailId = null,
                    ParentId = Guid.Empty,
                    Price = null,
                    Quantity = null,
                    Priority = priority
                }
            };

            priority++;

            foreach (var featureType in StaticFeatureTypeList)
            {
                parentIds = CreateProductFeaturesByFeatureType(ref priority, productId, parentIds, featureType.FeatureTypeDetailList, ref vmProductFeatureList);
            }

            var productFeatureList = (from p in vmProductFeatureList
                                      select new ProductFeature
                                      {
                                          Id = p.Id,
                                          ProductId = productId,
                                          FeatureTypeDetailId = p.FeatureTypeDetailId,
                                          ParentId = p.ParentId,
                                          Price = p.Price,
                                          Quantity = p.Quantity,
                                          Priority = p.Priority

                                      }).ToList();

            productFeatureRepository.CreateBatchProductFeature(productFeatureList);
            UnitOfWork.Commit();
        }

        public bool DeleteProductFeatureData(Guid productFeatureId, int categoryId)
        {
            productFeatureRepository.ClearProductFeatureData(productFeatureId);
            bool result = UnitOfWork.Commit();

            if (result == true)
            {
                var blImage = new BLImage(CurrentLanguageId);

                blImage.DeleteImageByProductFeature(productFeatureId);
                UIHelper.DeleteFilesByPattern("/Resources/Uploaded/Product/" + categoryId + "/", "*_" + productFeatureId.ToString().Replace("-", "_") + "_PID_*");
            }

            return result;
        }

        public TreeNode ResetProductFeatureTree(string parentId, int productId, int categoryId)
        {

            #region Load the old Product features tree

            var oldProductFeatureList = viewProductFeatureRepository.GetViewProductFeaturesByProduct(productId).OrderBy(p => p.Priority);

            var oldGuids = oldProductFeatureList.Select(o => o.Id).ToArray();

            var oldVmProductFeatureTree = (from p in oldProductFeatureList
                                           select new VmProductFeatureTree
                                           {
                                               Id = p.Id,
                                               ParentId = p.ParentId.Value,
                                               ProductId = p.ProductId,
                                               FeatureTypeId = p.FeatureTypeId,
                                               FeatureTypeDetailId = p.FeatureTypeDetailId,
                                               Price = p.Price,
                                               Quantity = p.Quantity,
                                               Name = p.FeatureTypeDetailName,
                                               AdditionalData = "",
                                               Priority = p.Priority,
                                               FeatureTypePriority = p.FeatureTypePriority,
                                               FeatureTypeDetailPriority = p.FeatureTypeDetailPriority,
                                               Showcase = p.Showcase,
                                               IconUrl = p.IconUrl,
                                               Description = p.Description,

                                           }).OrderBy(p => p.Priority).ToList();
            #endregion

            #region Extract old Feature Ids

            var vmFeatureTypeList = StaticCategoryFeatureTypeList.First(c => c.CategoryId == categoryId).FeatureTypeList;

            var oldFeatureTypeIdList = oldProductFeatureList.Select(d => d.FeatureTypeId).Distinct();

            var oldProductFeatureTypeDetailList = oldProductFeatureList
                .Where(d => d.FeatureTypeDetailId != null && d.FeatureTypeId == oldFeatureTypeIdList.Last())
                .Select(d => new VmProductFeatureTypeDetail
                {
                    ProductFeatureId = d.Id,
                    ParentId = d.ParentId.Value,
                    FeatureTypeDetailId = d.FeatureTypeDetailId.Value,

                });

            #endregion

            #region Generate New Product Features

            int priority = 1;
            var guidIndex = 0;

            var newId = oldGuids[guidIndex];

            var parentIds = new List<Guid>
            {
                newId
            };

            var newVmProductFeatureList = new List<VmProductFeature>
            {
                new VmProductFeature
                {
                    Id = newId,
                    ProductId = productId,
                    FeatureTypeId = null,
                    FeatureTypeDetailId = null,
                    ParentId = Guid.Empty,
                    Price = null,
                    Quantity = null,
                    Priority = priority,
                }
            };


            priority++;
            guidIndex++;
            int level = 1;
            bool setData = false;

            foreach (var featureType in vmFeatureTypeList)
            {
                if (level == vmFeatureTypeList.Count())
                {
                    setData = true;
                }

                level++;

                parentIds = ResetBatchProductFeature(setData, ref priority, productId, parentIds,
                    oldGuids, ref guidIndex,
                    oldProductFeatureTypeDetailList.ToList(),
                    featureType.FeatureTypeDetailList,
                    ref newVmProductFeatureList,
                    oldVmProductFeatureTree);
            }

            productFeatureRepository.DeleteProductFeatureByProduct(productId);

            var newProductFeatureList = from p in newVmProductFeatureList
                                        select new ProductFeature
                                        {
                                            Id = p.Id,
                                            ProductId = p.ProductId,
                                            FeatureTypeDetailId = p.FeatureTypeDetailId,
                                            ParentId = p.ParentId,
                                            Price = p.Price,
                                            Quantity = p.Quantity,
                                            Priority = p.Priority,
                                            Description = p.Description,
                                            IconUrl = p.IconUrl,
                                            Showcase = p.Showcase,
                                        };

            productFeatureRepository.CreateBatchProductFeature(newProductFeatureList);

            UnitOfWork.Commit();

            #endregion

            var resetProductFeatureList = viewProductFeatureRepository.GetViewProductFeaturesByProduct(productId);
            var resetVmProductFeatureTree = (from p in resetProductFeatureList
                                             select new VmProductFeatureTree
                                             {
                                                 Id = p.Id,
                                                 ParentId = p.ParentId.Value,
                                                 ProductId = p.ProductId,
                                                 FeatureTypeId = p.FeatureTypeId,
                                                 FeatureTypeDetailId = p.FeatureTypeDetailId,
                                                 Price = p.Price,
                                                 Quantity = p.Quantity,
                                                 Name = p.FeatureTypeDetailName,
                                                 AdditionalData = "",
                                                 Priority = p.Priority,
                                                 FeatureTypePriority = p.FeatureTypePriority,
                                                 FeatureTypeDetailPriority = p.FeatureTypeDetailPriority,
                                                 IconUrl = p.IconUrl,
                                                 Showcase = p.Showcase,
                                                 Description = p.Description,


                                             }).OrderBy(p => p.Priority).ToList();

            var blProduct = new BLProduct(CurrentLanguageId);

            resetVmProductFeatureTree.First().Name = blProduct.GetProductById(productId).Name;

            foreach (var item in resetVmProductFeatureTree)
            {
                if (item.FeatureTypeDetailId != null)
                {
                    var productFeature = new VmProductFeature
                    {
                        Id = item.Id,
                        ParentId = item.ParentId.Value,
                        ProductId = item.ProductId,
                        FeatureTypeDetailId = item.FeatureTypeDetailId.Value,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        IconUrl = item.IconUrl,
                        Showcase = item.Showcase,
                        Description = item.Description?.Replace("\'", "\\'").Replace("\r", "").Replace("\n", "<br/>").Replace("\"", "\\\"")

                    };

                    item.AdditionalData = JsonConvert.SerializeObject(productFeature).Replace("\'", "\\'");
                }
            }

            BLTreeModelTools blTreeModelTools = new BLTreeModelTools();

            if (resetVmProductFeatureTree != null && resetVmProductFeatureTree.Count() > 0)
            {
                var tree = blTreeModelTools.GetTreeModel(resetVmProductFeatureTree, parentId);
                return tree;

            }
            return null;

        }

        private List<Guid> ResetBatchProductFeature(
            bool setData,
            ref int priority,
            int productId,
            List<Guid> parentIds,
            Guid[] oldGuids,
            ref int guidIndex,
            List<VmProductFeatureTypeDetail> oldProductFeatureTypeDetailList,
            List<VmFeatureTypeDetail> vmFeatureTypeDetailList,
            ref List<VmProductFeature> newProductFeatureList,
            List<VmProductFeatureTree> oldProductFeatureList)
        {
            var lastParentIds = new List<Guid>();
            BLTreeModelTools blTreeModelTools = new BLTreeModelTools();

            int? featureTypeDetailId = null;
            decimal? Price = null;
            int? Quantity = null;
            bool? Showcase = null;
            string IconUrl = null;
            string Description = null;

            foreach (var parentId in parentIds)
            {
                foreach (var featureTypeDetail in vmFeatureTypeDetailList)
                {
                    var allowSetData = false;

                    var newId = Guid.NewGuid();
                    featureTypeDetailId = featureTypeDetail.Id;

                    Price = null;
                    Quantity = null;
                    Showcase = null;
                    IconUrl = null;
                    Description = null;

                    if (setData == true && oldProductFeatureList.Count() > 1)
                    {
                        int? usedFeatureTypeDetailId = null;
                        VmProductFeatureTypeDetail productFeatureTypeDetail = null;

                        /// Search current leaf in old data firstly
                        productFeatureTypeDetail = oldProductFeatureTypeDetailList.FirstOrDefault(d =>
                                                        d.FeatureTypeDetailId == featureTypeDetailId.Value);

                        if (productFeatureTypeDetail == null)
                        {
                            productFeatureTypeDetail = CheckNewParentsInOldPath(
                                                                 parentId,
                                                                 newProductFeatureList,
                                                                 oldProductFeatureTypeDetailList);

                        }
                        if (productFeatureTypeDetail != null)
                        {
                            var result = CheckOldParentDetailNodeInNewParent(
                                                                            newProductFeatureList, oldProductFeatureList,
                                                                            parentId, productFeatureTypeDetail);
                            if (result == true)
                            {
                                allowSetData = true;
                                usedFeatureTypeDetailId = productFeatureTypeDetail.FeatureTypeDetailId;
                            }
                        }

                        if (allowSetData == true)
                        {
                            var data = oldProductFeatureList.Find(
                                o => o.FeatureTypeDetailId == usedFeatureTypeDetailId);

                            var oldProductFeatureTypeDetail = oldProductFeatureTypeDetailList
                                .First(o => o.FeatureTypeDetailId == usedFeatureTypeDetailId);

                            oldProductFeatureTypeDetailList.Remove(oldProductFeatureTypeDetail);
                            /// Use old ProductFeatureId to keep Images

                            newId = data.Id;
                            Price = data.Price;
                            Quantity = data.Quantity;
                            Showcase = data.Showcase;
                            IconUrl = data.IconUrl;
                            Description = data.Description;

                            oldProductFeatureList.Remove(data);
                        }
                    }

                    newProductFeatureList.Add(new VmProductFeature
                    {
                        Id = newId,
                        ProductId = productId,
                        FeatureTypeId = featureTypeDetail.FeatureTypeId,
                        FeatureTypeDetailId = featureTypeDetailId,
                        ParentId = parentId,
                        Price = Price,
                        Quantity = Quantity,
                        Priority = priority,
                        Showcase = Showcase,
                        IconUrl = IconUrl,
                        Description = Description,
                    });

                    priority++;

                    lastParentIds.Add(newId);

                }
            }

            return lastParentIds;

        }

        private bool CheckOldParentDetailNodeInNewParent(List<VmProductFeature> newProductFeatureList, List<VmProductFeatureTree> oldProductFeatureList, Guid parentId, VmProductFeatureTypeDetail productFeatureTypeDetail)
        {
            VmProductFeature result = null;
            var searchParentId = productFeatureTypeDetail.ParentId;

            var finalResult = false; // Tag of all parent existance result

            while (searchParentId != Guid.Empty)
            {
                var parentFetureTypeDetail = oldProductFeatureList.Find(o => o.Id == searchParentId);

                if (parentFetureTypeDetail != null && parentFetureTypeDetail.FeatureTypeDetailId != null)
                {
                    result = CheckOldParentNodeInNewPath(
                                                            parentId,
                                                            newProductFeatureList,
                                                            parentFetureTypeDetail.FeatureTypeDetailId.Value);
                    if (result == null)
                    {
                        finalResult = false;
                        break;
                    }

                    finalResult = true;

                }

                searchParentId = parentFetureTypeDetail.ParentId;
            }

            return finalResult;
        }

        private VmProductFeatureTypeDetail CheckNewParentsInOldPath(
            Guid parentId,
            List<VmProductFeature> newProductFeatureList,
            List<VmProductFeatureTypeDetail> oldProductFeatureTypeDetailList)
        {
            if (parentId == Guid.Empty)
            {
                return null;
            }

            var newProductFeature = newProductFeatureList.FirstOrDefault(l => l.Id == parentId);

            var result = oldProductFeatureTypeDetailList.FirstOrDefault(d => newProductFeature.FeatureTypeDetailId != null
                &&
                d.FeatureTypeDetailId == newProductFeature.FeatureTypeDetailId.Value);

            if (result != null)
            {
                return result;
            }
            else
            {
                parentId = newProductFeature.ParentId;
                return CheckNewParentsInOldPath(parentId, newProductFeatureList, oldProductFeatureTypeDetailList);
            }
        }


        private VmProductFeature CheckOldParentNodeInNewPath(
            Guid parentId,
            List<VmProductFeature> newProductFeatureList,
            int featureTypeDetailId)
        {
            if (parentId == Guid.Empty)
            {
                return null;
            }

            var newProductFeature = newProductFeatureList.FirstOrDefault(l => l.Id == parentId
                &&
                l.FeatureTypeDetailId == featureTypeDetailId);


            if (newProductFeature != null)
            {
                return newProductFeature;
            }
            else
            {
                newProductFeature = newProductFeatureList.FirstOrDefault(l => l.Id == parentId);
                parentId = newProductFeature.ParentId;
                return CheckOldParentNodeInNewPath(parentId, newProductFeatureList, featureTypeDetailId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="productId"></param>
        /// <param name="parentIds">Preview Ids which they will be used as Parent for current level tree nodes</param>
        /// <param name="vmFeatureTypeDetailList"></param>
        /// <param name="productFeatureList"></param>
        /// <param name="generateByBaseId">if this parameter equals true then tree will be created 
        ///                                 by BaseFeatureTypeDetailId for keep unified search 
        ///                                 paths on tree. Default value is false to generate 
        ///                                 ProductFeature by relationed feature types.
        /// </param>
        /// <returns>Return current ids which they will be used as parent ids for next level of tree nodes </returns>
        public List<Guid> CreateProductFeaturesByFeatureType(ref int priority, int productId, List<Guid> parentIds,
            List<VmFeatureTypeDetail> vmFeatureTypeDetailList, ref List<VmProductFeature> productFeatureList, bool generateByBaseId = false)
        {
            var lastParentIds = new List<Guid>();

            foreach (var parentId in parentIds)
            {
                foreach (var featureTypeDetail in vmFeatureTypeDetailList)
                {
                    var newId = Guid.NewGuid();

                    productFeatureList.Add(new VmProductFeature
                    {
                        Id = newId,
                        ProductId = productId,
                        FeatureTypeId = featureTypeDetail.FeatureTypeId,
                        FeatureTypeDetailId = generateByBaseId ? featureTypeDetail.BaseFeatureTypeDetailId : featureTypeDetail.Id,
                        ParentId = parentId,
                        Price = null,
                        Quantity = null,
                        Priority = priority,
                    });

                    priority++;

                    lastParentIds.Add(newId);

                }
            }

            return lastParentIds;

        }

        public bool UpdateProductFeatureTreeNode(VmProductFeature productFeature)
        {
            string resultIconUrl = SaveIconUrl(productFeature.Id, productFeature.ProductId, productFeature.CategoryId, productFeature.IconUrl);

            productFeatureRepository.UpdateProductFeature(productFeature.Id, productFeature.Showcase,
                productFeature.Price, productFeature.Quantity, resultIconUrl, productFeature.Description);

            SaveImages(productFeature.Id, productFeature.ProductId, productFeature.CategoryId, productFeature.ClientImages, productFeature.ImagesPriority);

            return UnitOfWork.Commit();
        }
        private string SaveIconUrl(Guid productFeatureId, int productId, int categoryId, string iconUrl)
        {
            if (iconUrl.ToLower().Contains("data:") == true && iconUrl.ToLower().Contains("base64") == true)
            {
                UIHelper.DeleteFilesByPattern("/Resources/Uploaded/Product/" + categoryId + "/", "*_" + productFeatureId.ToString().Replace("-", "_") + "_PIID_*");

                var resultUrl = UIHelper.UploadPictureFile(
                       iconUrl.Split(new string[] { "base64," }, StringSplitOptions.RemoveEmptyEntries)[1],
                       productId + "_PIID_" + productFeatureId.ToString().Replace("-", "_") + ".png",
                       iconUrl.Split(new string[] { "base64," }, StringSplitOptions.RemoveEmptyEntries)[0],
                       "/Resources/Uploaded/Product/" + categoryId);

                return resultUrl;
            }

            return iconUrl.Split('?')[0];


        }
        private void SaveImages(Guid productFeatureId, int productId, int categoryId, string clientImages, string imagesPriority)
        {
            BLImage blImage = new BLImage(CurrentLanguageId);

            var vmImages = new List<VmImage>();

            var Base64ClientImages = JsonConvert.DeserializeObject<Model.ViewModels.ProductImage.VmClientImageBase64[]>(clientImages);

            if (blImage.DeleteImageByProductFeature(productFeatureId) == true)
            {
                UIHelper.DeleteFilesByPattern("/Resources/Uploaded/Product/" + categoryId + "/", "*_" + productFeatureId.ToString().Replace("-", "_") + "_PID_*");
            }

            if (Base64ClientImages.Count() == 0)
            {
                return;
            }
            var orderedFileNames = imagesPriority?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int priorityNo = 0;

            foreach (var fileName in orderedFileNames)
            {
                var base64ClientImage = Base64ClientImages.Where(f => f.FileName == fileName).FirstOrDefault();
                var base64String = base64ClientImage.Base64String.Split(new string[] { "base64," }, StringSplitOptions.RemoveEmptyEntries)[1];
                var contentType = base64ClientImage.Base64String.Split(new string[] { "base64," }, StringSplitOptions.RemoveEmptyEntries)[0];

                vmImages.Add(new VmImage
                {
                    ImageUrl = UIHelper.UploadPictureFile(
                        base64String,
                        productFeatureId + "_pid_" + fileName.ToLower().Replace("_pid_", "").Replace("_" + productFeatureId.ToString().Replace("-", "_").ToLower(), ""),
                        contentType,
                        "/Resources/Uploaded/Product/" + categoryId),

                    LinkUrl = "",
                    Title = "",
                    Priority = ++priorityNo,
                    ProducFeaturetId = productFeatureId

                });
            }

            if (blImage.BatchCreateImage(vmImages) == false)
            {
                UIHelper.DeleteFilesByPattern("/Resources/Uploaded/Product/" + categoryId + "/", "*_" + productFeatureId.ToString().Replace("-", "_") + "_PID_*");
            }
        }
        public VmProductFeature GetProductFeatureById(Guid id)
        {
            var productFeature = productFeatureRepository.GetProductFeatureById(id);

            return new VmProductFeature
            {
                Id = productFeature.Id,
                ParentId = productFeature.ParentId,
                ProductId = productFeature.ProductId,
                FeatureTypeDetailId = productFeature.FeatureTypeDetailId,
                Price = productFeature.Price,
                Quantity = productFeature.Quantity,
                IconUrl = productFeature.IconUrl,
                Images = (from i in productFeature.Images
                          select new VmImage
                          {
                              Id = i.Id,
                              ImageUrl = i.ImageUrl,
                              LinkUrl = i.LinkUrl,
                              Priority = i.Priority,
                              ProducFeaturetId = productFeature.Id,
                              Title = i.Title
                          }).ToList()
            };
        }
        public IEnumerable<VmShopProductFullInfo> GetHomeShopProducts(int index = 0, int count = int.MaxValue)
        {
            Expression<Func<ViewShopProduct, bool>> predicate = sp => sp.Showcase == true;

            return GetShopProducts(predicate);
        }

        public IEnumerable<VmShopProductFullInfo> GetShopProducts(Expression<Func<ViewShopProduct, bool>> expression)
        {
            var productIconList = GetShopProductsFeatureTypeDetailGroup(expression);

            var featureTypeDetailIdlist = (from i in productIconList
                                           select new
                                           {
                                               i.IconUrlInfoList.First().ProductId,
                                               i.IconUrlInfoList.First().FeatureTypeDetailId,
                                               i.IconUrlInfoList.First().BaseFeatureTypeDetailId,

                                           }).ToList();

            var predicateBuilderShopProduct = PredicateBuilder.True<ViewShopProduct>();
            var predicateBuilderInnerShopProduct = PredicateBuilder.False<ViewShopProduct>();

            foreach (var item in featureTypeDetailIdlist)
            {
                predicateBuilderInnerShopProduct = predicateBuilderInnerShopProduct.Or(p => p.ProductId == item.ProductId && p.FeatureTypeDetailId == item.FeatureTypeDetailId);
            }

            predicateBuilderShopProduct = predicateBuilderShopProduct.And(expression);
            predicateBuilderShopProduct = predicateBuilderShopProduct.And(predicateBuilderInnerShopProduct);

            IEnumerable<ViewShopProduct> viewShopProductList =
                viewShopProductRepository.GetShopProducts(predicateBuilderShopProduct, 0, int.MaxValue).ToList();

            var vmShopProductShopProductFullInfo =
                 (from spc in viewShopProductList
                  group new
                  {
                      spc.Id,
                      spc.ParentId,
                      spc.ProductId,
                      spc.FeatureTypeId,
                      spc.FeatureTypeName,
                      spc.FeatureTypeDetailId,
                      spc.BaseFeatureTypeDetailId,
                      spc.FeatureTypeDetailName,
                      spc.Price,
                      spc.Quantity,
                      spc.ImageUrl,
                      spc.IconUrl,
                      spc.LinkUrl,
                      spc.ImagePriority,
                      spc.ImageTitle,
                      spc.Description,
                  }
                 by new
                 {
                     spc.CategoryId,
                     spc.CategoryName,
                     spc.ProductId,
                     spc.ProductName,
                     spc.ProductionDate,
                     spc.ExpiryDate,
                     spc.ProductDescription,

                 } into g
                  select new VmShopProductFullInfo
                  {
                      CategoryId = g.Key.CategoryId,
                      CategoryName = g.Key.CategoryName,
                      ProductId = g.Key.ProductId,
                      ProductName = g.Key.ProductName,
                      ProductionDate = g.Key.ProductionDate,
                      ExpiryDate = g.Key.ExpiryDate,
                      ProductDescription = g.Key.ProductDescription,

                      ProductIconUrlInfoList = (from ig in productIconList
                                                where ig.ProductId == g.Key.ProductId
                                                select ig.IconUrlInfoList).FirstOrDefault(),

                      ShopProductList = (from p in g.ToList()
                                         select new VmShopProduct
                                         {
                                             Id = p.Id,
                                             ParentId = p.ParentId.Value,
                                             ProductId = p.ProductId,
                                             FeatureTypeId = p.FeatureTypeId,
                                             FeatureTypeName = p.FeatureTypeName,
                                             FeatureTypeDetailId = p.FeatureTypeDetailId.Value,
                                             BaseFeatureTypeDetailId = p.BaseFeatureTypeDetailId,
                                             FeatureTypeDetailName = p.FeatureTypeDetailName,
                                             Price = p.Price,
                                             Quantity = p.Quantity,
                                             ImageUrl = p.ImageUrl,
                                             LinkUrl = p.LinkUrl,
                                             ImagePriority = p.ImagePriority,
                                             ImageTitle = p.ImageTitle,
                                             Description = p.Description,

                                         }).ToList()
                  }).ToList();

            return vmShopProductShopProductFullInfo;
        }

        public IEnumerable<VmShopProductFullInfo> GetProductFeaturesByFilter(
                        VmFilterParameter filterParameter, VmSundryImage vmSundryImage)
        {
            /// Biz rule 
            /// Category ID is Temporary

            int categoryId = vmSundryImage.ObjectId;
            var staticCategoryFeatureType = StaticCategoryFeatureTypeList.FirstOrDefault(c => c.CategoryId == categoryId);
            if (staticCategoryFeatureType == null)
            {
                BusinessMessage.HasError = true;
                BusinessMessage.Message = "Feature/s not assigned to this category";

                return null;
            }
            var StaticFeatureTypeList = staticCategoryFeatureType.FeatureTypeList;

            decimal minPrice = 0;
            decimal maxPrice = int.MaxValue;

            var blTreeModelTools = new BLTreeModelTools();

            var featureTypeList = StaticFeatureTypeList;
            var featureTypesForTree = new List<VmFeatureType>();
            int firstIconPriority;

            if (filterParameter != null)
            {
                var existFeatureTypeIds = filterParameter.FeatureTypes.Select(f => f.Id);

                var tempFilterParameter = new VmFilterParameter
                {
                    FeatureTypes = new List<VmFeatureType>()
                };

                tempFilterParameter.FeatureTypes.AddRange(filterParameter.FeatureTypes);

                tempFilterParameter.FeatureTypes.AddRange(
                    featureTypeList.Where(f => existFeatureTypeIds.Contains(f.Id) == false).ToList());

                foreach (var item in tempFilterParameter.FeatureTypes)
                {
                    if (item.FeatureTypeDetailList.Count() == 0)
                    {
                        item.FeatureTypeDetailList = featureTypeList.Where(f => f.Id == item.Id).Select(d => d.FeatureTypeDetailList).First();
                    }
                }

                firstIconPriority = tempFilterParameter.FeatureTypes.OrderBy(f => f.Priority).Last().FeatureTypeDetailList.Min(d => d.Priority);

                if (firstIconPriority == 0)
                {
                    firstIconPriority = staticCategoryFeatureType.FirstIconPriority.Value;
                }

                featureTypesForTree.AddRange(tempFilterParameter.FeatureTypes);


                minPrice = decimal.Parse(filterParameter.MinPrice);
                maxPrice = decimal.Parse(filterParameter.MaxPrice);

            }
            else
            {
                firstIconPriority = staticCategoryFeatureType.FirstIconPriority.Value;

                featureTypesForTree.AddRange(featureTypeList);
            }

            string featureDetailCombinationList = string.Empty;

            var parentIds = string.Empty;

            switch (vmSundryImage.PackageItemType)
            {
                case PackageItemType.Category:

                    var categoryIds = new BLCategory(CurrentLanguageId).GetChildIds(vmSundryImage.ObjectId);
                    var parentIdList = viewProductFeatureRepository.GetViewShopProductsTreeParentIds(categoryIds.ToArray());

                    foreach (var item in parentIdList)
                    {
                        parentIds += item + ",";
                    }

                    if (!string.IsNullOrWhiteSpace(parentIds))
                    {
                        parentIds = parentIds.Substring(0, parentIds.Length - 1);
                    }
                    else
                    {
                        parentIds = Guid.Empty.ToString();
                    }


                    break;

                case PackageItemType.New:

                    return GetShopProducts(p => p.ExpiryDate >= DateTime.Now).OrderBy(p => p.ProductionDate);

                case PackageItemType.Discount:
                case PackageItemType.Popular:
                default:

                    return GetShopProducts(p => p.Showcase == true);
            }

            var tree = GenerateInMemoryTree(featureTypesForTree, true);
            var paths = blTreeModelTools.GenerateTreePath(tree, "/");

            foreach (var item in paths)
            {
                if (string.IsNullOrWhiteSpace(item) == false)
                {
                    featureDetailCombinationList += item + ",";
                }
            }

            if (featureDetailCombinationList.Length > 0)
            {
                featureDetailCombinationList = featureDetailCombinationList.Substring(0, featureDetailCombinationList.Length - 1);
            }

            string groupedIcons = string.Empty;
            string delimiter = "/";

            List<ViewShopProduct> viewShopProductList = spHandlerRepository.GetProductFeaturesByFilter(
                    out groupedIcons,
                    firstIconPriority,
                    featureDetailCombinationList,
                    delimiter,
                    parentIds,
                    minPrice,
                    maxPrice
                );

            #region Product Icons

            var groupedIconsXml = XDocument.Parse("<root>" + groupedIcons + "</root>");

            var xmltest = (from x in groupedIconsXml.Descendants("groupedIcons")
                           select new VmProductIconGroup
                           {
                               Id = Guid.Parse(x.Attribute("Id").Value),
                               ProductId = int.Parse(x.Attribute("ProductId").Value),
                               IconUrl = x.Attribute("IconUrl").Value,
                               FeatureTypeId = int.Parse(x.Attribute("FeatureTypeId").Value),
                               FeatureTypeName = x.Attribute("FeatureTypeName").Value,
                               FeatureTypeDetailId = int.Parse(x.Attribute("FeatureTypeDetailId").Value),
                               BaseFeatureTypeDetailId = int.Parse(x.Attribute("BaseFeatureTypeDetailId").Value),
                               FeatureTypeDetailPriority = int.Parse(x.Attribute("FeatureTypeDetailPriority").Value),
                               FeatureTypeDetailName = x.Attribute("FeatureTypeDetailName").Value,
                               XmlParentId = Guid.Parse(x.Attribute("ParentId").Value),

                           }).ToList();

            var productIconList = from p in xmltest
                                  group p by p.ProductId into pg
                                  select new VmProductIconList
                                  {
                                      ProductId = pg.Key,
                                      IconUrlInfoList = (from i in pg.OrderBy(p => p.FeatureTypeDetailPriority).ToList()
                                                         group new
                                                         {
                                                             i.Id,
                                                             i.ProductId,
                                                             i.IconUrl,
                                                             i.FeatureTypeId,
                                                             i.FeatureTypeName,
                                                             i.FeatureTypeDetailId,
                                                             i.BaseFeatureTypeDetailId,
                                                             i.FeatureTypeDetailName,
                                                             i.XmlFeatureDetailCombination,
                                                             i.XmlDelimiter,
                                                             i.XmlParentId,
                                                             i.XmlMinPrice,
                                                             i.XmlMaxPrice,
                                                         }
                                                         by new
                                                         {
                                                             i.FeatureTypeId,
                                                             i.FeatureTypeName,
                                                             i.FeatureTypeDetailId,
                                                             i.BaseFeatureTypeDetailId,
                                                             i.FeatureTypeDetailName,
                                                         }
                                                         into ig
                                                         select new VmProductIconUrlInfo
                                                         {
                                                             ProductId = pg.Key,
                                                             ProductFeatureId = ig.First().Id,

                                                             IconUrl =
                                                                (ig.FirstOrDefault(i => i.IconUrl != "/Resources/Images/pic/none-icon.png") == null)
                                                                ? ig.First().IconUrl
                                                                : ig.FirstOrDefault(i => i.IconUrl != "/Resources/Images/pic/none-icon.png").IconUrl,

                                                             FeatureTypeId = ig.Key.FeatureTypeId,
                                                             FeatureTypeName = ig.Key.FeatureTypeName,
                                                             FeatureTypeDetailId = ig.Key.FeatureTypeDetailId,
                                                             BaseFeatureTypeDetailId = ig.Key.BaseFeatureTypeDetailId,
                                                             FeatureTypeDetailName = ig.Key.FeatureTypeDetailName,
                                                             ParentId = ig.First().XmlParentId,


                                                         }).ToList()
                                  };

            #endregion Product Icons

            var vmShopProductShopProductFullInfo =
                 (from spc in viewShopProductList
                  group new
                  {
                      spc.Id,
                      spc.ParentId,
                      spc.ProductId,
                      spc.FeatureTypeId,
                      spc.FeatureTypeName,
                      spc.FeatureTypeDetailId,
                      spc.BaseFeatureTypeDetailId,
                      spc.FeatureTypeDetailName,
                      spc.Price,
                      spc.Quantity,
                      spc.ImageUrl,
                      spc.IconUrl,
                      spc.LinkUrl,
                      spc.ImagePriority,
                      spc.ImageTitle,
                      spc.Description,
                  }
                 by new
                 {
                     spc.CategoryId,
                     spc.CategoryName,
                     spc.ProductId,
                     spc.ProductName,
                     spc.ProductionDate,
                     spc.ExpiryDate,
                     spc.ProductDescription,

                 } into g
                  select new VmShopProductFullInfo
                  {
                      CategoryId = g.Key.CategoryId,
                      CategoryName = g.Key.CategoryName,
                      ProductId = g.Key.ProductId,
                      ProductName = g.Key.ProductName,
                      ProductionDate = g.Key.ProductionDate,
                      ExpiryDate = g.Key.ExpiryDate,
                      ProductDescription = g.Key.ProductDescription,

                      ProductIconUrlInfoList = (from ig in productIconList
                                                where ig.ProductId == g.Key.ProductId
                                                select ig.IconUrlInfoList).FirstOrDefault(),

                      ShopProductList = (from p in g.ToList()
                                         select new VmShopProduct
                                         {
                                             Id = p.Id,
                                             ParentId = p.ParentId.Value,
                                             ProductId = p.ProductId,
                                             FeatureTypeId = p.FeatureTypeId,
                                             FeatureTypeName = p.FeatureTypeName,
                                             FeatureTypeDetailId = p.FeatureTypeDetailId.Value,
                                             BaseFeatureTypeDetailId = p.BaseFeatureTypeDetailId,
                                             FeatureTypeDetailName = p.FeatureTypeDetailName,
                                             Price = p.Price,
                                             Quantity = p.Quantity,
                                             ImageUrl = p.ImageUrl,
                                             LinkUrl = p.LinkUrl,
                                             ImagePriority = p.ImagePriority,
                                             ImageTitle = p.ImageTitle,
                                             Description = p.Description,

                                         }).ToList()
                  }).ToList();
            if (filterParameter != null)
            {
                foreach (var product in vmShopProductShopProductFullInfo)
                {
                    switch (filterParameter.ProductSortType)
                    {
                        case ProductSortType.HighestToLowestPrice:
                            product.ShopProductList = product.ShopProductList.OrderBy(s => s.Price).ToList();
                            break;
                        case ProductSortType.LowestToHighestPrice:
                            product.ShopProductList = product.ShopProductList.OrderByDescending(s => s.Price).ToList();
                            break;
                        default:
                            break;
                    }

                }
            }
            return vmShopProductShopProductFullInfo;
        }

        public IEnumerable<VmShopProductFullInfo> GetProductsByCategory(int categoryId)
        {

            var staticCategoryFeatureType = StaticCategoryFeatureTypeList.First(c => c.CategoryId == categoryId);
            var StaticFeatureTypeList = staticCategoryFeatureType.FeatureTypeList;

            decimal minPrice = 0;
            decimal maxPrice = int.MaxValue;

            var blTreeModelTools = new BLTreeModelTools();

            var featureTypeList = StaticFeatureTypeList;

            var featureTypesForTree = new List<VmFeatureType>();
            int firstIconPriority;

            firstIconPriority = staticCategoryFeatureType.FirstIconPriority.Value;

            featureTypesForTree.AddRange(featureTypeList);

            string featureDetailCombinationList = string.Empty;

            var parentIds = string.Empty;

            var categoryIds = new BLCategory(CurrentLanguageId).GetChildIds(categoryId);
            var parentIdList = viewProductFeatureRepository.GetViewShopProductsTreeParentIds(categoryIds.ToArray());

            foreach (var item in parentIdList)
            {
                parentIds += item + ",";
            }

            if (!string.IsNullOrWhiteSpace(parentIds))
            {
                parentIds = parentIds.Substring(0, parentIds.Length - 1);
            }
            else
            {
                parentIds = Guid.Empty.ToString();
            }


            var tree = GenerateInMemoryTree(featureTypesForTree, true);
            var paths = blTreeModelTools.GenerateTreePath(tree, "/");

            foreach (var item in paths)
            {
                if (string.IsNullOrWhiteSpace(item) == false)
                {
                    featureDetailCombinationList += item + ",";
                }
            }

            if (featureDetailCombinationList.Length > 0)
            {
                featureDetailCombinationList = featureDetailCombinationList.Substring(0, featureDetailCombinationList.Length - 1);
            }

            string groupedIcons = string.Empty;
            string delimiter = "/";

            List<ViewShopProduct> viewShopProductList = spHandlerRepository.GetProductFeaturesByFilter(
                    out groupedIcons,
                    firstIconPriority,
                    featureDetailCombinationList,
                    delimiter,
                    parentIds,
                    minPrice,
                    maxPrice
                );

            #region Product Icons

            var groupedIconsXml = XDocument.Parse("<root>" + groupedIcons + "</root>");

            var xmltest = (from x in groupedIconsXml.Descendants("groupedIcons")
                           select new VmProductIconGroup
                           {
                               Id = Guid.Parse(x.Attribute("Id").Value),
                               ProductId = int.Parse(x.Attribute("ProductId").Value),
                               IconUrl = x.Attribute("IconUrl").Value,
                               FeatureTypeId = int.Parse(x.Attribute("FeatureTypeId").Value),
                               FeatureTypeName = x.Attribute("FeatureTypeName").Value,
                               FeatureTypeDetailId = int.Parse(x.Attribute("FeatureTypeDetailId").Value),
                               BaseFeatureTypeDetailId = int.Parse(x.Attribute("BaseFeatureTypeDetailId").Value),
                               FeatureTypeDetailPriority = int.Parse(x.Attribute("FeatureTypeDetailPriority").Value),
                               FeatureTypeDetailName = x.Attribute("FeatureTypeDetailName").Value,
                               XmlParentId = Guid.Parse(x.Attribute("ParentId").Value),

                           }).ToList();

            var productIconList = from p in xmltest
                                  group p by p.ProductId into pg
                                  select new VmProductIconList
                                  {
                                      ProductId = pg.Key,
                                      IconUrlInfoList = (from i in pg.OrderBy(p => p.FeatureTypeDetailPriority).ToList()
                                                         group new
                                                         {
                                                             i.Id,
                                                             i.ProductId,
                                                             i.IconUrl,
                                                             i.FeatureTypeId,
                                                             i.FeatureTypeName,
                                                             i.FeatureTypeDetailId,
                                                             i.BaseFeatureTypeDetailId,
                                                             i.FeatureTypeDetailName,
                                                             i.XmlFeatureDetailCombination,
                                                             i.XmlDelimiter,
                                                             i.XmlParentId,
                                                             i.XmlMinPrice,
                                                             i.XmlMaxPrice,
                                                         }
                                                         by new
                                                         {
                                                             i.FeatureTypeId,
                                                             i.FeatureTypeName,
                                                             i.FeatureTypeDetailId,
                                                             i.BaseFeatureTypeDetailId,
                                                             i.FeatureTypeDetailName,
                                                         }
                                                         into ig
                                                         select new VmProductIconUrlInfo
                                                         {
                                                             ProductId = pg.Key,
                                                             ProductFeatureId = ig.First().Id,

                                                             IconUrl =
                                                                (ig.FirstOrDefault(i => i.IconUrl != "/Resources/Images/pic/none-icon.png") == null)
                                                                ? ig.First().IconUrl
                                                                : ig.FirstOrDefault(i => i.IconUrl != "/Resources/Images/pic/none-icon.png").IconUrl,

                                                             FeatureTypeId = ig.Key.FeatureTypeId,
                                                             FeatureTypeName = ig.Key.FeatureTypeName,
                                                             FeatureTypeDetailId = ig.Key.FeatureTypeDetailId,
                                                             BaseFeatureTypeDetailId = ig.Key.BaseFeatureTypeDetailId,
                                                             FeatureTypeDetailName = ig.Key.FeatureTypeDetailName,
                                                             ParentId = ig.First().XmlParentId,


                                                         }).ToList()
                                  };

            #endregion Product Icons

            var vmShopProductShopProductFullInfo =
                 (from spc in viewShopProductList
                  group new
                  {
                      spc.Id,
                      spc.ParentId,
                      spc.ProductId,
                      spc.FeatureTypeId,
                      spc.FeatureTypeName,
                      spc.FeatureTypeDetailId,
                      spc.BaseFeatureTypeDetailId,
                      spc.FeatureTypeDetailName,
                      spc.Price,
                      spc.Quantity,
                      spc.ImageUrl,
                      spc.IconUrl,
                      spc.LinkUrl,
                      spc.ImagePriority,
                      spc.ImageTitle,
                      spc.Description,
                  }
                 by new
                 {
                     spc.CategoryId,
                     spc.CategoryName,
                     spc.ProductId,
                     spc.ProductName,
                     spc.ProductionDate,
                     spc.ExpiryDate,
                     spc.ProductDescription,

                 } into g
                  select new VmShopProductFullInfo
                  {
                      CategoryId = g.Key.CategoryId,
                      CategoryName = g.Key.CategoryName,
                      ProductId = g.Key.ProductId,
                      ProductName = g.Key.ProductName,
                      ProductionDate = g.Key.ProductionDate,
                      ExpiryDate = g.Key.ExpiryDate,
                      ProductDescription = g.Key.ProductDescription,

                      ProductIconUrlInfoList = (from ig in productIconList
                                                where ig.ProductId == g.Key.ProductId
                                                select ig.IconUrlInfoList).FirstOrDefault(),

                      ShopProductList = (from p in g.ToList()
                                         select new VmShopProduct
                                         {
                                             Id = p.Id,
                                             ParentId = p.ParentId.Value,
                                             ProductId = p.ProductId,
                                             FeatureTypeId = p.FeatureTypeId,
                                             FeatureTypeName = p.FeatureTypeName,
                                             FeatureTypeDetailId = p.FeatureTypeDetailId.Value,
                                             BaseFeatureTypeDetailId = p.BaseFeatureTypeDetailId,
                                             FeatureTypeDetailName = p.FeatureTypeDetailName,
                                             Price = p.Price,
                                             Quantity = p.Quantity,
                                             ImageUrl = p.ImageUrl,
                                             LinkUrl = p.LinkUrl,
                                             ImagePriority = p.ImagePriority,
                                             ImageTitle = p.ImageTitle,
                                             Description = p.Description,

                                         }).ToList()
                  }).ToList();

            return vmShopProductShopProductFullInfo;
        }

        public IEnumerable<VmShopProductFullInfo> GetProductByFeatureTypeDetail(int productId, int featureTypeDetailId)
        {
            Expression<Func<ViewShopProduct, bool>> predicate = p => p.ProductId == productId && p.BaseFeatureTypeDetailId == featureTypeDetailId;

            IEnumerable<ViewShopProduct> viewShopProductList =
                viewShopProductRepository.GetShopProducts(predicate, 0, int.MaxValue).ToList();

            var vmShopProductShopProductFullInfo =
                 (from spc in viewShopProductList
                  group new
                  {
                      spc.Id,
                      spc.ParentId,
                      spc.ProductId,
                      spc.FeatureTypeId,
                      spc.FeatureTypeName,
                      spc.BaseFeatureTypeDetailId,
                      spc.FeatureTypeDetailId,
                      spc.FeatureTypeDetailName,
                      spc.Price,
                      spc.Quantity,
                      spc.ImageUrl,
                      spc.IconUrl,
                      spc.LinkUrl,
                      spc.ImagePriority,
                      spc.ImageTitle,
                      spc.Description,
                  }
                 by new
                 {
                     spc.CategoryId,
                     spc.CategoryName,
                     spc.ProductId,
                     spc.ProductName,
                     spc.ProductionDate,
                     spc.ExpiryDate,
                     spc.ProductDescription,

                 } into g
                  select new VmShopProductFullInfo
                  {
                      CategoryId = g.Key.CategoryId,
                      CategoryName = g.Key.CategoryName,
                      ProductId = g.Key.ProductId,
                      ProductName = g.Key.ProductName,
                      ProductionDate = g.Key.ProductionDate,
                      ExpiryDate = g.Key.ExpiryDate,
                      ProductDescription = g.Key.ProductDescription,

                      ShopProductList = (from p in g.ToList()
                                         select new VmShopProduct
                                         {
                                             Id = p.Id,
                                             ParentId = p.ParentId.Value,
                                             ProductId = p.ProductId,
                                             FeatureTypeId = p.FeatureTypeId,
                                             FeatureTypeName = p.FeatureTypeName,
                                             BaseFeatureTypeDetailId = p.BaseFeatureTypeDetailId,
                                             FeatureTypeDetailId = p.FeatureTypeDetailId.Value,
                                             FeatureTypeDetailName = p.FeatureTypeDetailName,
                                             Price = p.Price,
                                             Quantity = p.Quantity,
                                             ImageUrl = p.ImageUrl,
                                             LinkUrl = p.LinkUrl,
                                             ImagePriority = p.ImagePriority,
                                             ImageTitle = p.ImageTitle,
                                             Description = p.Description,

                                         }).ToList()
                  }).ToList();

            return vmShopProductShopProductFullInfo;
        }

        public IEnumerable<VmShopProductFullInfo> GetProductPreviewFullInfo(
            int productId, int categoryId, int? clientFeatureTypeDetailId = null)
        {

            var staticCategoryFeatureType = StaticCategoryFeatureTypeList.First(c => c.CategoryId == categoryId);
            var StaticFeatureTypeList = staticCategoryFeatureType.FeatureTypeList;

            var blTreeModelTools = new BLTreeModelTools();

            #region Generate tree from fetched product tree data
            //Biz rule Caching
            Expression<Func<ViewProductFeatureFullInfo, bool>> predicate = p => p.ProductId == productId;

            IEnumerable<ViewProductFeatureFullInfo> viewProductFeatureFullInfoList =
                viewProductFeatureFullInfoRepository.GetProductFeatureFulInfoes(predicate, 0, int.MaxValue).ToList();

            var parentId = viewProductFeatureFullInfoList.First(p => p.ParentId == Guid.Empty).Id;

            var productFeatureTreeList = (from p in viewProductFeatureFullInfoList
                                              ///where p.ImageUrl == null
                                          select new VmProductFeatureTree
                                          {
                                              Id = p.Id,
                                              ParentId = p.ParentId.Value,
                                              ProductId = p.ProductId,
                                              FeatureTypeId = p.FeatureTypeId,
                                              FeatureTypeDetailId = p.BaseFeatureTypeDetailId,
                                              Price = p.Price,
                                              Quantity = p.Quantity,
                                              Name = p.BaseFeatureTypeDetailId.ToString(),
                                              AdditionalData = "",
                                              Priority = p.Priority,
                                              FeatureTypePriority = p.FeatureTypePriority,
                                              FeatureTypeDetailPriority = p.FeatureTypeDetailPriority,
                                              IconUrl = p.IconUrl,
                                              Showcase = p.Showcase,
                                              Description = p.Description,

                                          }).OrderBy(p => p.Priority).ToList().GroupBy(x => x.Id).Select(g => g.First());
            //Biz rule Caching

            var tree = blTreeModelTools.GetTreeModel(productFeatureTreeList, parentId.ToString());
            #endregion Generate tree from fetched product tree data

            #region Generate shop product list with paths

            var shopProductListWithPaths = ConvertProductFeatureTreeToList(tree, "/");

            #endregion Generate shop product list with paths

            #region Complete shop product list data  

            var searchList = (from p in productFeatureTreeList
                              join s in shopProductListWithPaths
                                   on p.Id equals s.Id
                              select new VmShopProduct
                              {
                                  Path = s.Path,
                                  Id = s.Id,
                                  ParentId = s.ParentId,
                                  ProductId = p.ProductId,
                                  FeatureTypeId = p.FeatureTypeId.Value,
                                  FeatureTypePriority = p.FeatureTypePriority,
                                  FeatureTypeDetailId = p.FeatureTypeDetailId.Value,
                                  FeatureTypeDetailName = s.FeatureTypeDetailName,
                                  FeatureTypeDetailPriority = p.FeatureTypeDetailPriority,
                                  Price = p.Price,
                                  Quantity = p.Quantity,
                                  IconUrl = p.IconUrl,
                                  Showcase = p.Showcase,
                                  Description = p.Description,

                              }).ToList();

            #endregion Complete shop product list data

            #region Calulate feature type statistics

            clientFeatureTypeDetailId = clientFeatureTypeDetailId ?? StaticFeatureTypeList.OrderBy(f => f.Priority).Last()
                .FeatureTypeDetailList.OrderBy(d => d.Priority).First().Id;

            var leafFeatureTypeId = StaticFeatureTypeList.OrderByDescending(f => f.Priority).First().Id;
            var tempFeatureTypeList = new List<VmFeatureType>();

            foreach (var f in StaticFeatureTypeList)
            {
                var tempFeatureType = new VmFeatureType
                {
                    Id = f.Id,
                    BaseFeatureTypeId = f.BaseFeatureTypeId,
                    Name = f.Name,
                    Priority = f.Priority,
                    FeatureTypeDetailList = new List<VmFeatureTypeDetail>()
                };

                foreach (var d in f.FeatureTypeDetailList)
                {
                    if (f.Id == leafFeatureTypeId && d.BaseFeatureTypeDetailId == clientFeatureTypeDetailId)
                    {
                        tempFeatureType.FeatureTypeDetailList.Add(new VmFeatureTypeDetail
                        {
                            Id = d.Id,
                            FeatureTypeId = d.FeatureTypeId,
                            BaseFeatureTypeDetailId = d.BaseFeatureTypeDetailId,
                            Name = d.Name,
                            IsLeaf = d.IsLeaf,
                        });
                    }
                    else
                    if (f.Id != leafFeatureTypeId)
                    {
                        tempFeatureType.FeatureTypeDetailList.Add(new VmFeatureTypeDetail
                        {
                            Id = d.Id,
                            FeatureTypeId = d.FeatureTypeId,
                            BaseFeatureTypeDetailId = d.BaseFeatureTypeDetailId,
                            Name = d.Name,
                            IsLeaf = d.IsLeaf,

                        });
                    }
                }

                tempFeatureTypeList.Add(tempFeatureType);
            }

            var featureTypesTree = GenerateInMemoryTree(tempFeatureTypeList, true);

            var featureTypesTreeWithPaths = ConvertFeatureTypeTreeToList(featureTypesTree, "/");

            #region Extract quantity for detail ids

            var productFeatureTypeDetailList = new List<VmProductFeatureTypeDetail>();

            foreach (var fttp in featureTypesTreeWithPaths)
            {
                var result = searchList.First(s => s.Path == fttp.Path);
                var featurTypeDetailParentList = result.Path.Split(new char[] { '/' });
                // M/L/R

                for (int i = featurTypeDetailParentList.Length - 1; i >= 0; i--)
                {
                    var featureDetailTypeId = int.Parse(featurTypeDetailParentList[i]);
                    var isLeaf = (i == featurTypeDetailParentList.Length - 1);

                    productFeatureTypeDetailList.Add(new VmProductFeatureTypeDetail
                    {
                        FeatureTypeId = StaticFeatureTypeList[i].Id,
                        FeatureTypePriority = StaticFeatureTypeList[i].Priority,
                        FeatureTypeName = StaticFeatureTypeList[i].Name,
                        FeatureTypeDetailId = featureDetailTypeId,

                        FeatureTypeDetailName = StaticFeatureTypeList
                                .First(f => f.Id == StaticFeatureTypeList[i].Id)
                                .FeatureTypeDetailList.First(d => d.BaseFeatureTypeDetailId == featureDetailTypeId).Name,

                        FeatureTypeDetailPriority = StaticFeatureTypeList
                                .First(f => f.Id == StaticFeatureTypeList[i].Id)
                                .FeatureTypeDetailList.First(d => d.BaseFeatureTypeDetailId == featureDetailTypeId).Priority,

                        IsLeaf = isLeaf,
                        Quantity = result.Quantity,
                        Price = result.Price,

                        IconUrl = isLeaf ? result.IconUrl : "",
                        ParentId = result.ParentId,
                        ProductFeatureId = result.Id,
                        ProductId = result.ProductId,
                        Description = result.Description,

                    });
                }

                /// Biz rule Calculate for greater than 2 level features
                /// To do code 
            }

            #endregion Extract quantity for detail ids

            #region Generate Master/Detail List
            var productFeatureTypeList = new List<VmProductFeatureType>();

            productFeatureTypeList =
              (from f in productFeatureTypeDetailList
               orderby f.FeatureTypePriority
               group new
               {
                   f.FeatureTypeDetailId,
                   f.FeatureTypeDetailName,
                   f.FeatureTypeDetailPriority,
                   f.IconUrl,
                   f.ParentId,
                   f.ProductId,
                   f.IsLeaf,
                   f.Quantity,
                   f.Price,
                   f.Description,

               }
               by new
               {
                   f.FeatureTypeId,
                   f.FeatureTypeName,
                   f.FeatureTypePriority
               } into g
               select new VmProductFeatureType
               {
                   Id = g.Key.FeatureTypeId,
                   Name = g.Key.FeatureTypeName,
                   Priority = g.Key.FeatureTypePriority,

                   ProductFeatureTypeDetailList = (from d in g.ToList()
                                                   select new VmProductFeatureTypeDetail
                                                   {
                                                       FeatureTypeId = g.Key.FeatureTypeId,
                                                       FeatureTypeDetailId = d.FeatureTypeDetailId,
                                                       FeatureTypeDetailName = d.FeatureTypeDetailName,
                                                       FeatureTypeDetailPriority = d.FeatureTypeDetailPriority,
                                                       IconUrl = d.IconUrl,
                                                       ParentId = d.ParentId,
                                                       ProductId = d.ProductId,
                                                       IsLeaf = d.IsLeaf,
                                                       Quantity = d.Quantity,
                                                       Price = d.Price,
                                                       Description = d.Description,

                                                   }).OrderBy(d => d.FeatureTypeDetailPriority).ToList()
               }).ToList();
            #endregion Generate Master/Detail List

            #endregion  Calulate feature type statistics

            #region Update Leaf Group

            var groupedSerchlist = from s in searchList
                                   group new
                                   {
                                       s.FeatureTypeId,
                                       s.FeatureTypeDetailId,
                                       s.FeatureTypeDetailName,
                                       s.FeatureTypeDetailPriority,
                                       s.IconUrl,
                                       s.ParentId,
                                       s.ProductId,
                                       s.Quantity,
                                       s.Price,
                                       s.Description,
                                   }
                                   by new
                                   {
                                       s.FeatureTypeDetailId
                                   } into g

                                   let last = g.OrderBy(i => i.IconUrl).Last()
                                   let detailInfo = StaticFeatureTypeList.Last().FeatureTypeDetailList
                                        .First(d => d.BaseFeatureTypeDetailId == last.FeatureTypeDetailId)
                                   select new VmProductFeatureTypeDetail
                                   {
                                       FeatureTypeId = last.FeatureTypeId,
                                       FeatureTypeDetailId = last.FeatureTypeDetailId,
                                       FeatureTypeDetailName = detailInfo.Name,
                                       FeatureTypeDetailPriority = detailInfo.Priority,
                                       IconUrl = last.IconUrl,
                                       ParentId = last.ParentId,
                                       ProductId = last.ProductId,
                                       IsLeaf = true,
                                       Quantity = last.Quantity,
                                       Price = last.Price,
                                       Description = last.Description,
                                       IsActive = (last.FeatureTypeDetailId == clientFeatureTypeDetailId),
                                   };

            productFeatureTypeList.Last().ProductFeatureTypeDetailList.Clear();

            productFeatureTypeList.Last().ProductFeatureTypeDetailList.AddRange(groupedSerchlist);
            #endregion Update Leaf Group

            var firstIconPriority = (clientFeatureTypeDetailId == null)
                ? staticCategoryFeatureType.FirstIconPriority.Value
                : StaticFeatureTypeList.Last().FeatureTypeDetailList
                        .First(d => d.BaseFeatureTypeDetailId == clientFeatureTypeDetailId.Value).Priority;

            if (clientFeatureTypeDetailId == null)
            {
                productFeatureTypeList.Last().ProductFeatureTypeDetailList.First().IsActive = true;
            }

            IEnumerable<ViewShopProduct> viewShopProductList =
                (from p in viewProductFeatureFullInfoList
                 where
                 p.FeatureTypeId == leafFeatureTypeId &&
                 p.FeatureTypeDetailPriority == firstIconPriority &&
                 p.ImageUrl != null
                 select new ViewShopProduct
                 {
                     Id = p.Id,
                     ParentId = p.ParentId,
                     ProductId = p.ProductId,
                     ProductName = p.ProductName,
                     FeatureTypeId = p.FeatureTypeId.Value,
                     FeatureTypeName = p.FeatureTypeName,
                     FeatureTypeDetailId = p.FeatureTypeDetailId,
                     FeatureTypeDetailName = p.FeatureTypeDetailName,
                     CategoryId = p.CategoryId,
                     CategoryName = p.CategoryName,
                     ProductDescription = p.ProductDescription,
                     Price = p.Price,
                     Quantity = p.Quantity,
                     ImageUrl = p.ImageUrl,
                     LinkUrl = p.LinkUrl,
                     ImagePriority = p.ImagePriority.Value,
                     ImageTitle = p.ImageTitle,
                     Description = p.Description,

                 }).ToList();

            var vmShopProductShopProductFullInfo =
                 (from spc in viewShopProductList
                  group new
                  {
                      spc.Id,
                      spc.ParentId,
                      spc.ProductId,
                      spc.FeatureTypeId,
                      spc.FeatureTypeName,
                      spc.FeatureTypeDetailId,
                      spc.FeatureTypeDetailName,
                      spc.Price,
                      spc.Quantity,
                      spc.ImageUrl,
                      spc.IconUrl,
                      spc.LinkUrl,
                      spc.ImagePriority,
                      spc.ImageTitle,
                      spc.Description,
                  }
                 by new
                 {
                     spc.CategoryId,
                     spc.CategoryName,
                     spc.ProductId,
                     spc.ProductName,
                     spc.ProductionDate,
                     spc.ExpiryDate,
                     spc.ProductDescription,

                 } into g
                  select new VmShopProductFullInfo
                  {
                      CategoryId = g.Key.CategoryId,
                      CategoryName = g.Key.CategoryName,
                      ProductId = g.Key.ProductId,
                      ProductName = g.Key.ProductName,
                      ProductDescription = g.Key.ProductDescription,
                      ProductionDate = g.Key.ProductionDate,
                      ExpiryDate = g.Key.ExpiryDate,

                      ProductFeatureTypeList = productFeatureTypeList.OrderByDescending(f => f.Priority).ToList(),

                      ShopProductList = (from p in g.ToList()
                                         select new VmShopProduct
                                         {
                                             Id = p.Id,
                                             ParentId = p.ParentId.Value,
                                             ProductId = p.ProductId,
                                             FeatureTypeId = p.FeatureTypeId,
                                             FeatureTypeName = p.FeatureTypeName,
                                             FeatureTypeDetailId = p.FeatureTypeDetailId.Value,
                                             FeatureTypeDetailName = p.FeatureTypeDetailName,
                                             Price = p.Price,
                                             Quantity = p.Quantity,
                                             ImageUrl = p.ImageUrl,
                                             LinkUrl = p.LinkUrl,
                                             ImagePriority = p.ImagePriority,
                                             ImageTitle = p.ImageTitle,
                                             Description = p.Description,

                                         }).ToList()
                  }).ToList();



            return vmShopProductShopProductFullInfo;
        }

        public Expression<Func<ViewShopProduct, bool>> GetShopProductExpression(VmFilterParameter filterParameter)
        {
            var minPrice = decimal.Parse(filterParameter.MinPrice);
            var maxPrice = decimal.Parse(filterParameter.MaxPrice);

            SpHandlerRepository spHandlerRepository = new SpHandlerRepository();

            if (filterParameter.FeatureTypes.Count() != 0)
            {
                string featureDetailCombinationList = string.Empty;
                string parentIdList = string.Empty;

                var featureTypeDetails = (from f in filterParameter.FeatureTypes
                                          from d in f.FeatureTypeDetailList
                                          select d.Id).ToList();

                Expression<Func<ViewShopProduct, bool>> predicateExpression = sp => sp.Price >= minPrice &&
                                                          sp.Price <= maxPrice &&
                                                          featureTypeDetails.Contains(sp.FeatureTypeDetailId.Value);

                return predicateExpression;
            }
            else
            {
                Expression<Func<ViewShopProduct, bool>> predicateExpression = sp => sp.Price >= minPrice &&
                                                         sp.Price <= maxPrice;

                return predicateExpression;
            }
        }
        public IEnumerable<VmProductIconList> GetShopProductsFeatureTypeDetailGroup(Expression<Func<ViewShopProduct, bool>> expression)
        {
            return viewShopProductRepository.GetShopProductsFeatureTypeDetailGroup(expression);

        }


        #region common methods
        public TreeNode GenerateInMemoryTree(List<VmFeatureType> vmFeatureTypeList, bool generateByBaseId = false)
        {

            int priority = 1;

            var newId = Guid.NewGuid();

            var parentIds = new List<Guid>
            {
                newId
            };

            List<VmProductFeature> productFeatureList = new List<VmProductFeature>
            {
                new VmProductFeature
                {
                    Id = newId,
                    ProductId = 0,
                    FeatureTypeId = 0,
                    FeatureTypeDetailId = null,
                    ParentId = Guid.Empty,
                    Price = null,
                    Quantity = null,
                    Priority = priority
                }
            };

            priority++;

            foreach (var featureType in vmFeatureTypeList.OrderBy(f => f.Priority))
            {
                parentIds = CreateProductFeaturesByFeatureType(ref priority, 0, parentIds,
                    featureType.FeatureTypeDetailList,
                    ref productFeatureList,
                    generateByBaseId);
            }

            var vmProductFeatureList = (from p in productFeatureList
                                        select new VmProductFeatureTree
                                        {
                                            Id = p.Id,
                                            ParentId = p.ParentId,
                                            ProductId = p.ProductId,
                                            FeatureTypeId = p.FeatureTypeId,
                                            FeatureTypeDetailId = p.FeatureTypeDetailId,
                                            Name = p.FeatureTypeDetailId.ToString(),
                                            Price = p.Price,
                                            Quantity = p.Quantity,
                                            AdditionalData = p.FeatureTypeId.ToString(),
                                            Priority = p.Priority,
                                            Showcase = p.Showcase,
                                            Description = p.Description,

                                        }).OrderBy(p => p.Priority).ToList();


            BLTreeModelTools blTreeModelTools = new BLTreeModelTools();
            var tree = blTreeModelTools.GetTreeModel(vmProductFeatureList, "");
            return tree;
        }
        public List<VmShopProduct> ConvertProductFeatureTreeToList(TreeNode tree, string delimiter)
        {
            BLTreeModelTools blTreeModelTools = new BLTreeModelTools();

            var paths = blTreeModelTools.ComputePaths(tree, n => n.children);

            var shopProductList = new List<VmShopProduct>();

            foreach (var item in paths)
            {

                var path = string.Empty;
                var leafNode = item.Last();

                var vmShopProduct = new VmShopProduct
                {
                    Id = Guid.Parse(leafNode.id),
                    ParentId = Guid.Parse(leafNode.ParentId),
                    FeatureTypeDetailName = leafNode.text,
                    FeatureTypeDetailId = int.Parse(leafNode.text),
                };

                foreach (var subItem in item)
                {
                    if (string.IsNullOrWhiteSpace(subItem.text) == false)
                    {
                        path += subItem.text + delimiter;

                    }
                }

                if (path.Length > 0)
                {
                    path = path.Substring(0, path.Length - delimiter.Length);
                }

                vmShopProduct.Path = path;

                shopProductList.Add(vmShopProduct);

            }

            return shopProductList;
        }
        public List<VmFeatureTypeDetail> ConvertFeatureTypeTreeToList(TreeNode tree, string delimiter)
        {
            BLTreeModelTools blTreeModelTools = new BLTreeModelTools();

            var paths = blTreeModelTools.ComputePaths(tree, n => n.children);

            var featureTypeDetailList = new List<VmFeatureTypeDetail>();

            foreach (var item in paths)
            {

                var path = string.Empty;
                var leafNode = item.Last();

                var featureTypeDetail = new VmFeatureTypeDetail
                {
                    Id = int.Parse(leafNode.text),
                    FeatureTypeId = int.Parse(leafNode.additionalData),
                    Name = leafNode.text,
                };

                foreach (var subItem in item)
                {
                    if (string.IsNullOrWhiteSpace(subItem.text) == false)
                    {
                        path += subItem.text + delimiter;
                    }
                }

                if (path.Length > 0)
                {
                    path = path.Substring(0, path.Length - delimiter.Length);
                }

                featureTypeDetail.Path = path;

                featureTypeDetailList.Add(featureTypeDetail);

            }

            return featureTypeDetailList;
        }

        #endregion common methods
    }
}