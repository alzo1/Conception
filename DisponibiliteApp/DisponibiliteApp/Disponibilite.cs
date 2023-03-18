using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisponibiliteApp
{
    /// <summary>
    /// Représente une disponibilité d'un employé.
    /// </summary>
    public class Disponibilite
    {
        /// <summary>
        /// Obtient ou définit l'identifiant de la disponibilité.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'employé auquel appartient la disponibilité.
        /// </summary>
        public int EmployeId { get; set; }

        /// <summary>
        /// Obtient ou définit le jour de la semaine de la disponibilité.
        /// </summary>
        public string JourSemaine { get; set; }

        /// <summary>
        /// Obtient ou définit l'heure de début de la disponibilité.
        /// </summary>
        public TimeSpan HeureDebut { get; set; }

        /// <summary>
        /// Obtient ou définit l'heure de fin de la disponibilité.
        /// </summary>
        public TimeSpan HeureFin { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe Disponibilite avec les valeurs spécifiées.
        /// </summary>
        /// <param name="id">L'identifiant de la disponibilité.</param>
        /// <param name="employeId">L'identifiant de l'employé auquel appartient la disponibilité.</param>
        /// <param name="jourSemaine">Le jour de la semaine de la disponibilité.</param>
        /// <param name="heureDebut">L'heure de début de la disponibilité.</param>
        /// <param name="heureFin">L'heure de fin de la disponibilité.</param>
        public Disponibilite(int id, int employeId, string jourSemaine, TimeSpan heureDebut, TimeSpan heureFin)
        {
            Id = id;
            EmployeId = employeId;
            JourSemaine = jourSemaine;
            HeureDebut = heureDebut;
            HeureFin = heureFin;
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe Disponibilite avec les valeurs spécifiées.
        /// </summary>
        /// <param name="jour">Le jour de la disponibilité.</param>
        /// <param name="heureDebut">L'heure de début de la disponibilité.</param>
        /// <param name="heureFin">L'heure de fin de la disponibilité.</param>
        public Disponibilite(string jour, DateTime heureDebut, DateTime heureFin)
        {
        }
    }
}

