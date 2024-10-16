﻿// <auto-generated />
using System;
using MTDemo.Sagas.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MTDemo.Sagas.Migrations
{
    [DbContext(typeof(SurveyImportSagaDbContext))]
    partial class SurveyImportSagaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MTDemo.Sagas.SagaStateMachines.SurveyImportSagaState", b =>
                {
                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("uuid");

                    b.Property<string>("CurrentState")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ImportEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ImportStartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea");

                    b.Property<DateTime?>("SurveyImportDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("SurveyPublishDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("CorrelationId");

                    b.ToTable("SurveyImportSagaState");
                });
#pragma warning restore 612, 618
        }
    }
}
