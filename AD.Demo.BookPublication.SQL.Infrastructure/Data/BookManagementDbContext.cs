using AD.Demo.BookPublication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SD.BuildingBlocks.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.SQL.Infrastructure.Data
{
    public class BookManagementDbContext : DbContext
    {
        public BookManagementDbContext(DbContextOptions<BookManagementDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                if (entityType.ClrType.IsSubclassOf(typeof(BaseEntity)))
                    SetTrackingColumnsMapping(modelBuilder, entityType.ClrType);
        }

        private void SetTrackingColumnsMapping(ModelBuilder modelBuilder, Type entityType)
        {
            modelBuilder.Entity(entityType).Property(nameof(BaseEntity.ID)).HasColumnName("ID").ValueGeneratedOnAdd();
            modelBuilder.Entity(entityType).Property(nameof(BaseEntity.CreatedDate)).HasColumnName("CREATED_DATE");
            modelBuilder.Entity(entityType).Property(nameof(BaseEntity.CreatedBy)).HasColumnName("CREATED_BY");
            modelBuilder.Entity(entityType).Property(nameof(BaseEntity.UpdatedBy)).HasColumnName("UPDATED_BY");
            modelBuilder.Entity(entityType).Property(nameof(BaseEntity.UpdatedDate)).HasColumnName("UPDATED_DATE");
        }
    }

}
