using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;

#nullable disable

namespace LMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class seedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string adminId = Guid.NewGuid().ToString();

            var hasher = new PasswordHasher<IdentityUser>();

        migrationBuilder.InsertData(
         table: "AspNetUsers",
         columns: new[]
         {
                "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed",
                "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed",
                "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount", "ProfileImg", "Name"
         },
         values: new object[]
         {
                adminId, "admin@admin.com", "admin@admin.com".ToUpper(), "admin@admin.com", "admin@admin.com".ToUpper(), true,
                hasher.HashPassword(null, "Admin@123"), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),null
                  ,false,false,null,true,0,null , "Super Admin",
         }
     );

            migrationBuilder.InsertData(
                   table: "AspNetUserRoles",
                   columns: new[] { "UserId", "RoleId" },
                   values: new object[] { adminId, "9685419b-f9fd-4f36-9783-5f7f0302c5c3" }
               );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM AspNetUserRoles WHERE UserId IN (SELECT Id FROM AspNetUsers WHERE UserName = 'admin@admin.com')");
            migrationBuilder.Sql("DELETE FROM AspNetUsers WHERE UserName = 'admin@admin.com'");


        }
    }
}
