using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
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
using static AppointmentApp.MainWindow;

namespace AppointmentApp
{
    /// <summary>
    /// Logique d'interaction pour searchWindow.xaml
    /// </summary>
    public partial class searchWindow : Window
    {
        public searchWindow()
        {
            InitializeComponent();
        }

        public searchWindow(string message)
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            int employerNumber;
            if (!int.TryParse(EmployerNumberTextBox.Text, out employerNumber))
            {
                MessageBox.Show("Employer number must be a valid integer");
                return;
            }

            try
            {
                string connectionString = "Data Source=RAMZIZIGI4B9D\\SQLEXPRESS;Initial Catalog=AppointmentDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT * FROM Appointments WHERE Num_employer = @Num_employer", connection);
                    command.Parameters.AddWithValue("@Num_employer", employerNumber);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);


                    // Conversion des données en objets Appointment
                    List<Appointment> appointments = new List<Appointment>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        appointments.Add(new Appointment
                        {
                            ID = (int)row["ID"],
                            Name = row["Name"].ToString(),
                            Date = (DateTime)row["Date"],
                            Time = row["Time"].ToString(),
                            Num_employer = (int)row["Num_employer"]
                        });
                    }

                    // Définition de la source de données pour le DataGrid
                    AppointmentDataGrid.ItemsSource = appointments;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
