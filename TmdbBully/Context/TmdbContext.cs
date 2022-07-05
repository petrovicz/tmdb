using Microsoft.EntityFrameworkCore;
using TmdbBully.Models;

namespace TmdbBully.Context
{
	public partial class TmdbContext : DbContext
	{
		private readonly IConfiguration _configuration;

		public TmdbContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public virtual DbSet<Director> Directors { get; set; }
		public virtual DbSet<FetchProcess> FetchProcesses { get; set; }
		public virtual DbSet<Genre> Genres { get; set; }
		public virtual DbSet<Movie> Movies { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DbConnectionString"));
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
