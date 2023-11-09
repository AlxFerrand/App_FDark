using System.Diagnostics.CodeAnalysis;

namespace App_FDark.Models
{
    public class Extension
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Extension() { }
        public Extension(int id, string name) 
        {
            Id = id;
            Name = name;
        }

    }
}
