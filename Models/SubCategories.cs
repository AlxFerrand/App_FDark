namespace App_FDark.Models
{
    public class SubCategories
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SubCategories() { }
        public SubCategories(int id, string name) 
        {
            Id = id;
            Name = name;
        }
    }
}
