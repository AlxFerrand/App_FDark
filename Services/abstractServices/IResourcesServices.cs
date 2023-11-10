using App_FDark.Models;

namespace App_FDark.Services.abstractServices
{
    public interface IResourcesServices
    {
        public CatalogViewModel CreateCatalogViewModel(List<Links> linksList);
        public List<ResourceAdminViewModel> CreateResourceAdminViewModel(List<Links> linksList);
    }
}
