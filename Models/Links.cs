using System.Diagnostics.CodeAnalysis;

namespace App_FDark.Models
{
    public class Links
    {
        public int Id { get; set; }
        public string Label { get; set; }
        [MaybeNull]
        public string Picture { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public int ContentId { get; set; }
        public int Status { get; set; }

        public Links() { }
        public Links(int id, string label, string url, string description, int contentId, int status)
        {
            Id = id;
            Label = label;
            Url = url;
            Description = description;
            ContentId = contentId;
            Status = status;
        }
        public Links(int id, string label,string picture, string url, string description, int contentId, int status)
        {
            Id = id;
            Label = label;
            Picture = picture;
            Url = url;
            Description = description;
            ContentId= contentId;
            Status = status;
        }
    }
}
