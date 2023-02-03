using Ella.Core.Entity;
using Ella.DAL.DAL;

namespace EllaSuper.Service
{
    public class LayoutService
    {
        private readonly AppDbContext _AppDbContext;
        public LayoutService(AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }

        public List<Setting> GetSettings()
        {
            List<Setting> settings = _AppDbContext.Settings.ToList();
            return settings;
        }
    }
}
