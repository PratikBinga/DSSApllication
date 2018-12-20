using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignature.Domain.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalSignature.Domain.Core.Data
{
    public class DigitalSignatureContext: DbContext

    {
        public DigitalSignatureContext(DbContextOptions<DigitalSignatureContext> options) : base(options)
        {
           
        }

        public DigitalSignatureContext()
        {

        }
        public DbSet<DigitalSignatureRecipient> DigitalSignatureRecipients { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //todo: do not deploy to uat/prod with this enabled
            //options.EnableSensitiveDataLogging();

           // options.UseLazyLoadingProxies();

            //options.ConfigureWarnings(w => w.Ignore(CoreEventId.DetachedLazyLoadingWarning));

            if (!options.IsConfigured)
            {
                options.UseSqlServer("Server=localhost;Database=DigitalSignatureDB;Trusted_Connection=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DigitalSignatureRecipient>(ConfigureDigitalSignatureRecipient);

        }

        private void ConfigureDigitalSignatureRecipient(EntityTypeBuilder<DigitalSignatureRecipient> builder)
        {

            builder.ToTable("Client_Envelope_Information");
            builder.Property(c => c.ClientAuthToken)
                .IsRequired();

            builder.Property(c => c.EnvelopeId)
                .IsRequired();
            builder.Property(c => c.RecipientEmail)
                .IsRequired();
            builder.Property(c => c.ReturnUrl)
                .IsRequired();
            builder.Property(c => c.SentOn)
               .HasDefaultValueSql("getdate()");
            builder.Property(c => c.UpdateOn)
               .HasDefaultValueSql("getdate()");

            builder.Property(c => c.Status);
                

        }
    }
}
