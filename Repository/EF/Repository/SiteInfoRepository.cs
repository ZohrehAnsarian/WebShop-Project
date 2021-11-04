using Model;
using Repository.EF.Base;

namespace Repository.EF.Repository
{
    public class SiteInfoRepository : EFBaseRepository<SiteInfo>
    {
        public void CreateSiteInfo(SiteInfo siteInfo)
        {
            Add(siteInfo);
        }
    }
}
