using Model;
using static Model.ApplicationDomainModels.ConstantObjects;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class SundryImageRepository : EFBaseRepository<SundryImage>
    {
        public IEnumerable<SundryImage> GetAllSundryImages(string searchText = "")
        {

            var sundryImages = from sundryImage in Context.SundryImages
                               where (
                               sundryImage.Title.Contains(searchText)
                               || sundryImage.LinkUrl.Contains(searchText)
                               || sundryImage.Type.ToString().Contains(searchText)
                               )
                               select sundryImage;

            return sundryImages.OrderBy(i => i.Priority).ToArray();

        }

        public IEnumerable<SundryImage> GetSundryImagesByType(SundryImageType sundryImageType, string searchText = "")
        {

            var sundryImages = from sundryImage in Context.SundryImages
                               where
                               sundryImage.Type == (int)sundryImageType &&
                               (
                                   sundryImage.Title.Contains(searchText)
                                   || sundryImage.LinkUrl.Contains(searchText)
                                   || sundryImage.Type.ToString().Contains(searchText)
                               )
                               select sundryImage;

            return sundryImages.OrderBy(i => i.Priority).ToArray();

        }
        public IEnumerable<SundryImage> GetMenuSundryImages()
        {

            var sundryImages = from sundryImage in Context.SundryImages
                               where
                               sundryImage.Type == (int)SundryImageType.PackageItem && sundryImage.ShowInMenu == true

                               select sundryImage;

            return sundryImages.OrderBy(i => i.Priority).ToArray();

        }

        public SundryImage GetSundryImagesById(int id)
        {

            var sundryImage = Context.SundryImages.Find(id);

            return sundryImage;

        }
        public void CreateSundryImage(SundryImage newSundryImage)
        {
            Add(newSundryImage);
        }
        public void UpdateSundryImage(SundryImage sundryImage)
        {
            var oldSundryImage = (from s in Context.SundryImages where s.Id == sundryImage.Id select s).FirstOrDefault();

            oldSundryImage.ImageUrl = sundryImage.ImageUrl;
            oldSundryImage.LinkUrl = sundryImage.LinkUrl;
            oldSundryImage.Priority = sundryImage.Priority;
            oldSundryImage.ObjectId = sundryImage.ObjectId;
            oldSundryImage.Type = sundryImage.Type;
            oldSundryImage.Title = sundryImage.Title;
            oldSundryImage.PackageItemType = sundryImage.PackageItemType;
            oldSundryImage.BannerImageUrl = sundryImage.BannerImageUrl;
            oldSundryImage.BannerImageTitle = sundryImage.BannerImageTitle;
            oldSundryImage.ShowInMenu = sundryImage.ShowInMenu;
            Update(oldSundryImage);
        }
        public void DeleteSundryImage(int id)
        {
            var oldSundryImage = (from s in Context.SundryImages where s.Id == id select s).FirstOrDefault();

            Delete(oldSundryImage);
        }
    }
}
