using BLL.Base;

using Model;
using Model.ViewModels.ProductImage;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLImage : BLBase
    {
        ImageRepository imageRepository;
        public BLImage(int currentLanguageId) : base(currentLanguageId)
        {
            imageRepository = UnitOfWork.GetRepository<ImageRepository>();
        }
        public bool BatchCreateImage(List<VmImage> vmImageList)
        {
            List<Image> newImageList = new List<Image>();

            foreach (var vmImage in vmImageList)
            {
                newImageList.Add(new Image
                {
                    ImageUrl = vmImage.ImageUrl,
                    LinkUrl = vmImage.LinkUrl,
                    Priority = vmImage.Priority,
                    ProductFeatureId = vmImage.ProducFeaturetId,
                    Title = vmImage.Title,
                });
            }

            imageRepository.BatchCreateImage(newImageList);

            return UnitOfWork.Commit();
        }
        public bool DeleteImageByProductFeature(Guid productFeatureId)
        {
            imageRepository.DeleteImageByProductFeature(productFeatureId);

            return UnitOfWork.Commit();
        }
    }
}
