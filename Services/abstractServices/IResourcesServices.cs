using App_FDark.Models;
using System.ComponentModel.DataAnnotations;

namespace App_FDark.Services.abstractServices
{
    public interface IResourcesServices
    {
        public CatalogViewModel CreateCatalogViewModel(List<Links> linksList);
        public List<ResourceVideo> CreateVideoList(List<Links> linksList);
        public List<ResourceSite> CreateSiteList(List<Links> linksList);
        public List<ResourceImage> CreateImageList(List<Links> linksList);

        public List<ResourceAdminViewModel> CreateResourceAdminViewModel(string order,string dataTypeSort,int statusId,int extId,int contentId);
    }
}
