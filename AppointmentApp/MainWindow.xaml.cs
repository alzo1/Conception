using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppointmentApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadAppointments();
        }

        public class Appointment
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public string Time { get; set; }
            public int Num_employer { get; set; }
        }

        public void clearData()
        {

            nameTextBox.Clear();
            employerNumberTextBox.Clear();

            // donnée clear
            datePicker.SelectedDate = null;
            datePicker.DisplayDate = DateTime.Today;
            TimeSpan defaultTime = new TimeSpan();
            dateD.Clear();

        }

        public void LoadAppointments()
        {
            AppointmentDataGrid.Items.Refresh();
            try
            {
                string connectionString = "Data Source=RAMZIZIGI4B9D\\SQLEXPRESS;Initial Catalog=AppointmentDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT * FROM Appointments", connection);
                    SqlDataReader reader = command.ExecuteReader();

                    List<Appointment> appointments = new List<Appointment>();
                    while (reader.Read())
                    {
                        appointments.Add(new Appointment
                        {
                            ID = Convert.IsDBNull(reader["ID"]) ? 0 : (int)reader["ID"],
                            Name = reader["Name"].ToString(),
                            Date = (DateTime)reader["Date"],
                            Time = reader["Time"].ToString(),
                            Num_employer = reader["Num_employer"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Num_employer"])
                        }); ;
                    }
                    AppointmentDataGrid.ItemsSource = appointments;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            AppointmentDataGrid.Items.Refresh();
            string name = nameTextBox.Text;
            DateTime date = datePicker.SelectedDate ?? DateTime.MinValue;
            string time = dateD.Text;

            int employerNumber;

            if (!int.TryParse(employerNumberTextBox.Text, out employerNumber))
            {
                MessageBox.Show("Employer number must be a valid integer");
                return;
            }


            if (string.IsNullOrEmpty(name) || date == DateTime.MinValue || string.IsNullOrEmpty(time) || !int.TryParse(employerNumberTextBox.Text, out employerNumber))
            {
                MessageBox.Show("Please fill in all fields");
                return;
            }

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                MessageBox.Show("Les rendez-vous ne peuvent pas être pris le week-end.");
                return;
            }
          

            try
            {
                // Connexion à la base de données
                string connectionString = "Data Source=RAMZIZIGI4B9D\\SQLEXPRESS;Initial Catalog=AppointmentDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérification si l'employé a déjà un rendez-vous
                    SqlCommand commandCheck = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE Num_employer = @Num_employer", connection);
                    commandCheck.Parameters.AddWithValue("@Num_employer", employerNumber);
                    int count = (int)commandCheck.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Un rendez-vous existe déjà pour cet employeur.");
                        return;
                    }

                    // Insertion de l'appointment dans la base de données
                    SqlCommand command = new SqlCommand("IF NOT EXISTS (SELECT * FROM Appointments WHERE Num_employer = @Num_employer)\r\nBEGIN\r\n  INSERT INTO Appointments (Name, Date, Time, Num_employer) \r\n  VALUES (@Name, @Date, @Time, @Num_employer)\r\nEND", connection);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Time", time);
                    command.Parameters.AddWithValue("@Num_employer", employerNumber);
                    
                    command.ExecuteNonQuery();

                    // Récupération des rendez-vous à partir de la base de données
                    command = new SqlCommand("SELECT ID, Name, Date, Time, Num_employer FROM Appointments ", connection);
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
                    // Affichage des rendez-vous dans le DataGrid
                    AppointmentDataGrid.ItemsSource = appointments;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            AppointmentDataGrid.Items.Refresh();
            Appointment appointment = (Appointment)AppointmentDataGrid.SelectedItem;
            if (appointment != null)
            {
                int id = appointment.ID;

                try
                {
                    string connectionString = "Data Source=RAMZIZIGI4B9D\\SQLEXPRESS;Initial Catalog=AppointmentDB;Integrated Security=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand("DELETE FROM Appointments WHERE ID = @ID", connection);
                        command.Parameters.AddWithValue("@ID", id);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            LoadAppointments();
                            MessageBox.Show("Record deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Record not found.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchWindow searchWindow = new searchWindow();
            bool? result = searchWindow.ShowDialog();
            if (result == true)
            {
                string searchName = searchWindow.EmployerNumberTextBox.Text;
                // Faire la recherche avec le nom saisi
            }

        }

        public class Employee
        {
            public int ID { get; set; }
            public string Name { get; set; }
            
           
            public int Num_employer { get; set; }
        }

    }

}
