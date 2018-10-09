using Microsoft.EntityFrameworkCore;
using System;

namespace ServiceGovernance.Registry.EntityFramework.Tests
{
    /// <summary>
    /// Database aware test base class
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    /// <typeparam name="TStoreOption">The type of the store option.</typeparam>
    public class DbAwareTests<TDbContext, TStoreOption> where TDbContext : DbContext
    {
        protected DbContextOptions<TDbContext> DbContextOptions { get; }
        protected TStoreOption StoreOptions { get; }

        protected DbAwareTests(string databaseName)
        {
            DbContextOptions = BuildInMemory<TDbContext>(databaseName);
            StoreOptions = Activator.CreateInstance<TStoreOption>();       
        }

        public static DbContextOptions<T> BuildInMemory<T>(string databaseName) where T : DbContext
        {
            var builder = new DbContextOptionsBuilder<T>();
            builder.UseInMemoryDatabase(databaseName);
            return builder.Options;
        }
    }
}
