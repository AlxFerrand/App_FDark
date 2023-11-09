namespace App_FDark.Models
{
    public class ContentType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ContentType() { }
        public ContentType(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
