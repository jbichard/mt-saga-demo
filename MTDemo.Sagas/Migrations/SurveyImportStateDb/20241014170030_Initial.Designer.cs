﻿// <auto-generated />
using System;
using MTDemo.Sagas.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MTDemo.Sagas.Migrations.SurveyImportStateDb
{
    [DbContext(typeof(SurveyImportStateDbContext))]
    [Migration("20241014170030_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MTDemo.Sagas.SurveyImport.QuestionImport", b =>
                {
                    b.Property<string>("QuestionId")
                        .HasColumnType("text");

                    b.Property<string>("Error")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsImported")
                        .HasColumnType("boolean");

                    b.Property<Guid>("SurveyImportId")
                        .HasColumnType("uuid");

                    b.HasKey("QuestionId");

                    b.HasIndex("SurveyImportId");

                    b.ToTable("QuestionImport");
                });

            modelBuilder.Entity("MTDemo.Sagas.SurveyImport.SurveyImport", b =>
                {
                    b.Property<Guid>("SurveyImportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("SurveyData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("SurveyImportId");

                    b.ToTable("SurveyImports");
                });

            modelBuilder.Entity("MTDemo.Sagas.SurveyImport.QuestionImport", b =>
                {
                    b.HasOne("MTDemo.Sagas.SurveyImport.SurveyImport", "SurveyImport")
                        .WithMany("Questions")
                        .HasForeignKey("SurveyImportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SurveyImport");
                });

            modelBuilder.Entity("MTDemo.Sagas.SurveyImport.SurveyImport", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}
