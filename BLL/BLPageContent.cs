using BLL.Base;
using Model;
using Model.ViewModels.PageContent;
using Repository.EF.Repository;

namespace BLL
{
    public class BLPageContent : BLBase
    {
        public BLPageContent(int currentLanguageId) : base(currentLanguageId)
        {
        }
        public VmPageContent GetByCurrentLanguageId(int id)
        {
            PageContentRepository PageContentRepository = UnitOfWork.GetRepository<PageContentRepository>();

            var result = PageContentRepository.GetByCurrentLanguageId(id);

            var vmPageContent = new VmPageContent
            {
                Id = result.Id,
                Content = result.Content,
                Subject = result.Subject,
                Type = result.Type.Value,
            };

            return vmPageContent;
        }
        public VmPageContent GetByLanguageId(int id, int languageId)
        {
            PageContentRepository PageContentRepository = UnitOfWork.GetRepository<PageContentRepository>();

            var result = PageContentRepository.GetByLanguageId(id, languageId);
            if (result != null)
            {
                var vmPageContent = new VmPageContent
                {
                    Id = result.Id,
                    Content = result.Content,
                    Subject = result.Subject,
                    Type = result.Type.Value,
                    LanguageId = result.LanguageId,
                };
                return vmPageContent;
            }
            return null;

        }
        public bool UpdatePageContent(VmPageContent pageContent)
        {
            PageContentRepository PageContentRepository = UnitOfWork.GetRepository<PageContentRepository>();

            var updateablePageContent = new PageContent
            {
                Id = pageContent.Id,
                Content = pageContent.Content,
                Subject = pageContent.Subject,
                Type = pageContent.Type,
                LanguageId = pageContent.LanguageId,
            };

            PageContentRepository.UpdatePageContent(updateablePageContent);

            UnitOfWork.Commit();

            return true;
        }
        public bool CreatePageContent(VmPageContent pageContent)
        {
            PageContentRepository PageContentRepository = UnitOfWork.GetRepository<PageContentRepository>();

            var newPageContent = new PageContent
            {
                Content = pageContent.Content,
                Subject = pageContent.Subject,
                Type = pageContent.Type,
                LanguageId = pageContent.LanguageId,

            };

            PageContentRepository.AddNewPageContent(newPageContent);

            UnitOfWork.Commit();

            return true;
        }
    }
}