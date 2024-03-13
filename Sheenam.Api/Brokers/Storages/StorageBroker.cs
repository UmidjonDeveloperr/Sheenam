using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Drawing;

namespace Sheenam.Api.Brokers.Storages
{
	public partial class StorageBroker: EFxceptionsContext
	{
		private readonly IConfiguration _configuration;

		public StorageBroker(IConfiguration configuration)
		{
			_configuration = configuration;
			this.Database.Migrate();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			string connectionString = 
				this._configuration.GetConnectionString(name: "DefaultConnection");

			optionsBuilder.UseSqlServer(connectionString);
		}

		public override void Dispose()
		{
			
		}
	}
}
