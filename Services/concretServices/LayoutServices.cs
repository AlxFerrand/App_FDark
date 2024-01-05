using App_FDark.Data;
using App_FDark.Models;
using App_FDark.Services.abstractServices;
namespace App_FDark.Services.concretServices
{
    public class LayoutServices : ILayoutServices
    {
        private readonly ApplicationDbContext _context;
        private int ActualCatId;
        public LayoutServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Extension> ExtensionList()
        {
            return new List<Extension>(_context.Extension).Where(e=>e.Type.Equals("ext")).ToList();
        }

        public List<Extension> ActivityList()
        {
            return new List<Extension>(_context.Extension).Where(e => e.Type.Equals("act")).ToList();
        }

        public void SetActualCatId(int id)
        {
            ActualCatId = id;
        }
        public int GetActualCatId()
        {
            return ActualCatId;
        }

        public int[] NewsCounter() {
            int[] newsCounter = new int[3];
            newsCounter[0] = _context.Links.Where(l => l.Status == 1).ToList().Count();
            newsCounter[1] = _context.Users.Where(u => u.EmailConfirmed == false).ToList().Count();
            newsCounter[2] = _context.Links.Where(l => l.Status == 2).ToList().Count();
            return newsCounter;
        }
    }
}
