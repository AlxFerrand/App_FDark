namespace App_FDark.Models
{
    public class ResourceText
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Text { get; set; }
        public int Status { get; set; }

        public ResourceText() { }
        public ResourceText(int id,string label, string text, int status)
        {
            Id = id; Label = label; Text = text; Status = status;
        }

    }
}
