using System.Diagnostics.CodeAnalysis;

namespace App_FDark.Models
{
    public class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [MaybeNull]
        public string SubCatIds { get; set; }

        public Categories() { }
        public Categories(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public Categories(int id, string name, string subCatIds) 
        {
            Id = id;
            Name = name;
            SubCatIds = subCatIds;
        }

    }
}
