using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class RegistrationForm : Form
    {
        private SqlConnection connection;

        public RegistrationForm()
        {
            InitializeComponent();
            string databaseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pizzaCAN.mdf");
            string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={databaseFile};Integrated Security=True";
            connection = new SqlConnection(connectionString);

            txtPassword.UseSystemPasswordChar = true;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Проверка наличия обязательных полей
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка регистрации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Подключение к базе данных
            connection.Open();

            // Проверка, существует ли пользователь с таким же именем
            string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username";
            using (SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection))
            {
                checkUserCommand.Parameters.AddWithValue("@username", username);
                int userCount = Convert.ToInt32(checkUserCommand.ExecuteScalar());
                if (userCount > 0)
                {
                    MessageBox.Show("Пользователь с таким именем уже существует.", "Ошибка регистрации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    connection.Close();
                    return;
                }
            }

            // Регистрация нового пользователя
            string registerQuery = "INSERT INTO Users (Username, Password, Role) VALUES (@username, @password, '@role')";
            using (SqlCommand registerCommand = new SqlCommand(registerQuery, connection))
            {
                registerCommand.Parameters.AddWithValue("@username", username);
                registerCommand.Parameters.AddWithValue("@password", password);
                registerCommand.Parameters.AddWithValue("@role", "user");
                registerCommand.ExecuteNonQuery();
            }


            MessageBox.Show("Регистрация успешно завершена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Закрытие подключения к базе данных
            connection.Close();

            // Закрытие формы регистрации
            this.Close();
        }
    }
}
