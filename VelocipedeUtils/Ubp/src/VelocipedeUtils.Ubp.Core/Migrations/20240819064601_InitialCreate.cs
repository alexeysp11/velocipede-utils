using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VelocipedeUtils.Ubp.Core.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MobilePhone = table.Column<string>(type: "text", nullable: true),
                    HomePhone = table.Column<string>(type: "text", nullable: true),
                    WorkPhone = table.Column<string>(type: "text", nullable: true),
                    OfficePhone = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    OfficeEmail = table.Column<string>(type: "text", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContractType = table.Column<int>(type: "integer", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    MobilePhone = table.Column<string>(type: "text", nullable: true),
                    WorkPhone = table.Column<string>(type: "text", nullable: true),
                    Skype = table.Column<string>(type: "text", nullable: true),
                    ICQ = table.Column<string>(type: "text", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EmployDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReplacementMode = table.Column<int>(type: "integer", nullable: false),
                    AuthProviderType = table.Column<int>(type: "integer", nullable: false),
                    Locale = table.Column<string>(type: "text", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    HardDelete = table.Column<bool>(type: "boolean", nullable: false),
                    ParentItemId = table.Column<long>(type: "bigint", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationItems_OrganizationItems_ParentItemId",
                        column: x => x.ParentItemId,
                        principalTable: "OrganizationItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectPlanItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Priority = table.Column<int>(type: "integer", nullable: true),
                    ProjectPlanItemId = table.Column<long>(type: "bigint", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPlanItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectPlanItem_ProjectPlanItem_ProjectPlanItemId",
                        column: x => x.ProjectPlanItemId,
                        principalTable: "ProjectPlanItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Photo = table.Column<byte[]>(type: "bytea", nullable: true),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    LastSeenDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RegistrationNumber = table.Column<string>(type: "text", nullable: true),
                    VatNumber = table.Column<string>(type: "text", nullable: true),
                    CrmRoleType = table.Column<int>(type: "integer", nullable: false),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    ShippingAddress = table.Column<string>(type: "text", nullable: true),
                    HasVatRegistration = table.Column<bool>(type: "boolean", nullable: false),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Absenses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    Reason = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DateStartActual = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateEndActual = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateStartExpected = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateEndExpected = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Absenses_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContractEmployee",
                columns: table => new
                {
                    ContractsId = table.Column<long>(type: "bigint", nullable: false),
                    OurEmployeesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractEmployee", x => new { x.ContractsId, x.OurEmployeesId });
                    table.ForeignKey(
                        name: "FK_ContractEmployee_Contract_ContractsId",
                        column: x => x.ContractsId,
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractEmployee_Employees_OurEmployeesId",
                        column: x => x.OurEmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skill_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeOrganizationItem",
                columns: table => new
                {
                    EmployeesId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizationItemsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeOrganizationItem", x => new { x.EmployeesId, x.OrganizationItemsId });
                    table.ForeignKey(
                        name: "FK_EmployeeOrganizationItem_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeOrganizationItem_OrganizationItems_OrganizationItem~",
                        column: x => x.OrganizationItemsId,
                        principalTable: "OrganizationItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeUserAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    UserAccountId = table.Column<long>(type: "bigint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeUserAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeUserAccounts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeUserAccounts_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsGroupByDefault = table.Column<bool>(type: "boolean", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    AuthorCreatedId = table.Column<long>(type: "bigint", nullable: true),
                    AuthorChangedId = table.Column<long>(type: "bigint", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroups_UserAccounts_AuthorChangedId",
                        column: x => x.AuthorChangedId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserGroups_UserAccounts_AuthorCreatedId",
                        column: x => x.AuthorCreatedId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyContract",
                columns: table => new
                {
                    ContractsId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerCompaniesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyContract", x => new { x.ContractsId, x.CustomerCompaniesId });
                    table.ForeignKey(
                        name: "FK_CompanyContract_Companies_CustomerCompaniesId",
                        column: x => x.CustomerCompaniesId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyContract_Contract_ContractsId",
                        column: x => x.ContractsId,
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyEmployee",
                columns: table => new
                {
                    CompaniesId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyEmployee", x => new { x.CompaniesId, x.EmployeesId });
                    table.ForeignKey(
                        name: "FK_CompanyEmployee_Companies_CompaniesId",
                        column: x => x.CompaniesId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyEmployee_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    CrmRoleType = table.Column<int>(type: "integer", nullable: false),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    UserAccountId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customer_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customer_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    HeadItemId = table.Column<long>(type: "bigint", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Organizations_OrganizationItems_HeadItemId",
                        column: x => x.HeadItemId,
                        principalTable: "OrganizationItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserAccountGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserAccountId = table.Column<long>(type: "bigint", nullable: false),
                    UserGroupId = table.Column<long>(type: "bigint", nullable: false),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccountGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccountGroups_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccountGroups_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractCustomer",
                columns: table => new
                {
                    ContractsId = table.Column<long>(type: "bigint", nullable: false),
                    CustomersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractCustomer", x => new { x.ContractsId, x.CustomersId });
                    table.ForeignKey(
                        name: "FK_ContractCustomer_Contract_ContractsId",
                        column: x => x.ContractsId,
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractCustomer_Customer_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ContractId = table.Column<long>(type: "bigint", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    GuaranteePeriodInMonths = table.Column<int>(type: "integer", nullable: false),
                    ManagerId = table.Column<long>(type: "bigint", nullable: true),
                    CompletePercent = table.Column<int>(type: "integer", nullable: false),
                    DateStartActual = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateEndActual = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateStartExpected = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateEndExpected = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Project_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Project_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Project_Employees_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContractOrganization",
                columns: table => new
                {
                    ContractsId = table.Column<long>(type: "bigint", nullable: false),
                    OurOrganizationsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractOrganization", x => new { x.ContractsId, x.OurOrganizationsId });
                    table.ForeignKey(
                        name: "FK_ContractOrganization_Contract_ContractsId",
                        column: x => x.ContractsId,
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractOrganization_Organizations_OurOrganizationsId",
                        column: x => x.OurOrganizationsId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectPhase",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<long>(type: "bigint", nullable: true),
                    ProjectPlanItemId = table.Column<long>(type: "bigint", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPhase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectPhase_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectPhase_ProjectPlanItem_ProjectPlanItemId",
                        column: x => x.ProjectPlanItemId,
                        principalTable: "ProjectPlanItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Risk",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    AuthorCreatedId = table.Column<long>(type: "bigint", nullable: true),
                    AuthorChangedId = table.Column<long>(type: "bigint", nullable: true),
                    DateResolved = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AuthorResolvedId = table.Column<long>(type: "bigint", nullable: true),
                    CommentResolved = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: true),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Risk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Risk_Employees_AuthorChangedId",
                        column: x => x.AuthorChangedId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Risk_Employees_AuthorCreatedId",
                        column: x => x.AuthorCreatedId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Risk_Employees_AuthorResolvedId",
                        column: x => x.AuthorResolvedId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Risk_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absenses_EmployeeId",
                table: "Absenses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ContactId",
                table: "Companies",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyContract_CustomerCompaniesId",
                table: "CompanyContract",
                column: "CustomerCompaniesId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyEmployee_EmployeesId",
                table: "CompanyEmployee",
                column: "EmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractCustomer_CustomersId",
                table: "ContractCustomer",
                column: "CustomersId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractEmployee_OurEmployeesId",
                table: "ContractEmployee",
                column: "OurEmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractOrganization_OurOrganizationsId",
                table: "ContractOrganization",
                column: "OurOrganizationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CompanyId",
                table: "Customer",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ContactId",
                table: "Customer",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserAccountId",
                table: "Customer",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOrganizationItem_OrganizationItemsId",
                table: "EmployeeOrganizationItem",
                column: "OrganizationItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeUserAccounts_EmployeeId",
                table: "EmployeeUserAccounts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeUserAccounts_UserAccountId",
                table: "EmployeeUserAccounts",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationItems_ParentItemId",
                table: "OrganizationItems",
                column: "ParentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CompanyId",
                table: "Organizations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_HeadItemId",
                table: "Organizations",
                column: "HeadItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_CompanyId",
                table: "Project",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ContractId",
                table: "Project",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_CustomerId",
                table: "Project",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ManagerId",
                table: "Project",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPhase_ProjectId",
                table: "ProjectPhase",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPhase_ProjectPlanItemId",
                table: "ProjectPhase",
                column: "ProjectPlanItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPlanItem_ProjectPlanItemId",
                table: "ProjectPlanItem",
                column: "ProjectPlanItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Risk_AuthorChangedId",
                table: "Risk",
                column: "AuthorChangedId");

            migrationBuilder.CreateIndex(
                name: "IX_Risk_AuthorCreatedId",
                table: "Risk",
                column: "AuthorCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Risk_AuthorResolvedId",
                table: "Risk",
                column: "AuthorResolvedId");

            migrationBuilder.CreateIndex(
                name: "IX_Risk_ProjectId",
                table: "Risk",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_EmployeeId",
                table: "Skill",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountGroups_UserAccountId",
                table: "UserAccountGroups",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountGroups_UserGroupId",
                table: "UserAccountGroups",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_AuthorChangedId",
                table: "UserGroups",
                column: "AuthorChangedId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_AuthorCreatedId",
                table: "UserGroups",
                column: "AuthorCreatedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Absenses");

            migrationBuilder.DropTable(
                name: "CompanyContract");

            migrationBuilder.DropTable(
                name: "CompanyEmployee");

            migrationBuilder.DropTable(
                name: "ContractCustomer");

            migrationBuilder.DropTable(
                name: "ContractEmployee");

            migrationBuilder.DropTable(
                name: "ContractOrganization");

            migrationBuilder.DropTable(
                name: "EmployeeOrganizationItem");

            migrationBuilder.DropTable(
                name: "EmployeeUserAccounts");

            migrationBuilder.DropTable(
                name: "ProjectPhase");

            migrationBuilder.DropTable(
                name: "Risk");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "UserAccountGroups");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "ProjectPlanItem");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "OrganizationItems");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
