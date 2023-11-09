namespace App_FDark.Models
{
    public class CatalogViewModel
    {
        public List<ResourceVideo> Videos { get; set; }
        public List<ResourceSite> Sites { get; set; }
        public List<ResourceImage> Images { get; set; }
        public List<ResourceText> Texts { get; set; }

        public CatalogViewModel() { }
        public CatalogViewModel(List<ResourceVideo> videos, List<ResourceSite> sites, List<ResourceImage> images, List<ResourceText> texts)
        {
            Videos = videos;
            Sites = sites;
            Images = images;
            Texts = texts;
        }
    }
}
