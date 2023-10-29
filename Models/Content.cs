using System.Diagnostics.CodeAnalysis;

namespace App_FDark.Models
{
    public class Content
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ExtensionId { get; set; }
        public int ContentTypeId { get; set; }
        [MaybeNull]
        public string Picture {  get; set; }

        public Content() { }
        public Content(int id, string name, int extensionId, int contentTypeId) 
        {
            Id = id;
            Name = name;
            ExtensionId = extensionId;
            ContentTypeId = contentTypeId;
        }
        public Content(int id, string name, int extensionId, int contentTypeId, string picture)
        {
            Id = id;
            Name = name;
            ExtensionId = extensionId;
            ContentTypeId = contentTypeId;
            Picture = picture;
        }
    }
}
