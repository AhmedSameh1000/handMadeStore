using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandMadeStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class seedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUsers] ([Id], [Name], [City], [StreetAddress], [PostalCode], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'2cd35483-da6b-4cb4-b189-1a43728f071a', N'Ahmed Sameh', N'Alex', N'alex', N'45947', N'ahmeds141000@gmail.com', N'AHMEDS141000@GMAIL.COM', N'ahmeds141000@gmail.com', N'AHMEDS141000@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEJBZXn202I/L17AGemrfO22xasL/O8SfmJdpXMLX8pR8/qsy5UsQNQy2Ho1r9RTvlg==', N'7ACTHSYGH5SOFT43UQ6VYJXIUBQ4LGE2', N'7e75879b-70a5-41ef-a5a7-5abaa3d5830c', N'01092532838', 0, 0, NULL, 1, 0)\r\n");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUsers] WHERE Id = '2cd35483-da6b-4cb4-b189-1a43728f071a'");
        }
    }
}