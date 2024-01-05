using App_FDark.Services.concretServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App_FDark.Models
{
    public class _LinksCreateViewModel
    {
        public int ContentID { get; set; }
        public SelectList ExtensionList { get; set; }
        public SelectList DataTypeList { get; set; }
        public Links NewLink { get; set; }
        public _LinksCreateViewModel() { }
        public _LinksCreateViewModel(List<Extension> extensionList)
        {
            ContentID = 0;
            ExtensionList = new SelectList(extensionList, "Id", "Name");
            DataTypeList = new SelectList(DataTypeDictionary.dataTypeDictionary.Values);
            NewLink = new Links();
        }
    }
}
