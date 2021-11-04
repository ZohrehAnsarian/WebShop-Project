using Model;

using Repository.EF.Base;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ImageRepository : EFBaseRepository<Image>
    {
        public IEnumerable<Image> GetImagesByType(byte imageType)
        {

            var images = from image in Context.Images

                         select image;

            return images.OrderBy(i => i.Priority).ToArray();

        }

        public Image GetImageWithMaxPriority(Guid productFeatureId)
        {

            var image = (from img in Context.Images
                          where img.ProductFeatureId == productFeatureId
                          orderby img.Priority
                          select img).SingleOrDefault();

            return image;

        }
        public Image GetImagesById(int id)
        {

            var image = Context.Images.Find(id);

            return image;

        }
        public void CreateImage(Image newImage)
        {
            Add(newImage);
        }
        public void BatchCreateImage(List<Image> newImageList)
        {
            foreach (var item in newImageList)
            {
                Add(item);

            }
        }
        public void UpdateImage(Image image)
        {
            var oldImage = (from s in Context.Images where s.Id == image.Id select s).FirstOrDefault();

            oldImage.ImageUrl = image.ImageUrl;
            oldImage.LinkUrl = image.LinkUrl;
            oldImage.Priority = image.Priority;

            Update(oldImage);
        }
        public void UpdateImageFile(string imageUrl, int id)
        {
            var oldImage = (from s in Context.Images where s.Id == id select s).FirstOrDefault();

            oldImage.ImageUrl = imageUrl;
            Update(oldImage);
        }
        public void DeleteImageByProductFeature(Guid productFeatureId)
        {
            var oldImages = from s in Context.Images where s.ProductFeatureId == productFeatureId select s;
            foreach (var item in oldImages)
            {
                Delete(item);
            }
        }
        public IEnumerable<Guid> DeleteImageByProductFeature(int productId)
        {

            var productFeatureIds = (from s in Context.ProductFeatures
                                     where s.ProductId == productId
                                     select s.Id).ToArray();

            var oldImages = from s in Context.Images where productFeatureIds.Contains(s.ProductFeatureId.Value) select s;

            foreach (var item in oldImages)
            {
                Delete(item);
            }

            return productFeatureIds;
        }
        public void DeleteImage(int id)
        {
            var oldImage = (from s in Context.Images where s.Id == id select s).FirstOrDefault();

            Delete(oldImage);
        }
        public void UpdateImageWithoutFile(Image image)
        {
            var oldImage = (from s in Context.Images where s.Id == image.Id select s).FirstOrDefault();

            oldImage.Priority = image.Priority;
            oldImage.Title = image.Title;
            oldImage.LinkUrl = image.LinkUrl;
            Update(oldImage);
        }
    }
}
