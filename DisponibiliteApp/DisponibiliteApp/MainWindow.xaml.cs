using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DisponibiliteApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=RAMZIZIGI4B9D\\SQLEXPRESS;Initial Catalog=MaBaseDeDonnees;Integrated Security=True"; // remplacer avec vos informations
        private List<Employe> Employes { get; set; }
        private List<Disponibilite> Disponibilites { get; set; }
        private SqlConnection conn;

        public MainWindow()
        {
            InitializeComponent();
            Employes = new List<Employe>();
            Disponibilites = new List<Disponibilite>();
            conn = new SqlConnection(connectionString);
            conn.Open();
            LoadEmployes();
        }
        /// <summary>
        /// Charge la liste des employés à partir de la base de données.
        /// </summary>
        private void LoadEmployes()
        {
            using (SqlCommand command = new SqlCommand("SELECT Id, Nom, Prenom FROM Employe", conn))
            {
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["Id"];
                    string nom = (string)reader["Nom"];
                    string prenom = (string)reader["Prenom"];
                    Employe employe = new Employe(id, nom, prenom);
                    Employes.Add(employe);
                }
                reader.Close();
                EmployesListView.ItemsSource = Employes;
            }
        }
        /// <summary>
        /// Supprime l'employé sélectionné de la liste et de la base de données.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void supprimerEmployeButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployesListView.SelectedItem != null)
            {
                var employe = (Employe)EmployesListView.SelectedItem;
                Employes.Remove(employe);
                // Supprimer l'employé de la base de données
                SqlCommand cmd = new SqlCommand("DELETE FROM Employe WHERE Id=@id", conn);
                cmd.Parameters.AddWithValue("@id", employe.Id);
                cmd.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un employé à supprimer.");
            }
        }
        /// <summary>
        /// Ajoute un nouvel employé à la liste et à la base de données.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ajouterEmployeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NomTextBox.Text) && !string.IsNullOrEmpty(PrenomTextBox.Text))
            {
                var employe = new Employe(Employes.Count + 1, NomTextBox.Text, PrenomTextBox.Text);
                // Insérer l'employé dans la base de données
                SqlCommand cmd = new SqlCommand("INSERT INTO Employe (Nom, Prenom) VALUES (@nom, @prenom)", conn);
                cmd.Parameters.AddWithValue("@nom", employe.Nom);
                cmd.Parameters.AddWithValue("@prenom", employe.Prenom);
                cmd.ExecuteNonQuery();

                Employes.Add(employe);
            }
            else
            {
                MessageBox.Show("Veuillez saisir un nom et un prénom.");
            }

        }
        /// <summary>
        /// supprime la disponibilité sélectionnée de la base de données et de la liste des disponibilités affichées dans l'interface utilisateur.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SupprimerDisponibiliteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DisponibilitesListBox.SelectedItem != null)
            {
                var disponibilite = (Disponibilite)DisponibilitesListBox.SelectedItem;

                // Supprimer la disponibilité de la base de données
                SqlCommand cmd = new SqlCommand("DELETE FROM Disponibilite WHERE Id=@id", conn);
                cmd.Parameters.AddWithValue("@id", disponibilite.Id);
                cmd.ExecuteNonQuery();

                // Supprimer la disponibilité de la liste
                Disponibilites.Remove(disponibilite);
            }
        }
    }

}


