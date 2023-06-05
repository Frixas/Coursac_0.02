namespace WindowsFormsApp1
{
    partial class AdminForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGenerateEventLogReport = new System.Windows.Forms.Button();
            this.cbUsers = new System.Windows.Forms.ComboBox();
            this.datePickerEventLog = new System.Windows.Forms.DateTimePicker();
            this.btnDirectorForm = new System.Windows.Forms.Button();
            this.btnGenerateEventLogReportDD = new System.Windows.Forms.Button();
            this.flowLayoutPanelEventLog = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelEventLogDD = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // btnGenerateEventLogReport
            // 
            this.btnGenerateEventLogReport.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnGenerateEventLogReport.FlatAppearance.BorderSize = 0;
            this.btnGenerateEventLogReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateEventLogReport.Location = new System.Drawing.Point(319, 94);
            this.btnGenerateEventLogReport.Name = "btnGenerateEventLogReport";
            this.btnGenerateEventLogReport.Size = new System.Drawing.Size(121, 23);
            this.btnGenerateEventLogReport.TabIndex = 3;
            this.btnGenerateEventLogReport.Text = "Генерация отчёта Событий";
            this.btnGenerateEventLogReport.UseVisualStyleBackColor = false;
            this.btnGenerateEventLogReport.Click += new System.EventHandler(this.btnGenerateEventLogReport_Click);
            // 
            // cbUsers
            // 
            this.cbUsers.BackColor = System.Drawing.SystemColors.ControlDark;
            this.cbUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUsers.FormattingEnabled = true;
            this.cbUsers.Location = new System.Drawing.Point(319, 41);
            this.cbUsers.Name = "cbUsers";
            this.cbUsers.Size = new System.Drawing.Size(121, 21);
            this.cbUsers.TabIndex = 4;
            this.cbUsers.SelectedIndexChanged += new System.EventHandler(this.cbUsers_SelectedIndexChanged);
            // 
            // datePickerEventLog
            // 
            this.datePickerEventLog.CalendarForeColor = System.Drawing.SystemColors.ControlDark;
            this.datePickerEventLog.CalendarMonthBackground = System.Drawing.SystemColors.ControlDark;
            this.datePickerEventLog.Location = new System.Drawing.Point(319, 68);
            this.datePickerEventLog.Name = "datePickerEventLog";
            this.datePickerEventLog.Size = new System.Drawing.Size(121, 20);
            this.datePickerEventLog.TabIndex = 5;
            // 
            // btnDirectorForm
            // 
            this.btnDirectorForm.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnDirectorForm.FlatAppearance.BorderSize = 0;
            this.btnDirectorForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDirectorForm.Location = new System.Drawing.Point(315, 12);
            this.btnDirectorForm.Name = "btnDirectorForm";
            this.btnDirectorForm.Size = new System.Drawing.Size(125, 23);
            this.btnDirectorForm.TabIndex = 8;
            this.btnDirectorForm.Text = "Директор";
            this.btnDirectorForm.UseVisualStyleBackColor = false;
            this.btnDirectorForm.Click += new System.EventHandler(this.btnDirectorForm_Click);
            // 
            // btnGenerateEventLogReportDD
            // 
            this.btnGenerateEventLogReportDD.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnGenerateEventLogReportDD.FlatAppearance.BorderSize = 0;
            this.btnGenerateEventLogReportDD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateEventLogReportDD.Location = new System.Drawing.Point(319, 123);
            this.btnGenerateEventLogReportDD.Name = "btnGenerateEventLogReportDD";
            this.btnGenerateEventLogReportDD.Size = new System.Drawing.Size(121, 38);
            this.btnGenerateEventLogReportDD.TabIndex = 9;
            this.btnGenerateEventLogReportDD.Text = "Генерация отчёта Событий(по дню)";
            this.btnGenerateEventLogReportDD.UseVisualStyleBackColor = false;
            this.btnGenerateEventLogReportDD.Click += new System.EventHandler(this.btnGenerateEventLogReportDD_Click);
            // 
            // flowLayoutPanelEventLog
            // 
            this.flowLayoutPanelEventLog.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.flowLayoutPanelEventLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanelEventLog.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanelEventLog.Name = "flowLayoutPanelEventLog";
            this.flowLayoutPanelEventLog.Size = new System.Drawing.Size(297, 168);
            this.flowLayoutPanelEventLog.TabIndex = 11;
            this.flowLayoutPanelEventLog.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanelEventLog_Paint);
            // 
            // flowLayoutPanelEventLogDD
            // 
            this.flowLayoutPanelEventLogDD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanelEventLogDD.Location = new System.Drawing.Point(12, 186);
            this.flowLayoutPanelEventLogDD.Name = "flowLayoutPanelEventLogDD";
            this.flowLayoutPanelEventLogDD.Size = new System.Drawing.Size(269, 211);
            this.flowLayoutPanelEventLogDD.TabIndex = 12;
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(452, 410);
            this.Controls.Add(this.flowLayoutPanelEventLogDD);
            this.Controls.Add(this.flowLayoutPanelEventLog);
            this.Controls.Add(this.btnGenerateEventLogReportDD);
            this.Controls.Add(this.btnDirectorForm);
            this.Controls.Add(this.datePickerEventLog);
            this.Controls.Add(this.cbUsers);
            this.Controls.Add(this.btnGenerateEventLogReport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AdminForm";
            this.Text = "PizzaCAN-Администратор";
            this.Load += new System.EventHandler(this.AdminForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnGenerateEventLogReport;
        private System.Windows.Forms.ComboBox cbUsers;
        private System.Windows.Forms.DateTimePicker datePickerEventLog;
        private System.Windows.Forms.Button btnDirectorForm;
        private System.Windows.Forms.Button btnGenerateEventLogReportDD;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelEventLog;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelEventLogDD;
    }
}