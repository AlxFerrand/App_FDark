namespace App_FDark.Models
{
    public class _LinksCatalogViewModel
    {
        public List<ResourceVideo> Videos { get; set; }
        public List<ResourceSite> Sites { get; set; }
        public List<ResourceImage> Images { get; set; }
        public List<ResourceText> Texts { get; set; }
        public ContentType ContentTypeSelected { get; set; }
        public Extension ContentExtension {  get; set; }
        public Content ContentSelected { get; set; }


        public _LinksCatalogViewModel() 
        {
            Videos = new List<ResourceVideo>();
            Sites = new List<ResourceSite>();
            Images = new List<ResourceImage>();
            Texts = new List<ResourceText>();
            ContentTypeSelected = new ContentType();
            ContentExtension = new Extension();
            ContentSelected = new Content();
        }
        public _LinksCatalogViewModel(List<ResourceVideo> videos, List<ResourceSite> sites, List<ResourceImage> images, List<ResourceText> texts, ContentType contentTypeSelected, Extension contentExtension, Content contentSelected)
        {
            Videos = videos;
            Sites = sites;
            Images = images;
            Texts = texts;
            ContentTypeSelected = contentTypeSelected;
            ContentExtension = contentExtension;
            ContentSelected = contentSelected;
        }
    }
}
