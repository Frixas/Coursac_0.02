using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class EditProductForm : Form
    {
        private SqlConnection connection;
        private int productId;

        public EditProductForm(int productId)
        {
            InitializeComponent();
            string databaseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pizzaCAN.mdf");
            string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={databaseFile};Integrated Security=True";
            connection = new SqlConnection(connectionString);
            this.productId = productId;
        }

        private void EditProductForm_Load(object sender, EventArgs e)
        {
            // Загрузка чере метод с id - продукта
            LoadProductDetails(productId);
        }

        private void LoadProductDetails(int productId)
        {
            try
            {
                connection.Open();

                string selectQuery = "SELECT Name, Quantity, Price FROM Products WHERE ProductId = @ProductId";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string productName = reader.GetString(0);
                            int quantity = reader.GetInt32(1);
                            decimal price = reader.GetDecimal(2);

                            // Обновление textbox с данными выбранного продукта
                            txtProductName.Text = productName;
                            txtQuantity.Text = quantity.ToString();
                            txtPrice.Text = price.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text;
            int quantity = Convert.ToInt32(txtQuantity.Text);
            decimal price = Convert.ToDecimal(txtPrice.Text);

            if (!string.IsNullOrEmpty(productName))
            {
                try
                {
                    connection.Open();

                    string updateQuery = "UPDATE Products SET Name = @Name, Quantity = @Quantity, Price = @Price WHERE ProductId = @ProductId";
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", productName);
                        command.Parameters.AddWithValue("@Quantity", quantity);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите название продукта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
