using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Student_Management.Migrations;

public partial class HardenStudentSchemaAndAddUsers : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(name: "CreatedDate", table: "Students", newName: "CreatedAtUtc");
        migrationBuilder.AlterColumn<string>(name: "Name", table: "Students", type: "nvarchar(100)", maxLength: 100, nullable: false, oldClrType: typeof(string), oldType: "nvarchar(max)");
        migrationBuilder.AlterColumn<string>(name: "Email", table: "Students", type: "nvarchar(256)", maxLength: 256, nullable: false, oldClrType: typeof(string), oldType: "nvarchar(max)");
        migrationBuilder.AlterColumn<string>(name: "Course", table: "Students", type: "nvarchar(100)", maxLength: 100, nullable: false, oldClrType: typeof(string), oldType: "nvarchar(max)");
        migrationBuilder.AlterColumn<DateTime>(name: "CreatedAtUtc", table: "Students", type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()", oldClrType: typeof(DateTime), oldType: "datetime2");
        migrationBuilder.AddColumn<byte[]>(name: "RowVersion", table: "Students", type: "rowversion", rowVersion: true, nullable: false);
        migrationBuilder.AddCheckConstraint(name: "CK_Students_Age", table: "Students", sql: "[Age] BETWEEN 5 AND 120");
        migrationBuilder.CreateIndex(name: "IX_Students_Email", table: "Students", column: "Email", unique: true);

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
            },
            constraints: table => table.PrimaryKey("PK_Users", x => x.Id));
        migrationBuilder.CreateIndex(name: "IX_Users_Username", table: "Users", column: "Username", unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Users");
        migrationBuilder.DropCheckConstraint(name: "CK_Students_Age", table: "Students");
        migrationBuilder.DropIndex(name: "IX_Students_Email", table: "Students");
        migrationBuilder.DropColumn(name: "RowVersion", table: "Students");
        migrationBuilder.AlterColumn<string>(name: "Name", table: "Students", type: "nvarchar(max)", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(100)", oldMaxLength: 100);
        migrationBuilder.AlterColumn<string>(name: "Email", table: "Students", type: "nvarchar(max)", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(256)", oldMaxLength: 256);
        migrationBuilder.AlterColumn<string>(name: "Course", table: "Students", type: "nvarchar(max)", nullable: false, oldClrType: typeof(string), oldType: "nvarchar(100)", oldMaxLength: 100);
        migrationBuilder.AlterColumn<DateTime>(name: "CreatedAtUtc", table: "Students", type: "datetime2", nullable: false, oldClrType: typeof(DateTime), oldType: "datetime2", oldDefaultValueSql: "SYSUTCDATETIME()");
        migrationBuilder.RenameColumn(name: "CreatedAtUtc", table: "Students", newName: "CreatedDate");
    }
}
