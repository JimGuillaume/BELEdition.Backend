using Domain.Common;

namespace Domain.Entities;

public class User : AuditableEntity
{
    private const int MaxTentatives = 5;

    private User() { } // Dapper

    private User(string t_nom, string t_prenom, string t_email)
    {
        SetNom(t_nom);
        SetPrenom(t_prenom);
        SetEmail(t_email);
        EstActif = true;
        EstSuspendu = false;
        TentativesEchouees = 0;
        DateCreation = DateTime.UtcNow;
    }

    // --- factories ---
    public static User CreerAvecMotDePasse(string t_nom, string t_prenom, string t_email, string t_motDePasseHash)
    {
        var user = new User(t_nom, t_prenom, t_email);
        user.SetMotDePasseHash(t_motDePasseHash);
        return user;
    }

    public static User CreerAvecItsme(string t_nom, string t_prenom, string t_email, string t_itsmeSubHash)
    {
        var user = new User(t_nom, t_prenom, t_email);
        user.SetEid(t_itsmeSubHash);
        return user;
    }

    public string Nom { get; private set; } = null!;
    public string Prenom { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string? MotDePasseHash { get; private set; }   // null = itsme-only account
    public string? EidIdentifiantHash { get; private set; }   // null = password-only account
    public bool EstActif { get; private set; }
    public bool EstSuspendu { get; private set; }
    public int TentativesEchouees { get; private set; }
    public DateTime? VerrouilleJusqua { get; private set; }


    // -- commands --
    public void ModifierProfil(string t_nom, string t_prenom, string t_email, string? t_motDePasseHash, bool? EstActif, bool? EstSuspendu)
    {
        SetNom(t_nom);
        SetPrenom(t_prenom);
        SetEmail(t_email);
        if(t_motDePasseHash != null) SetMotDePasseHash(t_motDePasseHash);
        if(EstActif.HasValue) this.EstActif = EstActif.Value;
        if (EstSuspendu.HasValue) this.EstSuspendu = EstSuspendu.Value;
    }

    // --- password ---
    //public void UpdatePassword(string motDePasseHash) => SetMotDePasseHash(motDePasseHash);

    public void UpdateTentativesEchouees()
    {
        if (VerrouilleJusqua.HasValue && VerrouilleJusqua.Value > DateTime.UtcNow)
            throw new InvalidOperationException(
                $"Compte verrouillé jusqu'au {VerrouilleJusqua!.Value:dd/MM/yyyy HH:mm}.");

        TentativesEchouees++;
        if (TentativesEchouees >= MaxTentatives)
        {
            VerrouilleJusqua = DateTime.UtcNow.AddMinutes(15);
            TentativesEchouees = 0;
        }
    }

    public void ResetTentativesEchouees()
    {
        TentativesEchouees = 0;
        VerrouilleJusqua = null;
    }

    // --- itsme ---
    public void LierItsme(string itsmeSubHash)
    {
        if (EidIdentifiantHash != null)
            throw new InvalidOperationException("Un compte itsme est déjà lié.");
        SetEid(itsmeSubHash);
    }

    public void DelierItsme()
    {
        if (MotDePasseHash == null)
            throw new InvalidOperationException("Impossible de délier itsme : aucun mot de passe défini.");
        EidIdentifiantHash = null;
    }

    // --- account state ---
    public void Suspendre() => EstSuspendu = true;
    public void Reactiver() => EstSuspendu = false;

    public void Supprimer()
    {
        EstActif = false;
        DateSuppression = DateTime.UtcNow;
    }

    // --- private validation ---
    private void SetNom(string t_nom)
    {
        if (string.IsNullOrWhiteSpace(t_nom))
            throw new ArgumentException("Le nom est obligatoire.", nameof(t_nom));
        Nom = t_nom.Trim();
    }

    private void SetPrenom(string t_prenom)
    {
        if (string.IsNullOrWhiteSpace(t_prenom))
            throw new ArgumentException("Le prénom est obligatoire.", nameof(t_prenom));
        Prenom = t_prenom.Trim();
    }

    private void SetEmail(string t_email)
    {
        var checker = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
        if (string.IsNullOrWhiteSpace(t_email) || !checker.IsValid(t_email))
            throw new ArgumentException("L'adresse email n'est pas valide.", nameof(t_email));
        Email = t_email.Trim().ToLowerInvariant();
    }

    private void SetMotDePasseHash(string t_hash)
    {
        if (string.IsNullOrWhiteSpace(t_hash))
            throw new ArgumentException("Le hash du mot de passe est obligatoire.", nameof(t_hash));
        MotDePasseHash = t_hash;
    }

    private void SetEid(string t_eid)
    {
        if (string.IsNullOrWhiteSpace(t_eid))
            throw new ArgumentException("Identifiant itsme invalide.", nameof(t_eid));
        EidIdentifiantHash = t_eid;
    }
}