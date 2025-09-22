using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class nlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainLogTable",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    EnteredDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GETDATE()"),
                    LogDate = table.Column<string>(nullable: true),
                    LogLevel = table.Column<string>(nullable: true),
                    LogLogger = table.Column<string>(nullable: true),
                    LogMessage = table.Column<string>(nullable: true),
                    LogException = table.Column<string>(nullable: true),
                    LogStacktrace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainLogTable", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MainLogTable_EnteredDate",
                table: "MainLogTable",
                column: "EnteredDate");




            //migrationBuilder.Sql(@"CREATE TRIGGER [MainLogTable_UPDATE] ON [dbo].[MainLogTable]
            //AFTER UPDATE
            //AS
            //    BEGIN
            //SET NOCOUNT ON;

            //IF((SELECT TRIGGER_NESTLEVEL()) > 1) RETURN;

            //DECLARE @Id uniqueidentifier

            //SELECT @Id = INSERTED.Id
            //FROM INSERTED

            //UPDATE dbo.MainLogTable
            //SET EnteredDate = GETDATE()
            //WHERE Id = @Id
            //END");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainLogTable");


            //migrationBuilder.Sql(@"
            //DROP TRIGGER  IF EXISTS  [MainLogTable_UPDATE]   
            //ON ALL SERVER");
        }
    }
}
