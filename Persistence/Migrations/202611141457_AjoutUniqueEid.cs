using FluentMigrator;

namespace Persistence.Migrations;

[Migration(202611141457)]
public class AjoutUniqueEid : Migration
{
    public override void Up()
    {
        Execute.Sql("UPDATE Utilisateurs SET EidIdentifiantHash = NULL WHERE MotDePasseHash IS NOT NULL;");
        
        Execute.Sql("""
            UPDATE Utilisateurs SET EstActif = 0, DateSuppression = COALESCE(DateSuppression, SYSUTCDATETIME())
            WHERE MotDePasseHash IS NULL;
            """);

        
        Execute.Sql("""
            CREATE UNIQUE INDEX UX_Utilisateurs_EidIdentifiantHash ON Utilisateurs(EidIdentifiantHash)
            WHERE EidIdentifiantHash IS NOT NULL AND DateSuppression IS NULL;
            """);
    }

    public override void Down() => Delete.Index("UX_Utilisateurs_EidIdentifiantHash").OnTable("Utilisateurs");
}