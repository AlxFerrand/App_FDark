namespace App_FDark.Models
{
    public class _ContentCatalogViewModel
    {
        public List<Content> ContentsList { get; set; }
        public List<ContentType> ContentTypesList { get; set; }
        public int ContentTypeSelected { get; set; }
        public int ActualCatId { get; set; }

        public _ContentCatalogViewModel(List<Content> contentsList, List<ContentType> contentTypesList, int contentTypeSelected, int actualCatId)
        {
            ContentsList = contentsList;
            ContentTypesList = contentTypesList;
            ContentTypeSelected = contentTypeSelected;
            ActualCatId = actualCatId;
        }
        public _ContentCatalogViewModel() 
        { 
            ContentsList = new List<Content>();
            ContentTypesList = new List<ContentType>();
            ContentTypeSelected = 0;
            ActualCatId = 0;
        }
    }
}
