using System.Diagnostics.CodeAnalysis;

namespace App_FDark.Models
{
    public class Extension
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public Extension() { }
        public Extension(int id, string name,string type) 
        {
            Id = id;
            Name = name;
            Type = type;
        }

    }
}
