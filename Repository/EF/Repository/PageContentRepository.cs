using Model;
using Repository.EF.Base;
using System.Linq;

namespace Repository.EF.Repository
{
    public class PageContentRepository : EFBaseRepository<PageContent>
    {

        public PageContent GetByLanguageId(int type, int languageId)
        {
            var pageContent = from p in Context.PageContents
                              where p.Type == type && p.LanguageId == languageId
                              select p;


            return pageContent.SingleOrDefault();
        }
        public PageContent GetByCurrentLanguageId(int type)
        {
            var pageContent = from p in Context.PageContents
                              where p.Type == type && p.LanguageId == CurrentLanguageId
                              select p;


            return pageContent.FirstOrDefault();
        }

        public void AddNewPageContent(PageContent PageContent)
        {
            Add(PageContent);
        }

        public void UpdatePageContent(PageContent PageContent)
        {
            var oldPageContent = (from s in Context.PageContents
                                  where s.Type == PageContent.Type && s.LanguageId == PageContent.LanguageId
                                  select s).FirstOrDefault();

            oldPageContent.Content = PageContent.Content;
            oldPageContent.Subject = PageContent.Subject;

            Update(oldPageContent);
        }
    }
}
