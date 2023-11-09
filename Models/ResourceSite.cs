namespace App_FDark.Models
{
    public class ResourceSite
    {
        public string Label { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public ResourceSite() { }

        public ResourceSite(string label, string url, string picture, string description) 
        {
            Label = label;
            Url = url;
            Picture = picture;
            Description = description;
        }
    }
}
