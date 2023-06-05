using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class UserForm : Form
    {
        private SqlConnection connection;
        private int userId;

        public UserForm(int userId)
        {
            InitializeComponent();
            string databaseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pizzaCAN.mdf");
            string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={databaseFile};Integrated Security=True";
            connection = new SqlConnection(connectionString);
            this.userId = userId;
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            LoadCartItems();
        }

        private void LoadCartItems()
        {
            // Очистка текущего содержимого корзины
            flowLayoutPanelCart.Controls.Clear();

            // Подключение к базе данных
            connection.Open();

            // Загрузка данных о продуктах в корзине из базы данных
            string query = "SELECT Products.Name, Products.Price, Cart.Quantity FROM Cart JOIN Products ON Cart.ProductId = Products.ProductId WHERE Cart.UserId = @userId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@userId", userId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Чтение данных о продукте из результата запроса
                        string productName = reader.GetString(0);
                        decimal productPrice = reader.GetDecimal(1);
                        int productQuantity = reader.GetInt32(2);

                        // Создание элементов управления для отображения продукта в корзине
                        Panel cartItemPanel = new Panel();
                        cartItemPanel.BorderStyle = BorderStyle.FixedSingle;
                        cartItemPanel.Margin = new Padding(10);

                        Label productNameLabel = new Label();
                        productNameLabel.Text = productName;
                        productNameLabel.Font = new Font("Arial", 12);
                        productNameLabel.Margin = new Padding(10);
                        productNameLabel.AutoSize = true;
                        productNameLabel.Dock = DockStyle.Top;

                        Label productPriceLabel = new Label();
                        productPriceLabel.Text = $"Price: {productPrice:C}";
                        productPriceLabel.Font = new Font("Arial", 10);
                        productPriceLabel.Margin = new Padding(10);
                        productPriceLabel.AutoSize = true;
                        productPriceLabel.Dock = DockStyle.Top;

                        Label productQuantityLabel = new Label();
                        productQuantityLabel.Text = $"Quantity: {productQuantity}";
                        productQuantityLabel.Font = new Font("Arial", 10);
                        productQuantityLabel.Margin = new Padding(10);
                        productQuantityLabel.AutoSize = true;
                        productQuantityLabel.Dock = DockStyle.Top;

                        // Добавление элементов управления в панель товара в корзине
                        cartItemPanel.Controls.Add(productQuantityLabel);
                        cartItemPanel.Controls.Add(productPriceLabel);
                        cartItemPanel.Controls.Add(productNameLabel);

                        // Добавление панели товара в корзину
                        flowLayoutPanelCart.Controls.Add(cartItemPanel);
                    }
                }
            }

            flowLayoutPanelCart.AutoScroll = true;

            // Закрытие подключения к базе данных
            connection.Close();
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            // Проверка, пуста ли корзина
            if (flowLayoutPanelCart.Controls.Count == 0)
            {
                MessageBox.Show("Корзина пуста. Добавьте товары перед размещением заказа.", "Пустая корзина", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Подключение к базе данных
            connection.Open();

            // Создание нового заказа в таблице Orders
            string insertOrderQuery = "INSERT INTO Orders (UserId, OrderDate) VALUES (@userId, @orderDate); SELECT SCOPE_IDENTITY();";
            using (SqlCommand insertOrderCommand = new SqlCommand(insertOrderQuery, connection))
            {
                insertOrderCommand.Parameters.AddWithValue("@userId", userId);
                insertOrderCommand.Parameters.AddWithValue("@orderDate", DateTime.Now);
                int orderId = Convert.ToInt32(insertOrderCommand.ExecuteScalar());

                // Создание записей о товарах в заказе в таблице OrderItems
                foreach (Control cartItemPanel in flowLayoutPanelCart.Controls)
                {
                    Panel panel = (Panel)cartItemPanel;
                    Label productNameLabel = (Label)panel.Controls[2];
                    Label productPriceLabel = (Label)panel.Controls[1];
                    Label productQuantityLabel = (Label)panel.Controls[0];

                    string productName = productNameLabel.Text;
                    string productPriceText = productPriceLabel.Text.Replace("Price: ", "").Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol, "").Replace(",", "").Trim();
                    decimal productPrice;
                    if (!decimal.TryParse(productPriceText, NumberStyles.Currency, CultureInfo.CurrentCulture, out productPrice))
                    {
                        MessageBox.Show("Неверный формат цены товара: " + productPriceLabel.Text, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string productQuantityText = productQuantityLabel.Text.Replace("Quantity: ", "").Trim();
                    int productQuantity;
                    if (!int.TryParse(productQuantityText, out productQuantity))
                    {
                        MessageBox.Show("Неверный формат количества товара: " + productQuantityLabel.Text, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Создание записей о товарах в заказе в таблице OrderItems
                    string insertOrderItemQuery = "INSERT INTO OrderItems (OrderId, ProductName, ProductPrice, Quantity) VALUES (@orderId, @productName, @productPrice, @quantity)";
                    using (SqlCommand insertOrderItemCommand = new SqlCommand(insertOrderItemQuery, connection))
                    {
                        insertOrderItemCommand.Parameters.AddWithValue("@orderId", orderId);
                        insertOrderItemCommand.Parameters.AddWithValue("@productName", productName);
                        insertOrderItemCommand.Parameters.AddWithValue("@productPrice", productPrice);
                        insertOrderItemCommand.Parameters.AddWithValue("@quantity", productQuantity);
                        insertOrderItemCommand.ExecuteNonQuery();
                    }

                    // Обновление количества продуктов в таблице Products
                    string updateProductQuery = "UPDATE Products SET Quantity = Quantity - @quantity WHERE Name = @productName";
                    using (SqlCommand updateProductCommand = new SqlCommand(updateProductQuery, connection))
                    {
                        updateProductCommand.Parameters.AddWithValue("@quantity", productQuantity);
                        updateProductCommand.Parameters.AddWithValue("@productName", productName);
                        updateProductCommand.ExecuteNonQuery();
                    }
                }

                // Удаление записей о заказах из таблицы Cart
                string deleteOrderQuery = "DELETE FROM Cart WHERE UserId = @userId";
                using (SqlCommand deleteOrderCommand = new SqlCommand(deleteOrderQuery, connection))
                {
                    deleteOrderCommand.Parameters.AddWithValue("@userId", userId);
                    deleteOrderCommand.ExecuteNonQuery();
                }

                // Очистка корзины после размещения заказа
                flowLayoutPanelCart.Controls.Clear();

                MessageBox.Show("Заказ успешно размещен.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Закрытие подключения к базе данных
            connection.Close();
        }


        private void btnClearCart_Click(object sender, EventArgs e)
        {
            // Проверка, пуста ли корзина
            if (flowLayoutPanelCart.Controls.Count == 0)
            {
                MessageBox.Show("Корзина уже пуста.", "Пустая корзина", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Подключение к базе данных
            connection.Open();

            // Удаление сохраненного заказа из таблицы Cart
            string deleteOrderQuery = "DELETE FROM Cart WHERE UserId = @userId";
            using (SqlCommand deleteOrderCommand = new SqlCommand(deleteOrderQuery, connection))
            {
                deleteOrderCommand.Parameters.AddWithValue("@userId", userId);
                deleteOrderCommand.ExecuteNonQuery();
            }

            // Очистка корзины
            flowLayoutPanelCart.Controls.Clear();

            MessageBox.Show("Корзина очищена!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Закрытие подключения к базе данных
            connection.Close();
        }
    }
}
