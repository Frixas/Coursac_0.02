namespace WindowsFormsApp1
{
    partial class DirectorForm
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
            this.datePickerEventLog = new System.Windows.Forms.DateTimePicker();
            this.cbUsers = new System.Windows.Forms.ComboBox();
            this.btnGenerateEventLogReport = new System.Windows.Forms.Button();
            this.comboBoxUsers = new System.Windows.Forms.ComboBox();
            this.cbRoles = new System.Windows.Forms.ComboBox();
            this.btnSaveRole = new System.Windows.Forms.Button();
            this.flowLayoutUsers = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSavePassword = new System.Windows.Forms.Button();
            this.flowLayoutPanelEventLog = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutUsers.SuspendLayout();
            this.SuspendLayout();
            // 
            // datePickerEventLog
            // 
            this.datePickerEventLog.CalendarForeColor = System.Drawing.SystemColors.ControlDark;
            this.datePickerEventLog.CalendarMonthBackground = System.Drawing.SystemColors.ControlDark;
            this.datePickerEventLog.Location = new System.Drawing.Point(153, 186);
            this.datePickerEventLog.Name = "datePickerEventLog";
            this.datePickerEventLog.Size = new System.Drawing.Size(176, 20);
            this.datePickerEventLog.TabIndex = 12;
            // 
            // cbUsers
            // 
            this.cbUsers.BackColor = System.Drawing.SystemColors.ControlDark;
            this.cbUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbUsers.FormattingEnabled = true;
            this.cbUsers.Location = new System.Drawing.Point(26, 185);
            this.cbUsers.Name = "cbUsers";
            this.cbUsers.Size = new System.Drawing.Size(121, 21);
            this.cbUsers.TabIndex = 11;
            // 
            // btnGenerateEventLogReport
            // 
            this.btnGenerateEventLogReport.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnGenerateEventLogReport.FlatAppearance.BorderSize = 0;
            this.btnGenerateEventLogReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateEventLogReport.Location = new System.Drawing.Point(26, 212);
            this.btnGenerateEventLogReport.Name = "btnGenerateEventLogReport";
            this.btnGenerateEventLogReport.Size = new System.Drawing.Size(165, 23);
            this.btnGenerateEventLogReport.TabIndex = 10;
            this.btnGenerateEventLogReport.Text = "Генерация отчёта Событий";
            this.btnGenerateEventLogReport.UseVisualStyleBackColor = false;
            this.btnGenerateEventLogReport.Click += new System.EventHandler(this.btnGenerateEventLogReport_Click);
            // 
            // comboBoxUsers
            // 
            this.comboBoxUsers.BackColor = System.Drawing.SystemColors.ControlDark;
            this.comboBoxUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxUsers.FormattingEnabled = true;
            this.comboBoxUsers.Location = new System.Drawing.Point(633, 12);
            this.comboBoxUsers.Name = "comboBoxUsers";
            this.comboBoxUsers.Size = new System.Drawing.Size(159, 21);
            this.comboBoxUsers.TabIndex = 13;
            // 
            // cbRoles
            // 
            this.cbRoles.BackColor = System.Drawing.SystemColors.ControlDark;
            this.cbRoles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbRoles.FormattingEnabled = true;
            this.cbRoles.Location = new System.Drawing.Point(633, 50);
            this.cbRoles.Name = "cbRoles";
            this.cbRoles.Size = new System.Drawing.Size(159, 21);
            this.cbRoles.TabIndex = 14;
            // 
            // btnSaveRole
            // 
            this.btnSaveRole.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnSaveRole.FlatAppearance.BorderSize = 0;
            this.btnSaveRole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveRole.Location = new System.Drawing.Point(633, 77);
            this.btnSaveRole.Name = "btnSaveRole";
            this.btnSaveRole.Size = new System.Drawing.Size(159, 28);
            this.btnSaveRole.TabIndex = 15;
            this.btnSaveRole.Text = "Назначить роль";
            this.btnSaveRole.UseVisualStyleBackColor = false;
            this.btnSaveRole.Click += new System.EventHandler(this.btnSaveRole_Click);
            // 
            // flowLayoutUsers
            // 
            this.flowLayoutUsers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutUsers.Controls.Add(this.btnSavePassword);
            this.flowLayoutUsers.Location = new System.Drawing.Point(437, 169);
            this.flowLayoutUsers.Name = "flowLayoutUsers";
            this.flowLayoutUsers.Size = new System.Drawing.Size(355, 205);
            this.flowLayoutUsers.TabIndex = 16;
            // 
            // btnSavePassword
            // 
            this.btnSavePassword.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnSavePassword.FlatAppearance.BorderSize = 0;
            this.btnSavePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSavePassword.Location = new System.Drawing.Point(3, 3);
            this.btnSavePassword.Name = "btnSavePassword";
            this.btnSavePassword.Size = new System.Drawing.Size(75, 23);
            this.btnSavePassword.TabIndex = 0;
            this.btnSavePassword.Text = "button1";
            this.btnSavePassword.UseVisualStyleBackColor = false;
            // 
            // flowLayoutPanelEventLog
            // 
            this.flowLayoutPanelEventLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanelEventLog.Location = new System.Drawing.Point(26, 11);
            this.flowLayoutPanelEventLog.Name = "flowLayoutPanelEventLog";
            this.flowLayoutPanelEventLog.Size = new System.Drawing.Size(297, 168);
            this.flowLayoutPanelEventLog.TabIndex = 17;
            // 
            // DirectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(804, 399);
            this.Controls.Add(this.flowLayoutPanelEventLog);
            this.Controls.Add(this.flowLayoutUsers);
            this.Controls.Add(this.btnSaveRole);
            this.Controls.Add(this.cbRoles);
            this.Controls.Add(this.comboBoxUsers);
            this.Controls.Add(this.datePickerEventLog);
            this.Controls.Add(this.cbUsers);
            this.Controls.Add(this.btnGenerateEventLogReport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DirectorForm";
            this.Text = "PizzaCAN-Директор";
            this.Load += new System.EventHandler(this.DirectorForm_Load);
            this.flowLayoutUsers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DateTimePicker datePickerEventLog;
        private System.Windows.Forms.ComboBox cbUsers;
        private System.Windows.Forms.Button btnGenerateEventLogReport;
        private System.Windows.Forms.ComboBox comboBoxUsers;
        private System.Windows.Forms.ComboBox cbRoles;
        private System.Windows.Forms.Button btnSaveRole;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutUsers;
        private System.Windows.Forms.Button btnSavePassword;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelEventLog;
    }
}