using Microsoft.EntityFrameworkCore;
using EventAssos.Domain.Entities;

namespace EventAssos.Infrastructure.DataBase.Context
{
    public class EventAssosContext : DbContext
    {
        public EventAssosContext(DbContextOptions options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        //public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventAssosContext).Assembly);
        }
    }
}




