using Domain.Common;

namespace Domain.Entities;

public class User : BaseEntity
{
    private const int MaxTentatives = 5;

    private User() { } // EF Core

    private User(string t_nom, string t_prenom, string t_email)
    {
        if(eid_identifiant_hash == null && mot_de_passe_hash == null)
            throw new ArgumentException("Un utilisateur doit avoir soit un mot de passe, soit un identifiant itsme.");
        SetNom(t_nom);
        SetPrenom(t_prenom);
        SetEmail(t_email);
        est_actif = true;
        est_suspendu = false;
        tentatives_echouees = 0;
        date_inscription = DateTime.UtcNow;
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

    public int id_utilisateur { get; private set; }
    public string nom { get; private set; } = null!;
    public string prenom { get; private set; } = null!;
    public string email { get; private set; } = null!;
    public string? mot_de_passe_hash { get; private set; }   // null = itsme-only account
    public string? eid_identifiant_hash { get; private set; }   // null = password-only account
    public bool est_actif { get; private set; }
    public bool est_suspendu { get; private set; }
    public int tentatives_echouees { get; private set; }
    public DateTime? verrouille_jusqua { get; private set; }
    public DateTime date_inscription { get; private set; }
    public DateTime? date_suppression { get; private set; }

    // --- password ---
    public void UpdatePassword(string motDePasseHash) => SetMotDePasseHash(motDePasseHash);

    public void UpdateTentatives_echouees()
    {
        if (verrouille_jusqua.HasValue && verrouille_jusqua.Value > DateTime.UtcNow)
            throw new InvalidOperationException(
                $"Compte verrouillé jusqu'au {verrouille_jusqua!.Value:dd/MM/yyyy HH:mm}.");

        tentatives_echouees++;
        if (tentatives_echouees >= MaxTentatives)
        {
            verrouille_jusqua = DateTime.UtcNow.AddMinutes(15);
            tentatives_echouees = 0;
        }
    }

    public void ResetTentatives_echouees()
    {
        tentatives_echouees = 0;
        verrouille_jusqua = null;
    }

    // --- itsme ---
    public void LierItsme(string itsmeSubHash)
    {
        if (eid_identifiant_hash != null)
            throw new InvalidOperationException("Un compte itsme est déjà lié.");
        SetEid(itsmeSubHash);
    }

    public void DelierItsme()
    {
        if (mot_de_passe_hash == null)
            throw new InvalidOperationException("Impossible de délier itsme : aucun mot de passe défini.");
        eid_identifiant_hash = null;
    }

    // --- account state ---
    public void Suspendre() => est_suspendu = true;
    public void Reactiver() => est_suspendu = false;

    public void Supprimer()
    {
        est_actif = false;
        date_suppression = DateTime.UtcNow;
    }

    // --- private validation ---
    private void SetNom(string t_nom)
    {
        if (string.IsNullOrWhiteSpace(t_nom))
            throw new ArgumentException("Le nom est obligatoire.", nameof(t_nom));
        nom = t_nom.Trim();
    }

    private void SetPrenom(string t_prenom)
    {
        if (string.IsNullOrWhiteSpace(t_prenom))
            throw new ArgumentException("Le prénom est obligatoire.", nameof(t_prenom));
        prenom = t_prenom.Trim();
    }

    private void SetEmail(string t_email)
    {
        var checker = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
        if (string.IsNullOrWhiteSpace(t_email) || !checker.IsValid(t_email))
            throw new ArgumentException("L'adresse email n'est pas valide.", nameof(t_email));
        email = t_email.Trim().ToLowerInvariant();
    }

    private void SetMotDePasseHash(string t_hash)
    {
        if (string.IsNullOrWhiteSpace(t_hash))
            throw new ArgumentException("Le hash du mot de passe est obligatoire.", nameof(t_hash));
        mot_de_passe_hash = t_hash;
    }

    private void SetEid(string t_eid)
    {
        if (string.IsNullOrWhiteSpace(t_eid))
            throw new ArgumentException("Identifiant itsme invalide.", nameof(t_eid));
        eid_identifiant_hash = t_eid;
    }
}