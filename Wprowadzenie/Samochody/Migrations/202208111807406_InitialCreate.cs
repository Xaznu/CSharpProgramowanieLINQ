namespace Samochody.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Samochods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rok = c.Int(nullable: false),
                        Producent = c.String(),
                        Model = c.String(),
                        Pojemnosc = c.Double(nullable: false),
                        IloscCylindrow = c.Int(nullable: false),
                        SpalanieMiasto = c.Int(nullable: false),
                        SpalanieAutostrada = c.Int(nullable: false),
                        SpalanieMieszane = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Samochods");
        }
    }
}
