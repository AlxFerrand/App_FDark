using App_FDark.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace App_FDark.Services.abstractServices
{
    public interface IResourcesServices
    {
        public CatalogViewModel CreateCatalogViewModel(List<Links> linksList);
        public List<ResourceVideo> CreateVideoList(List<Links> linksList);
        public List<ResourceSite> CreateSiteList(List<Links> linksList);
        public List<ResourceImage> CreateImageList(List<Links> linksList);

        public List<ResourceAdminViewModel> CreateResourceAdminViewModel(string order,string dataTypeSort,int statusId,int extId,int contentId);
        public Links CreateNewRessource(string dataType, string Label, string Url, string Description, int contentId, List<IFormFile> files);
        public Object MakeSnapResource(int id, string DataType, string Label, string Url, string Description, List<IFormFile> files);
        public Links EditResource(Links link, List<IFormFile> files, string oldPicture);
        public void DeleteFileResource(int id);
    }
}
