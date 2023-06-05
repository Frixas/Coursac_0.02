using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WindowsFormsApp1
{
    public partial class AdminForm : Form
    {
        private SqlConnection connection;
        private string userRole;

        public AdminForm(string userRole)
        {
            InitializeComponent();
            string databaseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pizzaCAN.mdf");
            string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={databaseFile};Integrated Security=True";
            connection = new SqlConnection(connectionString);
            this.userRole = userRole;
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            LoadUsers();
            LoadEventLogData();
        }

        private void LoadUsers()
        {
            try
            {
                // Подключение к базе данных
                connection.Open();

                // Загрузка списка пользователей из базы данных
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
                // Закрытие подключения
                connection.Close();
            }
        }

        private void LoadEventLogData()
        {
            try
            {
                connection.Open();

                // Загрузка событий из базы данных
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

                            // Панель для логов
                            Panel logPanel = new Panel();
                            logPanel.BorderStyle = BorderStyle.FixedSingle;
                            logPanel.Padding = new Padding(10);
                            logPanel.Margin = new Padding(5);
                            logPanel.Width = 250;

                            // label с именем
                            Label usernameLabel = new Label();
                            usernameLabel.Text = $"Username: {username}";
                            usernameLabel.AutoSize = true;
                            logPanel.Controls.Add(usernameLabel);

                            // label с временем
                            Label loginTimeLabel = new Label();
                            loginTimeLabel.Text = $"Login Time: {loginTime.ToString("dd.MM.yyyy HH:mm:ss")}";
                            loginTimeLabel.AutoSize = true;
                            loginTimeLabel.Top = usernameLabel.Bottom + 5; 
                            logPanel.Controls.Add(loginTimeLabel);

                            // logid
                            Label logIdLabel = new Label();
                            logIdLabel.Text = $"Log ID: {logId}";
                            logIdLabel.AutoSize = true;
                            logIdLabel.Top = loginTimeLabel.Bottom + 5; 
                            logPanel.Controls.Add(logIdLabel);

                            logPanel.Height = logIdLabel.Bottom + 10;

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
                connection.Close();
            }
            flowLayoutPanelEventLog.AutoScroll = true;
        }

        private void cbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUser = cbUsers.SelectedItem?.ToString();

            try
            {
                connection.Open();

                // Загрузка данных о входе по конкретному пользователю
                string selectQuery = "SELECT LogId, Username, LoginTime FROM EventLog WHERE Username = @Username";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", selectedUser);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        flowLayoutPanelEventLogDD.Controls.Clear();

                        while (reader.Read())
                        {
                            int logId = Convert.ToInt32(reader["LogId"]);
                            string username = reader["Username"].ToString();
                            DateTime loginTime = Convert.ToDateTime(reader["LoginTime"]);

                            Panel logPanel = new Panel();
                            logPanel.BorderStyle = BorderStyle.FixedSingle;
                            logPanel.Padding = new Padding(10);
                            logPanel.Margin = new Padding(5);
                            logPanel.Width = 250;

                            Label logIdLabel = new Label();
                            logIdLabel.Text = $"Log ID: {logId}";
                            logIdLabel.AutoSize = true;
                            logPanel.Controls.Add(logIdLabel);

                            Label usernameLabel = new Label();
                            usernameLabel.Text = $"Username: {username}";
                            usernameLabel.AutoSize = true;
                            usernameLabel.Top = logIdLabel.Bottom + 5;
                            logPanel.Controls.Add(usernameLabel);

                            Label loginTimeLabel = new Label();
                            loginTimeLabel.Text = $"Login Time: {loginTime.ToString("dd.MM.yyyy HH:mm:ss")}";
                            loginTimeLabel.AutoSize = true;
                            loginTimeLabel.Top = usernameLabel.Bottom + 5; 
                            logPanel.Controls.Add(loginTimeLabel);

                            logPanel.Height = loginTimeLabel.Bottom + 10;

                            flowLayoutPanelEventLogDD.Controls.Add(logPanel);
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

        private void btnGenerateEventLogReport_Click(object sender, EventArgs e)
        {
            try
            {
                // Открытие Excel
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = true;

                // Создание новых листов
                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = workbook.ActiveSheet;

                // Загаловки
                for (int i = 0; i < flowLayoutPanelEventLog.Controls.Count; i++)
                {
                    Panel logPanel = (Panel)flowLayoutPanelEventLog.Controls[i];
                    Label logIdLabel = (Label)logPanel.Controls[0];
                    Label usernameLabel = (Label)logPanel.Controls[1];
                    Label loginTimeLabel = (Label)logPanel.Controls[2];

                    worksheet.Cells[1, 1] = "Log ID";
                    worksheet.Cells[1, 2] = "Username";
                    worksheet.Cells[1, 3] = "Login Time";

                    // Заполнение данными
                    worksheet.Cells[i + 2, 1] = logIdLabel.Text.Replace("Log ID: ", "");
                    worksheet.Cells[i + 2, 2] = usernameLabel.Text.Replace("Username: ", "");
                    worksheet.Cells[i + 2, 3] = loginTimeLabel.Text.Replace("Login Time: ", "");
                }

                // Сохранение в excel
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

                // Закрытие Excel
                excelApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации отчета: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDirectorForm_Click(object sender, EventArgs e)
        {
            // Check user role
            if (userRole == "director")
            {
                // Open DirectorForm
                DirectorForm directorForm = new DirectorForm(userRole);
                directorForm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("У Вас недостаточно прав!", "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerateEventLogReportDD_Click(object sender, EventArgs e)
        {
            string selectedUser = cbUsers.SelectedItem?.ToString();
            DateTime selectedDate = datePickerEventLog.Value.Date;

            try
            {
                connection.Open();

                // Загрузка событий входа для конкретного пользователя
                string selectQuery = "SELECT LogId, Username, LoginTime FROM EventLog WHERE Username = @Username AND LoginTime >= @StartDate AND LoginTime < @EndDate";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", selectedUser);
                    command.Parameters.AddWithValue("@StartDate", selectedDate);
                    command.Parameters.AddWithValue("@EndDate", selectedDate.AddDays(1));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Excel.Application excelApp = new Excel.Application();
                        excelApp.Visible = true;

                        Excel.Workbook workbook = excelApp.Workbooks.Add();
                        Excel.Worksheet worksheet = workbook.ActiveSheet;

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            worksheet.Cells[1, i + 1] = reader.GetName(i);
                        }

                        int row = 2; 

                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                object value = reader.GetValue(i);
                                worksheet.Cells[row, i + 1] = value != null ? value.ToString() : string.Empty;
                            }

                            row++;
                        }

                        worksheet.Columns.AutoFit();

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

                        excelApp.Quit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка генерации отчета: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void flowLayoutPanelEventLog_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
