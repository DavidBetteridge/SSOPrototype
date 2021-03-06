﻿using ProductB.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;

namespace ProductB
{
    public partial class ProductDB : DbContext
    {
        public ProductDB() : base()
        {
            Database.SetInitializer(new ProductDBInitializer());

        }
        public virtual DbSet<SSO_Application> SSO_Applications { get; set; }

        public virtual DbSet<SSO_ApplicationUser> SSO_ApplicationUsers { get; set; }

        public virtual DbSet<SSO_AuthorisationCode> SSO_AuthorisationCodes { get; set; }

        public virtual DbSet<SSO_AccessToken> SSO_AccessTokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<SSO_ApplicationUser>()
                .Property(t => t.SourceUserID)
                .HasMaxLength(255)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("ak_ApplicationUser", 1) { IsUnique = true }));

                modelBuilder
                    .Entity<SSO_ApplicationUser>()
                    .Property(t => t.SSO_ApplicationID)
                    .HasColumnAnnotation(
                        IndexAnnotation.AnnotationName,
                        new IndexAnnotation(
                            new IndexAttribute("ak_ApplicationUser", 2) { IsUnique = true }));
        }
    }

    public class ProductDBInitializer : DropCreateDatabaseAlways<ProductDB>
    {
        protected override void Seed(ProductDB context)
        {

            var applicationA = new SSO_Application()
            {
                ClientID = "client1",
                ClientSecret = "secret1",
                ID = 1,
                IsServer = true,
                Name = "Product A",
                ProductType = "A",
                URL = "http://localhost:53815/",
                ApplicationUsers = new Collection<SSO_ApplicationUser>()
            };

            //applicationA.ApplicationUsers.Add(new SSO_ApplicationUser()
            //{
            //    LocalUserID = "john.smith@proactis.com",
            //    SourceUserID = "david.betteridge@proactis.com"
            //});

            context.SSO_Applications.Add(applicationA);



            base.Seed(context);
        }
    }
}