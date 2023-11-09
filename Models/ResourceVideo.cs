using System.Diagnostics.CodeAnalysis;

namespace App_FDark.Models
{
    public class ResourceVideo
    {
        public string Label { get; set; }
        public string Url { get; set; }
        public string VideoId { get; set; }
        public string Description { get; set; }

        public ResourceVideo() { }
        public ResourceVideo(string label, string url, string description, string videoId)
        {
            Label = label;
            Url = url;
            Description = description;
            VideoId = videoId;
        }

    }
}
