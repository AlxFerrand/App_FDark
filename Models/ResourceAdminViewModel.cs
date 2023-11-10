using System.Diagnostics.CodeAnalysis;

namespace App_FDark.Models
{
    public class ResourceAdminViewModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Picture { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public string DataType { get; set; }

        public ResourceAdminViewModel() { }
        public ResourceAdminViewModel(int id, string label, string picture, string url, string description, string content, string status, string dataType)
        {
            Id = id;
            Label = label;
            Picture = picture;
            Url = url;
            Description = description;
            Content = content;
            Status = status;
            DataType = dataType;
        }
    }
}
