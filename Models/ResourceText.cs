namespace App_FDark.Models
{
    public class ResourceText
    {
        public string Label { get; set; }
        public string Text { get; set; }

        public ResourceText() { }
        public ResourceText(string label, string text) {  Label = label; Text = text; }

    }
}
