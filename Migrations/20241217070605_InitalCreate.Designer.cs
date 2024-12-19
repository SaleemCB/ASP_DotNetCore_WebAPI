﻿// <auto-generated />
using System;
using ConceptZeeWebAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ConceptZeeWebAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241217070605_InitalCreate")]
    partial class InitalCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("ConceptZeeWebAPI.Models.CZeeContact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ContactDOB")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactRemarks")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });
#pragma warning restore 612, 618
        }
    }
}
