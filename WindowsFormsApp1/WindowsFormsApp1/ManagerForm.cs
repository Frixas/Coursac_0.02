using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ManagerForm : Form
    {
        private SqlConnection connection;
        private string userRole;

        public ManagerForm(string userRole)
        {
            InitializeComponent();
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\pizzaCAN.mdf;Integrated Security=True";
            connection = new SqlConnection(connectionString);
            this.userRole = userRole;
        }

        private void LoadProducts()
        {
            try
            {
                connection.Open();

                string selectQuery = "SELECT ProductId, Name, Quantity, Price FROM Products";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        flowLayoutPanelProducts.Controls.Clear();

                        while (reader.Read())
                        {
                            int productId = reader.GetInt32(0);
                            string productName = reader.GetString(1);
                            int quantity = reader.GetInt32(2);
                            decimal price = reader.GetDecimal(3);

                            Panel productPanel = new Panel();
                            productPanel.BorderStyle = BorderStyle.FixedSingle;
                            productPanel.Padding = new Padding(10);
                            productPanel.Margin = new Padding(5);
                            productPanel.Width = flowLayoutPanelProducts.Width - 50;

                            TableLayoutPanel tableLayout = new TableLayoutPanel();
                            tableLayout.Dock = DockStyle.Fill;
                            tableLayout.ColumnCount = 2;
                            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

                            Label lblProductName = new Label();
                            lblProductName.Text = "Название: " + productName + "  Id: " + productId;
                            tableLayout.Controls.Add(lblProductName, 0, 0);

                            Label lblQuantity = new Label();
                            lblQuantity.Text = "Количество: " + quantity;
                            tableLayout.Controls.Add(lblQuantity, 0, 1);

                            Label lblPrice = new Label();
                            lblPrice.Text = "Цена: " + price;
                            tableLayout.Controls.Add(lblPrice, 0, 2);

                            LinkLabel editLinkLabel = new LinkLabel();
                            editLinkLabel.Text = "Изменить данные";
                            editLinkLabel.LinkClicked += (sender, e) => EditLinkLabel_LinkClicked(productId);
                            tableLayout.Controls.Add(editLinkLabel, 0, 3);

                            productPanel.Controls.Add(tableLayout);
                            flowLayoutPanelProducts.Controls.Add(productPanel);
                            flowLayoutPanelProducts.AutoScroll = true;
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

        private void EditLinkLabel_LinkClicked(int productId)
        {
            EditProductForm editForm = new EditProductForm(productId);
            editForm.ShowDialog();
            LoadProducts();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text;
            int quantity = Convert.ToInt32(txtQuantity.Text);
            decimal price = Convert.ToDecimal(txtPrice.Text);

            if (!string.IsNullOrEmpty(productName))
            {
                try
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Products (Name, Quantity, Price) VALUES (@Name, @Quantity, @Price)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", productName);
                        command.Parameters.AddWithValue("@Quantity", quantity);
                        command.Parameters.AddWithValue("@Price", price);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Продукт успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении продукта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }

                LoadProducts();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите название продукта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Функция удаления товара недоступна для редактирования в форме ManagerForm. Редактирование возможно только через форму EditProductForm.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string filter = txtSearch.Text;
            FilterProducts(filter);
        }

        private void FilterProducts(string filter)
        {
            flowLayoutPanelProducts.Controls.Clear();

            try
            {
                connection.Open();

                string selectQuery = "SELECT ProductId, Name, Quantity, Price FROM Products WHERE Name LIKE @Filter";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Filter", "%" + filter + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int productId = reader.GetInt32(0);
                            string productName = reader.GetString(1);
                            int quantity = reader.GetInt32(2);
                            decimal price = reader.GetDecimal(3);

                            Panel productPanel = new Panel();
                            productPanel.BorderStyle = BorderStyle.FixedSingle;
                            productPanel.Padding = new Padding(15);
                            productPanel.Margin = new Padding(5);
                            productPanel.Width = flowLayoutPanelProducts.Width - 40;

                            Label lblProductName = new Label();
                            lblProductName.Text = "Название: " + productName;
                            productPanel.Controls.Add(lblProductName);

                            Label lblQuantity = new Label();
                            lblQuantity.Text = "Количество: " + quantity;
                            productPanel.Controls.Add(lblQuantity);

                            Label lblPrice = new Label();
                            lblPrice.Text = "Цена: " + price;
                            productPanel.Controls.Add(lblPrice);

                            LinkLabel editLinkLabel = new LinkLabel();
                            editLinkLabel.Text = "Изменить данные";
                            editLinkLabel.Tag = productId;
                            editLinkLabel.LinkClicked += (sender, e) => EditLinkLabel_LinkClicked(productId);
                            productPanel.Controls.Add(editLinkLabel);

                            flowLayoutPanelProducts.Controls.Add(productPanel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при фильтрации продуктов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void ManagerForm_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }
    }
}
