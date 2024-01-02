namespace App_FDark.Models
{
    public class ResourceSite
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public ResourceSite() { }

        public ResourceSite(int id,string label, string url, string picture, string description,int status) 
        {
            Id = id;
            Label = label;
            Url = url;
            Picture = picture;
            Description = description;
            Status = status;
        }
    }
}
