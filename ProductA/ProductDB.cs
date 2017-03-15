using ProductA.Models;
using System.Data.Entity;

namespace ProductA
{
    public partial class ProductDB : DbContext
    {
        public ProductDB() : base()
        {
            Database.SetInitializer(new ProductDBInitializer());

        }
        public virtual DbSet<SSO_Application> SSO_Applications { get; set; }

    }

    public class ProductDBInitializer : DropCreateDatabaseAlways<ProductDB>
    {
        protected override void Seed(ProductDB context)
        {

            var applicationB = new SSO_Application()
            {
                ClientID = "client1",
                ClientSecret = "secret1",
                ID = 1,
                IsServer = false,
                Name = "Product B",
                ProductType = "B",
                URL = "http://localhost:56418"

            };
            context.SSO_Applications.Add(applicationB);


            base.Seed(context);
        }
    }
}