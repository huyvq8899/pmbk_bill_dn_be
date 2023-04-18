using DLL.Data;
using DLL.Entity.Config;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DLL
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly Datacontext _context;

        public DatabaseInitializer(Datacontext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            if (!_context.ThietLapTruongDuLieus.Any())
            {
                var data = new ThietLapTruongDuLieuData().InitData();
                await _context.ThietLapTruongDuLieus.AddRangeAsync(data);
            }

            await _context.SaveChangesAsync();
        }
    }
}
