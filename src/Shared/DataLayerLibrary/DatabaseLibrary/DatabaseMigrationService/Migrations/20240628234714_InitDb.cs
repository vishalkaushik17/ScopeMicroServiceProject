using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DBOperationsLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountConfirmations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    HostId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    ConfirmOnDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountConfirmations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Address1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Milestone = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Area = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Pincode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationHostMasters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Domain = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ConnectionString = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HashString = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    DatabaseType = table.Column<byte>(type: "smallint", nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationHostMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankMaster",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    TypeName = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTypes", x => x.Id);
                    table.UniqueConstraint("AK_CompanyTypes_TypeName", x => x.TypeName);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Symbol = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatabaseConnections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Catalog = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DbType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DataSourceServer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseConnections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DegreeMaster",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DegreeMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DemoRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Website = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ContactNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    City = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ReferenceCode = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsDemoActivated = table.Column<bool>(type: "boolean", nullable: false),
                    DemoActivatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsRestrictedForDemo = table.Column<bool>(type: "boolean", nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoRequests", x => x.Id);
                    table.UniqueConstraint("AK_DemoRequests_ContactNo", x => x.ContactNo);
                    table.UniqueConstraint("AK_DemoRequests_Email", x => x.Email);
                    table.UniqueConstraint("AK_DemoRequests_Name", x => x.Name);
                    table.UniqueConstraint("AK_DemoRequests_Website", x => x.Website);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentMaster",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailMasters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    EmailNotificationType = table.Column<byte>(type: "smallint", nullable: false),
                    ToEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FromEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CCEmail = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Subject = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Body = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                    ClientId = table.Column<string>(type: "character varying(540)", maxLength: 540, nullable: true),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NativeName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryAuthors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryAuthors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryBookCollections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Abbreviation = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryBookCollections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryMediaTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryMediaTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ProductName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Details = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CompanyBarCode = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    CustomBarCode = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    UnitPerBox = table.Column<double>(type: "double precision", nullable: false),
                    ProductImage = table.Column<byte[]>(type: "bytea", nullable: true),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolLibraries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolLibraries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SequenceMaster",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Prefix = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Suffix = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    AddYear = table.Column<bool>(type: "boolean", nullable: false),
                    AddMonth = table.Column<bool>(type: "boolean", nullable: false),
                    SequenceNo = table.Column<long>(type: "bigint", nullable: false),
                    IncrementBy = table.Column<byte>(type: "smallint", nullable: false),
                    DoRepeat = table.Column<bool>(type: "boolean", nullable: false),
                    SequenceBreakOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaxSequenceNo = table.Column<long>(type: "bigint", nullable: false),
                    SequenceLength = table.Column<byte>(type: "smallint", nullable: false),
                    TableName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SequenceMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemPreferences",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ModuleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PreferenceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DefaultValue = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    CustomValue = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false, defaultValue: "n/a"),
                    ValueType = table.Column<byte>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPreferences", x => x.Id);
                    table.UniqueConstraint("AK_SystemPreferences_PreferenceName_ModuleName", x => new { x.PreferenceName, x.ModuleName });
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    GSTNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CSTNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ContactPerson = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContactNo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    EmailId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Website = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Type = table.Column<byte>(type: "smallint", nullable: false),
                    AddressId = table.Column<string>(type: "character varying(450)", nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_VendorAddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyMasters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SuffixDomain = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Website = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DemoExpireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccountExpire = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompanyTypeId = table.Column<string>(type: "character varying(450)", nullable: false),
                    ReferenceCode = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ParentReferenceCode = table.Column<string>(type: "text", nullable: false),
                    IsDemoExpired = table.Column<bool>(type: "boolean", nullable: false),
                    IsDemoMode = table.Column<bool>(type: "boolean", nullable: false),
                    DemoRequestId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyMasters_CompanyTypes_CompanyTypeId",
                        column: x => x.CompanyTypeId,
                        principalTable: "CompanyTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DesignationMaster",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DepartmentId = table.Column<string>(type: "character varying(450)", nullable: false),
                    AllowedSeats = table.Column<byte>(type: "smallint", nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignationMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesignationMaster_DepartmentMaster_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DepartmentMaster",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LibraryRooms",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    LibraryId = table.Column<string>(type: "character varying(450)", nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryRooms_SchoolLibraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "SchoolLibraries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LibraryBookMasters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SubTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Snippet = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Pages = table.Column<int>(type: "integer", nullable: false),
                    BookImage = table.Column<string>(type: "text", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ISBN10 = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ISBN13 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    VolumeNo = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    LanguageId = table.Column<string>(type: "character varying(450)", nullable: false),
                    CurrencyId = table.Column<string>(type: "character varying(450)", nullable: false),
                    AuthorId = table.Column<string>(type: "character varying(450)", nullable: false),
                    PublisherId = table.Column<string>(type: "character varying(450)", nullable: false),
                    CollectionId = table.Column<string>(type: "character varying(450)", nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryBookMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryBookMasters_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryBookMasters_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryBookMasters_LibraryAuthors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "LibraryAuthors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryBookMasters_LibraryBookCollections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "LibraryBookCollections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryBookMasters_Vendors_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Vendors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppDbHostVsCompanyMasters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    AppHostId = table.Column<string>(type: "character varying(450)", nullable: false),
                    CompanyMasterId = table.Column<string>(type: "character varying(450)", nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDbHostVsCompanyMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppDbHostVsCompanyMasters_ApplicationHostMasters_AppHostId",
                        column: x => x.AppHostId,
                        principalTable: "ApplicationHostMasters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppDbHostVsCompanyMasters_CompanyMasters_CompanyMasterId",
                        column: x => x.CompanyMasterId,
                        principalTable: "CompanyMasters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: true),
                    CompanyId = table.Column<string>(type: "character varying(450)", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_CompanyMasters_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyMasters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PersonalEmailId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    AcceptTerms = table.Column<bool>(type: "boolean", nullable: false),
                    VerificationToken = table.Column<string>(type: "text", nullable: true),
                    Verified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResetToken = table.Column<string>(type: "text", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PasswordReset = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<string>(type: "character varying(450)", nullable: false),
                    CompanyTypeId = table.Column<string>(type: "character varying(450)", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_CompanyMasters_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyMasters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_CompanyTypes_CompanyTypeId",
                        column: x => x.CompanyTypeId,
                        principalTable: "CompanyTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyMasterProfiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NoOfStudents = table.Column<int>(type: "integer", nullable: false),
                    NoOfEmployees = table.Column<int>(type: "integer", nullable: false),
                    NoOfUsers = table.Column<int>(type: "integer", nullable: false),
                    DatabaseName = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    HostName = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    Username = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyMasterProfiles", x => x.Id);
                    table.UniqueConstraint("AK_CompanyMasterProfiles_Name", x => x.Name);
                    table.ForeignKey(
                        name: "FK_CompanyMasterProfiles_CompanyMasters_Id",
                        column: x => x.Id,
                        principalTable: "CompanyMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SequenceGenerators",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    TableName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    test = table.Column<string>(type: "text", nullable: false),
                    SequenceNo = table.Column<long>(type: "bigint", nullable: false),
                    Prefix = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Suffix = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    SequenceLength = table.Column<byte>(type: "smallint", nullable: false),
                    SequenceStartingNo = table.Column<int>(type: "integer", nullable: false),
                    SequenceEndingNo = table.Column<int>(type: "integer", nullable: false),
                    AddByNo = table.Column<byte>(type: "smallint", nullable: false),
                    AddYear = table.Column<bool>(type: "boolean", nullable: false),
                    AddMonth = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<string>(type: "character varying(450)", nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SequenceGenerators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SequenceGenerators_CompanyMasters_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyMasters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeStudentParentMaster",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    RecordType = table.Column<byte>(type: "smallint", nullable: false),
                    DesignationId = table.Column<string>(type: "character varying(450)", nullable: false),
                    BankId = table.Column<string>(type: "character varying(450)", nullable: false),
                    SchoolSpecificId = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    MotherName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContactNo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ProfessionalEmailId = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    PersonalEmailId = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    PanNo = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ElectionId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UIDNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PermanentAddressId = table.Column<string>(type: "character varying(450)", nullable: false),
                    CommunicationAddressId = table.Column<string>(type: "character varying(450)", nullable: false),
                    Gender = table.Column<byte>(type: "smallint", nullable: false),
                    MaritalStatus = table.Column<byte>(type: "smallint", nullable: false),
                    BloodGroup = table.Column<byte>(type: "smallint", nullable: false),
                    EmergencyContactNo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ResidentContactNo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Cast = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Religion = table.Column<byte>(type: "smallint", nullable: false),
                    WorkExperience = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Photograph = table.Column<string>(type: "text", nullable: false),
                    Achivements = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Ifsccode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MICRCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BranchName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BankState = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    BankCity = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Spouse = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SpouseContactNo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    PassportNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeStudentParentMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankId",
                        column: x => x.BankId,
                        principalTable: "BankMaster",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeStudentParentMaster_DesignationMaster_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "DesignationMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Fk_CommunicationAddressId",
                        column: x => x.CommunicationAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Fk_PermanentAddressId",
                        column: x => x.PermanentAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibrarySections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RoomId = table.Column<string>(type: "character varying(450)", nullable: false),
                    LibraryId = table.Column<string>(type: "character varying(450)", nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibrarySections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibrarySections_LibraryRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "LibraryRooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibrarySections_SchoolLibraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "SchoolLibraries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(450)", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    LoginProvider = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true),
                    Discriminator = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: true),
                    CompanyId = table.Column<string>(type: "character varying(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_CompanyMasters_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyMasters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByIp = table.Column<string>(type: "text", nullable: false),
                    Revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RevokedByIp = table.Column<string>(type: "text", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "text", nullable: true),
                    ReasonRevoked = table.Column<string>(type: "text", nullable: true),
                    CompanyId = table.Column<string>(type: "character varying(450)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "character varying(450)", nullable: true),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RefreshTokens_CompanyMasters_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompanyMasters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeQualifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    DegreeId = table.Column<string>(type: "character varying(450)", nullable: false),
                    MonthOfCompletion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    YearOfCompletion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Grade = table.Column<string>(type: "text", nullable: false),
                    EmployeeParentId = table.Column<string>(type: "character varying(450)", nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeQualifications", x => x.Id);
                    table.UniqueConstraint("AK_EmployeeQualifications_DegreeId_EmployeeParentId", x => new { x.DegreeId, x.EmployeeParentId });
                    table.ForeignKey(
                        name: "FK_DegreeId",
                        column: x => x.DegreeId,
                        principalTable: "DegreeMaster",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeStudentParentId",
                        column: x => x.EmployeeParentId,
                        principalTable: "EmployeeStudentParentMaster",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LibraryRacks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RoomId = table.Column<string>(type: "character varying(450)", nullable: false),
                    LibraryId = table.Column<string>(type: "character varying(450)", nullable: false),
                    SectionId = table.Column<string>(type: "character varying(450)", nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryRacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryRacks_LibraryRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "LibraryRooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryRacks_LibrarySections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "LibrarySections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryRacks_SchoolLibraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "SchoolLibraries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LibraryBookshelves",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    RoomId = table.Column<string>(type: "character varying(450)", nullable: false),
                    LibraryId = table.Column<string>(type: "character varying(450)", nullable: false),
                    SectionId = table.Column<string>(type: "character varying(450)", nullable: false),
                    RackId = table.Column<string>(type: "character varying(450)", nullable: false),
                    RecordStatus = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsEditable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryBookshelves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryBookshelves_LibraryRacks_RackId",
                        column: x => x.RackId,
                        principalTable: "LibraryRacks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryBookshelves_LibraryRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "LibraryRooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryBookshelves_LibrarySections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "LibrarySections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryBookshelves_SchoolLibraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "SchoolLibraries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppDbHostVsCompanyMasters_AppHostId",
                table: "AppDbHostVsCompanyMasters",
                column: "AppHostId");

            migrationBuilder.CreateIndex(
                name: "IX_AppDbHostVsCompanyMasters_CompanyMasterId",
                table: "AppDbHostVsCompanyMasters",
                column: "CompanyMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_CompanyId",
                table: "AspNetRoles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyTypeId",
                table: "AspNetUsers",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserTokens_CompanyId",
                table: "AspNetUserTokens",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyMasters_CompanyTypeId",
                table: "CompanyMasters",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyMasters_Name",
                table: "CompanyMasters",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DesignationMaster_DepartmentId",
                table: "DesignationMaster",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeQualifications_EmployeeParentId",
                table: "EmployeeQualifications",
                column: "EmployeeParentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStudentParentMaster_BankId",
                table: "EmployeeStudentParentMaster",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStudentParentMaster_CommunicationAddressId",
                table: "EmployeeStudentParentMaster",
                column: "CommunicationAddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStudentParentMaster_DesignationId",
                table: "EmployeeStudentParentMaster",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeStudentParentMaster_PermanentAddressId",
                table: "EmployeeStudentParentMaster",
                column: "PermanentAddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBookMasters_AuthorId",
                table: "LibraryBookMasters",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBookMasters_CollectionId",
                table: "LibraryBookMasters",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBookMasters_CurrencyId",
                table: "LibraryBookMasters",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBookMasters_LanguageId",
                table: "LibraryBookMasters",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBookMasters_PublisherId",
                table: "LibraryBookMasters",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBookshelves_LibraryId",
                table: "LibraryBookshelves",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBookshelves_RackId",
                table: "LibraryBookshelves",
                column: "RackId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBookshelves_RoomId",
                table: "LibraryBookshelves",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBookshelves_SectionId",
                table: "LibraryBookshelves",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryRacks_LibraryId",
                table: "LibraryRacks",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryRacks_RoomId",
                table: "LibraryRacks",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryRacks_SectionId",
                table: "LibraryRacks",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryRooms_LibraryId",
                table: "LibraryRooms",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_LibrarySections_LibraryId",
                table: "LibrarySections",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_LibrarySections_RoomId",
                table: "LibrarySections",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_ApplicationUserId",
                table: "RefreshTokens",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_CompanyId",
                table: "RefreshTokens",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SequenceGenerators_CompanyId",
                table: "SequenceGenerators",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_AddressId",
                table: "Vendors",
                column: "AddressId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountConfirmations");

            migrationBuilder.DropTable(
                name: "AppDbHostVsCompanyMasters");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CompanyMasterProfiles");

            migrationBuilder.DropTable(
                name: "DatabaseConnections");

            migrationBuilder.DropTable(
                name: "DemoRequests");

            migrationBuilder.DropTable(
                name: "EmailMasters");

            migrationBuilder.DropTable(
                name: "EmployeeQualifications");

            migrationBuilder.DropTable(
                name: "LibraryBookMasters");

            migrationBuilder.DropTable(
                name: "LibraryBookshelves");

            migrationBuilder.DropTable(
                name: "LibraryMediaTypes");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "SequenceGenerators");

            migrationBuilder.DropTable(
                name: "SequenceMaster");

            migrationBuilder.DropTable(
                name: "SystemPreferences");

            migrationBuilder.DropTable(
                name: "ApplicationHostMasters");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DegreeMaster");

            migrationBuilder.DropTable(
                name: "EmployeeStudentParentMaster");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "LibraryAuthors");

            migrationBuilder.DropTable(
                name: "LibraryBookCollections");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "LibraryRacks");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "BankMaster");

            migrationBuilder.DropTable(
                name: "DesignationMaster");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "LibrarySections");

            migrationBuilder.DropTable(
                name: "CompanyMasters");

            migrationBuilder.DropTable(
                name: "DepartmentMaster");

            migrationBuilder.DropTable(
                name: "LibraryRooms");

            migrationBuilder.DropTable(
                name: "CompanyTypes");

            migrationBuilder.DropTable(
                name: "SchoolLibraries");
        }
    }
}
