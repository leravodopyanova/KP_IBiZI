﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KP
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PersonnelEntities1 : DbContext
    {
        public PersonnelEntities1()
            : base("name=PersonnelEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Challenger> Challengers { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Premium> Premiums { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
