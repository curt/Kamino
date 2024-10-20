﻿// <auto-generated />
using System;
using Kamino.Shared.Entities;
using Kamino.Shared.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Kamino.Shared.Migrations
{
    [DbContext(typeof(NpgsqlContext))]
    [Migration("20241008141021_PingRelationships")]
    partial class PingRelationships
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "post_type", new[] { "none", "article", "note", "review", "check_in", "event" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "source_type", new[] { "none", "markdown", "plain", "html" });
            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "citext");
            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Kamino.Shared.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Kamino.Shared.Entities.Follow", b =>
                {
                    b.Property<string>("Uri")
                        .HasColumnType("text");

                    b.Property<string>("AcceptUri")
                        .HasColumnType("text");

                    b.Property<string>("ActorUri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ObjectUri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Uri");

                    b.HasAlternateKey("ActorUri", "ObjectUri");

                    b.ToTable("Follows");
                });

            modelBuilder.Entity("Kamino.Shared.Entities.Like", b =>
                {
                    b.Property<string>("Uri")
                        .HasColumnType("text");

                    b.Property<string>("ActorUri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ObjectUri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Uri");

                    b.HasAlternateKey("ActorUri", "ObjectUri");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Kamino.Shared.Entities.Ping", b =>
                {
                    b.Property<string>("Uri")
                        .HasColumnType("text");

                    b.Property<string>("ActorUri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ToUri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Uri");

                    b.HasIndex("ActorUri");

                    b.HasIndex("ToUri");

                    b.ToTable("Pings");
                });

            modelBuilder.Entity("Kamino.Shared.Entities.Place", b =>
                {
                    b.Property<string>("Uri")
                        .HasColumnType("text");

                    b.Property<string>("AuthorUri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.Property<Point>("Location")
                        .HasColumnType("geography (point)");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Slug")
                        .HasColumnType("text");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<SourceType>("SourceType")
                        .HasColumnType("source_type");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<DateTime?>("TombstonedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Uri");

                    b.HasIndex("AuthorUri");

                    b.ToTable("Places");
                });

            modelBuilder.Entity("Kamino.Shared.Entities.Pong", b =>
                {
                    b.Property<string>("Uri")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PingUri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Uri");

                    b.HasIndex("PingUri");

                    b.ToTable("Pongs");
                });

            modelBuilder.Entity("Kamino.Shared.Entities.Post", b =>
                {
                    b.Property<string>("Uri")
                        .HasColumnType("text");

                    b.Property<string>("AuthorUri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("CachedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ContextUri")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("EditedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("EndsAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InReplyToUri")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<PostType>("PostType")
                        .HasColumnType("post_type");

                    b.Property<DateTime?>("PublishedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Slug")
                        .HasColumnType("text");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<SourceType>("SourceType")
                        .HasColumnType("source_type");

                    b.Property<DateTime?>("StartsAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<DateTime?>("TombstonedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Uri");

                    b.HasIndex("AuthorUri");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Kamino.Shared.Entities.Profile", b =>
                {
                    b.Property<string>("Uri")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CachedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("CreatedAt")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text");

                    b.Property<string>("Inbox")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PrivateKey")
                        .HasColumnType("text");

                    b.Property<string>("PublicKey")
                        .HasColumnType("text");

                    b.Property<string>("PublicKeyId")
                        .HasColumnType("text");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.Property<DateTime?>("TombstonedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Uri");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("Kamino.Shared.Entities.Tag", b =>
                {
                    b.Property<string>("NormalizedTitle")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Slug")
                        .HasColumnType("text");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<SourceType>("SourceType")
                        .HasColumnType("source_type");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("NormalizedTitle");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("PlacesTags", b =>
                {
                    b.Property<string>("PlacesUri")
                        .HasColumnType("text");

                    b.Property<string>("TagsNormalizedTitle")
                        .HasColumnType("text");

                    b.HasKey("PlacesUri", "TagsNormalizedTitle");

                    b.HasIndex("TagsNormalizedTitle");

                    b.ToTable("PlacesTags");
                });

            modelBuilder.Entity("PostsPlaces", b =>
                {
                    b.Property<string>("PlacesUri")
                        .HasColumnType("text");

                    b.Property<string>("PostsUri")
                        .HasColumnType("text");

                    b.HasKey("PlacesUri", "PostsUri");

                    b.HasIndex("PostsUri");

                    b.ToTable("PostsPlaces");
                });

            modelBuilder.Entity("PostsTags", b =>
                {
                    b.Property<string>("PostsUri")
                        .HasColumnType("text");

                    b.Property<string>("TagsNormalizedTitle")
                        .HasColumnType("text");

                    b.HasKey("PostsUri", "TagsNormalizedTitle");

                    b.HasIndex("TagsNormalizedTitle");

                    b.ToTable("PostsTags");
                });

            modelBuilder.Entity("Kamino.Shared.Entities.Ping", b =>
                {
                    b.HasOne("Kamino.Shared.Entities.Profile", "Actor")
                        .WithMany("PingsActor")
                        .HasForeignKey("ActorUri")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kamino.Shared.Entities.Profile", "To")
                        .WithMany("PingsTo")
                        .HasForeignKey("ToUri")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Actor");

                    b.Navigation("To");
                });

            modelBuilder.Entity("Kamino.Shared.Entities.Place", b =>
                {
                    b.HasOne("Kamino.Shared.Entities.Profile", "Author")
                        .WithMany("PlacesAuthored")
                        .HasForeignKey("AuthorUri")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Kamino.Shared.Entities.Pong", b =>
                {
                    b.HasOne("Kamino.Shared.Entities.Ping", "Ping")
                        .WithMany("Pongs")
                        .HasForeignKey("PingUri")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ping");
                });

            modelBuilder.Entity("Kamino.Shared.Entities.Post", b =>
                {
                    b.HasOne("Kamino.Shared.Entities.Profile", "Author")
                        .WithMany("PostsAuthored")
                        .HasForeignKey("AuthorUri")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Kamino.Shared.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Kamino.Shared.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kamino.Shared.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Kamino.Shared.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlacesTags", b =>
                {
                    b.HasOne("Kamino.Shared.Entities.Place", null)
                        .WithMany()
                        .HasForeignKey("PlacesUri")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kamino.Shared.Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsNormalizedTitle")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PostsPlaces", b =>
                {
                    b.HasOne("Kamino.Shared.Entities.Place", null)
                        .WithMany()
                        .HasForeignKey("PlacesUri")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kamino.Shared.Entities.Post", null)
                        .WithMany()
                        .HasForeignKey("PostsUri")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PostsTags", b =>
                {
                    b.HasOne("Kamino.Shared.Entities.Post", null)
                        .WithMany()
                        .HasForeignKey("PostsUri")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kamino.Shared.Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsNormalizedTitle")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Kamino.Shared.Entities.Ping", b =>
                {
                    b.Navigation("Pongs");
                });

            modelBuilder.Entity("Kamino.Shared.Entities.Profile", b =>
                {
                    b.Navigation("PingsActor");

                    b.Navigation("PingsTo");

                    b.Navigation("PlacesAuthored");

                    b.Navigation("PostsAuthored");
                });
#pragma warning restore 612, 618
        }
    }
}
