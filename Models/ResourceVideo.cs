using System.Diagnostics.CodeAnalysis;

namespace App_FDark.Models
{
    public class ResourceVideo
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }
        public string VideoId { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public ResourceVideo() { }
        public ResourceVideo(int id,string label, string url, string description, string videoId, int status)
        {
            Id = id;
            Label = label;
            Url = url;
            Description = description;
            VideoId = videoId;
            Status = status;
        }

    }
}
