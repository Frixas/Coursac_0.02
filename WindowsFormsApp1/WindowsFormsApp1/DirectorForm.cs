using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WindowsFormsApp1
{
    public partial class DirectorForm : Form
    {
        private SqlConnection connection;
        private string userRole;

        public DirectorForm(string userRole)
        {
            InitializeComponent();
            string databaseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pizzaCAN.mdf");
            string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={databaseFile};Integrated Security=True";
            connection = new SqlConnection(connectionString);
            this.userRole = userRole;
        }

        private void DirectorForm_Load(object sender, EventArgs e)
        {
            LoadComboBoxUsers();
            LoadUsers();
            LoadCbUsers();
            LoadRoles();
            LoadEventLogData();
        }

        private void LoadCbUsers()
        {
            try
            {
                connection.Open();

                // Загрузка списка пользователей с базы данных
                string selectQuery = "SELECT username FROM Users";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        cbUsers.Items.Clear();

                        while (reader.Read())
                        {
                            string username = reader["username"].ToString();
                            cbUsers.Items.Add(username);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void LoadComboBoxUsers()
        {
            try
            {
                connection.Open();

                // Загрузка пользователей с базы данных
                string selectQuery = "SELECT username FROM Users";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        comboBoxUsers.Items.Clear();

                        while (reader.Read())
                        {
                            string username = reader["username"].ToString();
                            comboBoxUsers.Items.Add(username);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: { ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void LoadUsers()
        {
            try
            {
                connection.Open();

                // Загрузка пользователей с паролями
                string selectQuery = "SELECT username, password FROM Users";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        flowLayoutUsers.Controls.Clear();

                        while (reader.Read())
                        {
                            string username = reader["username"].ToString();
                            string password = reader["password"].ToString();

                            // Создание панели user
                            FlowLayoutPanel userPanel = new FlowLayoutPanel();
                            userPanel.FlowDirection = FlowDirection.TopDown;
                            userPanel.AutoSize = true;
                            userPanel.Tag = username;

                            // label с именем
                            Label lblUsername = new Label();
                            lblUsername.Text = username;
                            lblUsername.AutoSize = true;
                            userPanel.Controls.Add(lblUsername);

                            // Текстовое поле для пароля
                            TextBox txtPassword = new TextBox();
                            txtPassword.Name = "txtPassword";
                            txtPassword.Text = password;
                            txtPassword.PasswordChar = '*';
                            userPanel.Controls.Add(txtPassword);

                            // Кнопка для сохранения
                            Button btnSavePassword = new Button();
                            btnSavePassword.Text = "Save Password";
                            btnSavePassword.Tag = username;
                            btnSavePassword.Click += btnSavePassword_Click;
                            userPanel.Controls.Add(btnSavePassword);

                            flowLayoutUsers.Controls.Add(userPanel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Закрытие подключения
                connection.Close();
            }
        }

        private void LoadRoles()
        {
            cbRoles.Items.Clear();
            cbRoles.Items.AddRange(new string[] { "manager", "administrator", "director" });
        }

        private void LoadEventLogData()
        {
            {
                try
                {
                    // Подключение к базе данных
                    connection.Open();

                    // Загрузка данных о входе
                    string selectQuery = "SELECT Username, LogId, LoginTime FROM EventLog";
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            flowLayoutPanelEventLog.Controls.Clear();

                            while (reader.Read())
                            {
                                string username = reader["Username"].ToString();
                                int logId = Convert.ToInt32(reader["LogId"]);
                                DateTime loginTime = Convert.ToDateTime(reader["LoginTime"]);

                                Panel logPanel = new Panel();
                                logPanel.BorderStyle = BorderStyle.FixedSingle;
                                logPanel.Padding = new Padding(10);
                                logPanel.Margin = new Padding(5);
                                logPanel.Width = 250;

                                Label usernameLabel = new Label();
                                usernameLabel.Text = $"Username: {username}";
                                usernameLabel.AutoSize = true;
                                logPanel.Controls.Add(usernameLabel);

                                Label loginTimeLabel = new Label();
                                loginTimeLabel.Text = $"Login Time: {loginTime.ToString("dd.MM.yyyy HH:mm:ss")}";
                                loginTimeLabel.AutoSize = true;
                                loginTimeLabel.Top = usernameLabel.Bottom + 5; 
                                logPanel.Controls.Add(loginTimeLabel);

                                // idlog
                                Label logIdLabel = new Label();
                                logIdLabel.Text = $"Log ID: {logId}";
                                logIdLabel.AutoSize = true;
                                logIdLabel.Top = loginTimeLabel.Bottom + 5; 
                                logPanel.Controls.Add(logIdLabel);

                                logPanel.Height = logIdLabel.Bottom + 10;

                                // logPanel 
                                flowLayoutPanelEventLog.Controls.Add(logPanel);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Закрытие подключения
                    connection.Close();
                }
                flowLayoutPanelEventLog.AutoScroll = true;
            }
        }

        private void comboBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUser = cbUsers.SelectedItem?.ToString();

            try
            {
                // Подключение к базе данных
                connection.Open();

                // Загрузка информации о логировании определенного пользователя
                string selectQuery = "SELECT LogId, Username, LoginTime FROM EventLog WHERE Username = @Username";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", selectedUser);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        flowLayoutPanelEventLog.Controls.Clear();

                        while (reader.Read())
                        {
                            string logId = reader["LogId"].ToString();
                            string username = reader["Username"].ToString();
                            string loginTime = reader["LoginTime"].ToString();

                            // label
                            Label logEntryLabel = new Label();
                            logEntryLabel.Text = $"Username: {username}\nLogin Time: {loginTime}\nLog ID: {logId}";
                            logEntryLabel.AutoSize = true;

                            // label
                            flowLayoutPanelEventLog.Controls.Add(logEntryLabel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Закрытие подключения
                connection.Close();
            }
        }



        private void btnGenerateEventLogReport_Click(object sender, EventArgs e)
        {
            try
            {
                // Открытие Excel
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = true;

                // Создание нового листа
                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = workbook.ActiveSheet;

                // Загаловки столбцов
                for (int i = 0; i < flowLayoutPanelEventLog.Controls.Count; i++)
                {
                    Panel logPanel = (Panel)flowLayoutPanelEventLog.Controls[i];
                    Label usernameLabel = (Label)logPanel.Controls[0];
                    Label loginTimeLabel = (Label)logPanel.Controls[1];
                    Label logIdLabel = (Label)logPanel.Controls[2];

                    worksheet.Cells[1, 1] = "Username";
                    worksheet.Cells[1, 2] = "Login Time";
                    worksheet.Cells[1, 3] = "Log ID";

                    worksheet.Cells[i + 2, 1] = usernameLabel.Text.Substring(10);
                    worksheet.Cells[i + 2, 2] = loginTimeLabel.Text.Substring(12);
                    worksheet.Cells[i + 2, 3] = logIdLabel.Text.Substring(7);
                }

                // Сохранение excel-файла
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Сохранить Отчёт Событий";
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Отчёт был успешно сохранен!", "Сохранить Отчёт", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Не выбрано имя файла", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //Закрыть Excel
                excelApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации отчета: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSaveRole_Click(object sender, EventArgs e)
        {
            string selectedUser = comboBoxUsers.SelectedItem?.ToString();
            string selectedRole = cbRoles.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedUser) || string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("Выберите роль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            { 
                connection.Open();

                // Обновление роли выбранного пользователя
                string updateQuery = "UPDATE Users SET role = @Role WHERE username = @Username";
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Role", selectedRole);
                    command.Parameters.AddWithValue("@Username", selectedUser);
                    command.ExecuteNonQuery();

                    MessageBox.Show($"Роль для пользователя {selectedUser} успешно обновлена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления роли пользователя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnSavePassword_Click(object sender, EventArgs e)
        {
            Button btnSavePassword = (Button)sender;
            string username = btnSavePassword.Tag.ToString();

            FlowLayoutPanel userPanel = (FlowLayoutPanel)btnSavePassword.Parent;
            TextBox txtPassword = (TextBox)userPanel.Controls["txtPassword"];
            string newPassword = txtPassword.Text;

            try
            {
                connection.Open();

                string updateQuery = "UPDATE Users SET password = @Password WHERE username = @Username";
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Password", newPassword);
                    command.Parameters.AddWithValue("@Username", username);
                    command.ExecuteNonQuery();

                    MessageBox.Show($"Пароль для пользователя {username} успешно изменен", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления пароля для пользователя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
