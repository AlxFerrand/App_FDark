namespace App_FDark.Models
{
    public class ResourceImage
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string[] Pictures { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public ResourceImage() { }

        public ResourceImage (int id,string label, string[] pictures, string description,int status)
        {
            Id = id;
            Label = label;
            Pictures = pictures;
            Description = description;
            Status = status;
        }
    }
}
