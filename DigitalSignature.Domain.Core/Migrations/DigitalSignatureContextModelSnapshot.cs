﻿// <auto-generated />
using System;
using DigitalSignature.Domain.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DigitalSignature.Domain.Core.Migrations
{
    [DbContext(typeof(DigitalSignatureContext))]
    partial class DigitalSignatureContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DigitalSignature.Domain.Core.Model.DigitalSignatureRecipient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientAuthToken")
                        .IsRequired();

                    b.Property<string>("EnvelopeId")
                        .IsRequired();

                    b.Property<string>("RecipientEmail")
                        .IsRequired();

                    b.Property<string>("ReturnUrl")
                        .IsRequired();

                    b.Property<DateTime>("SentOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("Status");

                    b.Property<DateTime>("UpdateOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.ToTable("Client_Envelope_Information");
                });
#pragma warning restore 612, 618
        }
    }
}
