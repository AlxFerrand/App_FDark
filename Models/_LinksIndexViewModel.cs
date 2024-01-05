using App_FDark.Services.concretServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App_FDark.Models
{
    public class _LinksIndexViewModel
    {
        public List<ResourceAdminViewModel> Resources { get; set; }
        public SelectList DataTypeList { get; set; }
        public SelectList ExtensionList { get; set; }
        public SelectList StatusList { get; set; }
        public int ContentSelected {  get; set; }

        public int StatusSelected { get; set; }
        public string DataTypeSelected { get; set; }
        public int ExtSelected { get; set; }
        public string Order  {  get; set; }
        private List<KeyValuePair<int, string>> statusList = StatusDictionary.statusDictionary.ToList();

        public _LinksIndexViewModel(int statusSelected, string dataTypeSelected, int extSelected, List<Extension> extList,string order,int contentSelected) 
        { 
            Resources = new List<ResourceAdminViewModel>(); 
            ExtSelected = extSelected;
            StatusSelected = statusSelected;
            DataTypeSelected = dataTypeSelected;
            DataTypeList = new SelectList(DataTypeDictionary.dataTypeDictionary.Values, dataTypeSelected);
            StatusList = new SelectList(statusList, "Key", "Value", statusSelected);
            ExtensionList = new SelectList(extList, "Id", "Name", extSelected);
            Order = order;
            ContentSelected = contentSelected;
        }
    }
}
