using DisponibiliteApp;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Classe représentant un employé
/// </summary>
public class Employe
{
    /// <summary>
    /// Nom de l'employé
    /// </summary>
    public string Nom { get; set; }

    /// <summary>
    /// Prénom de l'employé
    /// </summary>
    public string Prenom { get; set; }

    /// <summary>
    /// Identifiant de l'employé
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Liste des disponibilités de l'employé
    /// </summary>
    public List<Disponibilite> Disponibilites { get; set; }

    /// <summary>
    /// Constructeur de la classe Employe
    /// </summary>
    /// <param name="id">Identifiant de l'employé</param>
    /// <param name="nom">Nom de l'employé</param>
    /// <param name="prenom">Prénom de l'employé</param>
    public Employe(int id, string nom, string prenom)
    {
        Id = id;
        Nom = nom;
        Prenom = prenom;
        Disponibilites = new List<Disponibilite>();
    }

    /// <summary>
    /// Ajoute une disponibilité pour l'employé
    /// </summary>
    /// <param name="jour">Jour de la semaine</param>
    /// <param name="heureDebut">Heure de début de la disponibilité</param>
    /// <param name="heureFin">Heure de fin de la disponibilité</param>
    public void AjouterDisponibilite(string jour, DateTime heureDebut, DateTime heureFin)
    {
        var disponibilite = new Disponibilite(jour, heureDebut, heureFin);
        Disponibilites.Add(disponibilite);
    }

    /// <summary>
    /// Vérifie si l'employé est disponible à une certaine plage horaire
    /// </summary>
    /// <param name="jour">Jour de la semaine</param>
    /// <param name="heureDebut">Heure de début de la plage horaire</param>
    /// <param name="heureFin">Heure de fin de la plage horaire</param>
    /// <returns>Retourne vrai si l'employé est disponible, faux sinon</returns>
    public bool VerifierDisponibilite(string jour, DateTime heureDebut, DateTime heureFin)
    {
        return Disponibilites.Any(d => d.JourSemaine == jour && d.HeureDebut <= heureDebut.TimeOfDay && d.HeureFin >= heureFin.TimeOfDay);
    }

    /// <summary>
    /// Supprime une disponibilité de l'employé
    /// </summary>
    /// <param name="jour">Jour de la semaine</param>
    /// <param name="heureDebut">Heure de début de la disponibilité</param>
    /// <param name="heureFin">Heure de fin de la disponibilité</param>
    public void SupprimerDisponibilite(string jour, DateTime heureDebut, DateTime heureFin)
    {
        Disponibilites.RemoveAll(d => d.JourSemaine == jour && d.HeureDebut == heureDebut.TimeOfDay && d.HeureFin == heureFin.TimeOfDay);
    }
}
