using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class LoginForm : Form
    {
        private SqlConnection connection;
        private string userRole;
        private int userId;

        public LoginForm()
        {
            InitializeComponent();
            string databaseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pizzaCAN.mdf");
            string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={databaseFile};Integrated Security=True";
            connection = new SqlConnection(connectionString);
            txtPassword.UseSystemPasswordChar = true;

            // Добавление обработчиков событий для Label
            label1.MouseEnter += label1_MouseEnter;
            label1.MouseLeave += label1_MouseLeave;
        }

        private int GetUserIdByUsername(string username)
        {
            int userId = 0;

            try
            {
                // Подключение к базе данных
                connection.Open();

                // Получение userId по имени пользователя
                string query = "SELECT UserId FROM Users WHERE Username = @username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка исключения, если необходимо
                Console.WriteLine("Произошла ошибка: " + ex.Message);
            }
            finally
            {
                // Закрытие подключения к базе данных
                connection.Close();
            }

            return userId;
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = "";

            try
            {
                // Поиск пользователя в базе данных
                string query = "SELECT Role FROM Users WHERE Username = @username AND Password = @password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            role = reader.GetString(0);
                            // Получение userId по имени пользователя
                            userId = GetUserIdByUsername(username);
                        }
                    }
                    connection.Close();
                }

                // Проверка роли пользователя
                if (role == "user" || role == "manager" || role == "admin" || role == "director")
                {
                    userRole = role;
                    // Открыть форму для пользователя
                    MainMenu mainMenu = new MainMenu(userRole, userId); // Передача userId в MainMenu
                    mainMenu.Show();
                    this.Hide();

                    // Создание отметки в таблице EventLog
                    string insertQuery = "INSERT INTO EventLog (Username, LoginTime) VALUES (@username, @loginTime)";
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@username", username);
                        insertCommand.Parameters.AddWithValue("@loginTime", DateTime.Now);
                        connection.Open();
                        insertCommand.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Неправильный логин или пароль", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                // Закрытие подключения к базе данных (в случае возникновения исключения)
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }



        private void label1_Click(object sender, EventArgs e)
        {
            RegistrationForm registrationForm = new RegistrationForm();
            registrationForm.Show();
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            // Обработчик события MouseEnter для Label
            // Выполняйте нужные действия здесь, когда курсор наведен на Label
            Label label = (Label)sender;
            label.ForeColor = Color.Blue;
            label.Font = new Font(label.Font, FontStyle.Underline);
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            // Обработчик события MouseLeave для Label
            // Выполняйте нужные действия здесь, когда курсор покинул Label
            Label label = (Label)sender;
            label.ForeColor = SystemColors.ControlText;
            label.Font = new Font(label.Font, FontStyle.Regular);
        }
    }
}
