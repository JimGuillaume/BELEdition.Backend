using FluentMigrator;

namespace Persistence.Migrations;

[Migration(202611071200)]
public class AjoutOeuvre : Migration
{
    public override void Up()
    {
        Create.Table("Oeuvres")
            .WithColumn("Id").AsString().PrimaryKey()
            .WithColumn("Titre").AsString(300).NotNullable()
            .WithColumn("Resume").AsString(int.MaxValue).Nullable()
            .WithColumn("CouverturePremiere").AsString(500).Nullable()
            .WithColumn("CouvertureQuatrieme").AsString(500).Nullable()
            .WithColumn("DateCreation").AsDateTime2().NotNullable()
            .WithColumn("DateModification").AsDateTime2().Nullable()
            .WithColumn("DateSuppression").AsDateTime2().Nullable()
            .WithColumn("Id_Autheur").AsString().NotNullable()
            .WithColumn("Id_Type").AsString().NotNullable()
            .WithColumn("Id_Status").AsString().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Oeuvres");
    }
}
