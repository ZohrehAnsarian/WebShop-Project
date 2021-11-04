using BLL.Base;
using Model;
using Model.ViewModels.News;
using Repository.EF.Repository;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLNews : BLBase
    {
        public IEnumerable<VmNews> GetAllNews()
        {
            var NewsRepository = UnitOfWork.GetRepository<NewsRepository>();

            var newsList = NewsRepository.GetAllNews();

            var VmNewsList = from m in newsList
                             select new VmNews
                             {
                                 Id = m.Id,
                                 Priority = m.Priority,
                                 Body = m.Body,
                                 HtmlBody = m.HtmlBody,
                                 PictureContentUrl = m.PictureContentUrl,
                                 PictureName = m.PictureName,
                                 PictureType = m.PictureType,
                                 Title = m.Title,
                                 Type = m.Type,
                             };

            return VmNewsList;
        }
        public IEnumerable<VmNews> GetNewsByType(int type)
        {
            var NewsRepository = UnitOfWork.GetRepository<NewsRepository>();

            var newsList = NewsRepository.GetNewsByType(type);

            var VmNewsList = from m in newsList
                             select new VmNews
                             {
                                 Id = m.Id,
                                 Priority = m.Priority,
                                 Body = m.Body,
                                 HtmlBody = m.HtmlBody,
                                 PictureContentUrl = m.PictureContentUrl,
                                 PictureName = m.PictureName,
                                 PictureType = m.PictureType,
                                 Title = m.Title,
                                 Type = m.Type,

                             };

            return VmNewsList;
        }
        public VmNewsDetail GetNewsById(int id)
        {
            var NewsRepository = UnitOfWork.GetRepository<NewsRepository>();

            var newsDetail = NewsRepository.GetNewsById(id);

            var VmNewsDetail = new VmNewsDetail
            {
                Id = newsDetail.Id,
                Priority = newsDetail.Priority,
                Body = newsDetail.Body,
                HtmlBody = newsDetail.HtmlBody,
                PictureContentUrl = newsDetail.PictureContentUrl,
                PictureName = newsDetail.PictureName,
                PictureType = newsDetail.PictureType,
                Title = newsDetail.Title,
                Type = newsDetail.Type,

            };

            return VmNewsDetail;
        }
        public IEnumerable<VmNews> GetNewsByFilter(VmNews filterItem)
        {
            var NewsRepository = UnitOfWork.GetRepository<NewsRepository>();

            var FilterItem = new News
            {
                Id = filterItem.Id,
                Priority = filterItem.Priority,
                HtmlBody = filterItem.HtmlBody,
                Title = filterItem.Title,
            };

            var newsList = NewsRepository.Select(FilterItem, 0, int.MaxValue);

            var vmNewsList = from news in newsList
                             select new VmNews
                             {
                                 Id = news.Id,
                                 Priority = news.Priority,
                                 Body = news.Body,
                                 HtmlBody = news.HtmlBody,
                                 PictureContentUrl = news.PictureContentUrl,
                                 PictureName = news.PictureName,
                                 PictureType = news.PictureType,
                                 Title = news.Title,
                                 Type = news.Type,
                             };
            return vmNewsList;
        }
        public bool UpdateNewsWithoutImage(VmNews vmNews)
        {
            try
            {
                var newsRepository = UnitOfWork.GetRepository<NewsRepository>();

                var updateableNews = new News
                {
                    Id = vmNews.Id,
                    Priority = vmNews.Priority,
                    Body = vmNews.Body,
                    HtmlBody = vmNews.HtmlBody,
                    PictureType = vmNews.PictureType,
                    Title = vmNews.Title,
                    Type = vmNews.Type,
                };

                newsRepository.UpdateNewsWithoutImage(updateableNews);

                UnitOfWork.Commit();

                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool UpdateNewsImage(string ImageUrl, int id)
        {
            try
            {
                var newsRepository = UnitOfWork.GetRepository<NewsRepository>();

                newsRepository.UpdateNewsImage(ImageUrl, id);

                UnitOfWork.Commit();

                return true;
            }
            catch
            {
                return false;
            }

        }
        public int CreateNewsImage(string ImageUrl)
        {
            try
            {
                var newsRepository = UnitOfWork.GetRepository<NewsRepository>();


                var newNews = new News()
                {
                    PictureContentUrl = ImageUrl,

                };

                newsRepository.CreateNews(newNews);

                UnitOfWork.Commit();

                return newNews.Id;
            }
            catch
            {
                return -1;
            }

        }
        public int CreateNews(VmNews vmNews)
        {
            NewsRepository NewsRepository = UnitOfWork.GetRepository<NewsRepository>();

            var newNews = new News()
            {
                Priority = vmNews.Priority,
                Body = vmNews.Body,
                HtmlBody = vmNews.HtmlBody,
                PictureContentUrl = vmNews.PictureContentUrl,
                PictureName = vmNews.PictureName,
                PictureType = vmNews.PictureType,
                Title = vmNews.Title,
                Type = vmNews.Type,
            };

            NewsRepository.CreateNews(newNews);

            UnitOfWork.Commit();

            return newNews.Id;
        }
        public bool UpdateNews(VmNews vmNews)
        {
            try
            {
                NewsRepository NewsRepository = UnitOfWork.GetRepository<NewsRepository>();

                var updateableNews = new News()
                {
                    Id = vmNews.Id,
                    Priority = vmNews.Priority,
                    Body = vmNews.Body,
                    HtmlBody = vmNews.HtmlBody,
                    PictureContentUrl = vmNews.PictureContentUrl,
                    PictureName = vmNews.PictureName,
                    PictureType = vmNews.PictureType,
                    Title = vmNews.Title,
                    Type = vmNews.Type,
                };

                NewsRepository.UpdateNews(updateableNews);

                UnitOfWork.Commit();

                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool DeleteNews(int id)
        {
            bool result = true;
            try
            {
                NewsRepository NewsRepository = UnitOfWork.GetRepository<NewsRepository>();

                if (NewsRepository.DeleteNews(id) == true)
                {
                    UnitOfWork.Commit();
                }
            }
            catch
            {
                result = false;
            }

            return result;

        }

    }
}