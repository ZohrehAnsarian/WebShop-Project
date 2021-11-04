using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class NewsRepository : EFBaseRepository<News>
    {
        public IEnumerable<News> GetAllNews()
        {
            var NewsList = from s in Context.News
                           select s;

            return NewsList.OrderByDescending(A => A.Priority).ToArray();
        }
        public IEnumerable<News> GetNewsByType(int type)
        {
            var NewsList = from s in Context.News
                           where s.Type == type
                           select s;

            return NewsList.OrderByDescending(A => A.Priority).ToArray();
        }
        public News GetNewsById(int id)
        {
            var NewsList = from s in Context.News
                           where s.Id == id
                           select s;

            return NewsList.FirstOrDefault();
        }
        public IEnumerable<News> Select(News filterItem, int index, int count)
        {
            var newsList = from news in Context.News

                           select news;


            if (filterItem.HtmlBody != null)
            {
                newsList = newsList.Where(j => j.HtmlBody.Contains(filterItem.HtmlBody));
            }

            if (filterItem.Title != null)
            {
                newsList = newsList.Where(j => j.Title.Contains(filterItem.Title));
            }

            if (filterItem.Priority != null)
            {
                newsList = newsList.Where(j => j.Priority == filterItem.Priority);
            }

            return newsList.OrderByDescending(j => j.Priority).OrderBy(j => j.Priority).Skip(index).Take(count).ToArray();
        }
        public void UpdateNewsWithoutImage(News news)
        {
            var oldNews = (from s in Context.News where s.Id == news.Id select s).FirstOrDefault();

            oldNews.Priority = news.Priority;
            oldNews.HtmlBody = news.HtmlBody;
            oldNews.Body = news.Body;
            oldNews.Title = news.Title;
            oldNews.Type = news.Type;
            oldNews.PictureType = news.PictureType;

            Update(oldNews);
        }
        public void UpdateNewsImage(string ImageUrl, int id)
        {
            var oldNews = (from s in Context.News where s.Id == id select s).FirstOrDefault();

            oldNews.PictureContentUrl = ImageUrl;

            Update(oldNews);
        }
        public void CreateNews(News News)
        {
            Add(News);
        }

        public void UpdateNews(News news)
        {
            var oldNews = (from s in Context.News where s.Id == news.Id select s).FirstOrDefault();
            oldNews.Priority = news.Priority;
            oldNews.HtmlBody = news.HtmlBody;
            oldNews.Body = news.Body;
            oldNews.PictureContentUrl = news.PictureContentUrl;
            oldNews.Title = news.Title;
            oldNews.Type = news.Type;
            oldNews.PictureName = news.PictureName;
            oldNews.PictureType = news.PictureType;

            Update(oldNews);

        }

        public bool DeleteNews(int id)
        {
            var result = (from j in Context.News where j.Id == id select j).Count();
            if (result == 0)
            {
                var deleteable = Context.News.Find(id);
                Delete(deleteable);
                return true;
            }

            return false;

        }

    }
}
