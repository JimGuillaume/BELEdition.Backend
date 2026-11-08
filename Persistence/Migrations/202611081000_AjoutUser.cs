using FluentMigrator;

namespace Persistence.Migrations;

[Migration(202611081000)]
public class AjoutUtilisateur : Migration
{
    public override void Up()
    {
        Create.Table("Utilisateurs")
            .WithColumn("Id").AsString().PrimaryKey()
            .WithColumn("Nom").AsString(100).NotNullable()
            .WithColumn("Prenom").AsString(100).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable().Unique()
            .WithColumn("MotDePasseHash").AsString(255).Nullable()
            .WithColumn("EidIdentifiantHash").AsString(255).Nullable()
            .WithColumn("EstActif").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("EstSuspendu").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("TentativesEchouees").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("VerrouilleJusqua").AsDateTime2().Nullable()
            .WithColumn("DateCreation").AsDateTime2().NotNullable()
            .WithColumn("DateSuppression").AsDateTime2().Nullable();

        //Execute.Sql("CREATE UNIQUE INDEX UX_Utilisateurs_EidIdentifiantHash ON Utilisateurs(EidIdentifiantHash) WHERE EidIdentifiantHash IS NOT NULL;");

        Execute.Sql("ALTER TABLE Utilisateurs ADD CONSTRAINT CK_Utilisateurs_Auth CHECK (MotDePasseHash IS NOT NULL OR EidIdentifiantHash IS NOT NULL);");
    }

    public override void Down() => Delete.Table("Utilisateurs");
}