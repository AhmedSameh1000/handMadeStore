using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandMadeStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class seedAllRolesToAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUserRoles] (UserId, RoleId) SELECT '2cd35483-da6b-4cb4-b189-1a43728f071a', Id FROM [dbo].[AspNetRoles]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUserRoles] WHERE UserId = '2cd35483-da6b-4cb4-b189-1a43728f071a'");
        }
    }
}