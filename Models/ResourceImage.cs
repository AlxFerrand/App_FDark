namespace App_FDark.Models
{
    public class ResourceImage
    {
        public string Label { get; set; }
        public string[] Pictures { get; set; }
        public string Description { get; set; }

        public ResourceImage() { }

        public ResourceImage (string label, string[] pictures, string description)
        {
            Label = label;
            Pictures = pictures;
            Description = description;
        }
    }
}
