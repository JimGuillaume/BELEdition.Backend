using FluentMigrator;

namespace Persistence.Migrations;

[Migration(202611081900)]
public class AjoutDateModificationUtilisateur : Migration
{
    public override void Up() =>
        Alter.Table("Utilisateurs").AddColumn("DateModification").AsDateTime2().Nullable();

    public override void Down() =>
        Delete.Column("DateModification").FromTable("Utilisateurs");
}
