using App_FDark.Models;

namespace App_FDark.Services.abstractServices
{
    public interface ILayoutServices
    {
        public List<Extension> ExtensionList();
        public List<Extension> ActivityList();

        public void SetActualCatId(int id);
        public int GetActualCatId();
        public int[] NewsCounter();
    }
}
