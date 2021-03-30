using Microsoft.EntityFrameworkCore;

namespace Atom.Application.Web.Models
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    public class DatabaseContext : DbContext
    {
        /// <summary/>
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        /// <summary>
        /// Таблица "Книги"
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}