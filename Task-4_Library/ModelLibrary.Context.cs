namespace Task_4_Library
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class LibraryEntities : DbContext
    {
        public LibraryEntities()
            : base("name=LibraryEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<Reader> Reader { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Register> Register { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}