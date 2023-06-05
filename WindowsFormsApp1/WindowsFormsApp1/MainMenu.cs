using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MainMenu : Form
    {
        private string connectionString;
        private string userRole;
        private int userId;

        public MainMenu(string role, int userId)
        {
            InitializeComponent();
            string databaseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pizzaCAN.mdf");
            connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={databaseFile};Integrated Security=True";
            userRole = role;
            this.userId = userId;
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            LoadProductsFromDatabase();
            CheckUserRole();
        }

        private void CheckUserRole()
        {
            if (userRole != "manager" && userRole != "admin" && userRole != "director" && userRole != "user")
            {
                MessageBox.Show("Недостаточно прав для доступа", "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void LoadProductsFromDatabase()
        {
            // Очистка текущего содержимого меню товаров
            flowLayoutPanelProducts.Controls.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Подключение к базе данных
                connection.Open();

                // Загрузка данных о продуктах из базы данных
                string query = "SELECT * FROM Products";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Чтение данных о продукте из результата запроса
                            int productId = reader.GetInt32(0);
                            string productName = reader.GetString(1);
                            decimal productPrice = reader.GetDecimal(2);
                            int productQuantity = reader.GetInt32(3);

                            // Пропустить продукты с нулевым количеством
                            if (productQuantity <= 0)
                                continue;

                            // Создание элементов управления для отображения продукта
                            Panel productPanel = new Panel();
                            productPanel.BorderStyle = BorderStyle.FixedSingle;
                            productPanel.Margin = new Padding(10);
                            productPanel.Dock = DockStyle.Top;

                            Label productNameLabel = new Label();
                            productNameLabel.Text = productName;
                            productNameLabel.Font = new Font("Arial", 12);
                            productNameLabel.Margin = new Padding(10);
                            productNameLabel.AutoSize = true;
                            productNameLabel.Dock = DockStyle.Top;

                            Button addToCartButton = new Button();
                            addToCartButton.Text = "Добавить в корзину";
                            addToCartButton.Tag = productId;
                            addToCartButton.Margin = new Padding(10);
                            addToCartButton.Dock = DockStyle.Top;
                            addToCartButton.Click += addToCartButton_Click;

                            // Добавление элементов управления в панель товара
                            productPanel.Controls.Add(addToCartButton);
                            productPanel.Controls.Add(productNameLabel);

                            // Добавление панели товара в меню товаров
                            flowLayoutPanelProducts.Controls.Add(productPanel);
                        }
                    }
                }

                // Закрытие подключения к базе данных
                connection.Close();
            }
        }

        private void addToCartButton_Click(object sender, EventArgs e)
        {
            Button addToCartButton = (Button)sender;
            int productId = (int)addToCartButton.Tag;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Подключение к базе данных
                connection.Open();

                // Проверка наличия выбранного продукта в корзине
                string checkQuery = "SELECT COUNT(*) FROM Cart WHERE UserId = @userId AND ProductId = @productId";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@userId", userId);
                    checkCommand.Parameters.AddWithValue("@productId", productId);

                    int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (existingCount > 0)
                    {
                        // Обновление количества позиций товара
                        string updateQuery = "UPDATE Cart SET Quantity = Quantity + 1 WHERE UserId = @userId AND ProductId = @productId";
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@userId", userId);
                            updateCommand.Parameters.AddWithValue("@productId", productId);
                            updateCommand.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Добавление продукта в корзину
                        string insertQuery = "INSERT INTO Cart (UserId, ProductId, Quantity) VALUES (@userId, @productId, 1)";
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@userId", userId);
                            insertCommand.Parameters.AddWithValue("@productId", productId);
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }

                // Закрытие подключения к базе данных
                connection.Close();
            }
        }

        private void btnManager_Click(object sender, EventArgs e)
        {
            if (userRole == "manager" || userRole == "director")
            {
                ManagerForm managerForm = new ManagerForm(userRole);
                managerForm.FormClosed += (s, args) => LoadProductsFromDatabase();
                managerForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Недостаточно прав для доступа", "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            if (userRole == "admin" || userRole == "director")
            {
                AdminForm adminForm = new AdminForm(userRole);
                adminForm.FormClosed += (s, args) => LoadProductsFromDatabase();
                adminForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Недостаточно прав для доступа", "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            UserForm userForm = new UserForm(userId);
            userForm.FormClosed += (s, args) => LoadProductsFromDatabase();
            userForm.ShowDialog();
        }
    }
}
