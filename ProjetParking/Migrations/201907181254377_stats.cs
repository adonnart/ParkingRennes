namespace ProjetParking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stats : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ParkingStats",
                c => new
                    {
                        ParkingStatId = c.Int(nullable: false, identity: true),
                        ParkingName = c.String(),
                        NbPlacesLibres = c.Int(nullable: false),
                        NbPlacesTotal = c.Int(nullable: false),
                        StatDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ParkingStatId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ParkingStats");
        }
    }
}
