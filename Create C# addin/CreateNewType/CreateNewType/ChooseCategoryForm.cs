using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CreateNewType
{
    // Token: 0x02000003 RID: 3
    public class ChooseCategoryForm : Form
    {
        // Token: 0x0600000E RID: 14 RVA: 0x00002928 File Offset: 0x00000B28
        public ChooseCategoryForm(Command command)
        {
            this.m_Command = command;
            this.InitializeComponent();
            string[] dataSource = new string[]
            {
                "Structural Columns",
                "Structural Framing",
                "Structural Foundations",
                "Doors",
                "Windows",
                "Generic models"
            };
            this.ComboBox_TypeOf.DataSource = dataSource;
        }

        // Token: 0x0600000F RID: 15 RVA: 0x0000299D File Offset: 0x00000B9D
        private void button_OK_Click(object sender, EventArgs e)
        {
            this.m_Command.CategoryName = (this.ComboBox_TypeOf.SelectedValue as string);
            this.m_Command.filePath = this.txtFileName.Text;
        }

        // Token: 0x06000010 RID: 16 RVA: 0x000029D4 File Offset: 0x00000BD4
        private void button_filepath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel 97-2003 Workbook|*.xls|Excel Workbook|*xlsx"
            })
            {
                bool flag = openFileDialog.ShowDialog() == DialogResult.OK;
                if (flag)
                {
                    this.txtFileName.Text = openFileDialog.FileName;
                }
            }
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00002A34 File Offset: 0x00000C34
        protected override void Dispose(bool disposing)
        {
            bool flag = disposing && this.components != null;
            if (flag)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00002A6C File Offset: 0x00000C6C
        private void InitializeComponent()
        {
            this.button_Cancel = new Button();
            this.ComboBox_TypeOf = new ComboBox();
            this.button_OK = new Button();
            this.groupBox1 = new GroupBox();
            this.txtFileName = new TextBox();
            this.groupBox2 = new GroupBox();
            this.button_filepath = new Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.button_Cancel.DialogResult = DialogResult.Cancel;
            this.button_Cancel.Location = new Point(282, 150);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new Size(75, 23);
            this.button_Cancel.TabIndex = 2;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.ComboBox_TypeOf.FormattingEnabled = true;
            this.ComboBox_TypeOf.Location = new Point(6, 19);
            this.ComboBox_TypeOf.Name = "ComboBox_TypeOf";
            this.ComboBox_TypeOf.Size = new Size(339, 21);
            this.ComboBox_TypeOf.TabIndex = 3;
            this.button_OK.DialogResult = DialogResult.OK;
            this.button_OK.Location = new Point(201, 150);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new Size(75, 23);
            this.button_OK.TabIndex = 4;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += this.button_OK_Click;
            this.groupBox1.Controls.Add(this.ComboBox_TypeOf);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(351, 54);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Types of Category";
            this.txtFileName.Location = new Point(6, 19);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new Size(258, 20);
            this.txtFileName.TabIndex = 6;
            this.groupBox2.Controls.Add(this.button_filepath);
            this.groupBox2.Controls.Add(this.txtFileName);
            this.groupBox2.Location = new Point(12, 73);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(351, 53);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Excel path";
            this.button_filepath.AutoEllipsis = true;
            this.button_filepath.Location = new Point(270, 19);
            this.button_filepath.Name = "button_filepath";
            this.button_filepath.Size = new Size(75, 20);
            this.button_filepath.TabIndex = 7;
            this.button_filepath.Text = "...";
            this.button_filepath.UseVisualStyleBackColor = true;
            this.button_filepath.Click += this.button_filepath_Click;
            base.AcceptButton = this.button_OK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.button_Cancel;
            base.ClientSize = new Size(375, 185);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.button_OK);
            base.Controls.Add(this.button_Cancel);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ChooseCategoryForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Create types";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            base.ResumeLayout(false);
        }

        // Token: 0x0400000D RID: 13
        private Command m_Command = null;

        // Token: 0x0400000E RID: 14
        private IContainer components = null;

        // Token: 0x0400000F RID: 15
        private Button button_Cancel;

        // Token: 0x04000010 RID: 16
        private ComboBox ComboBox_TypeOf;

        // Token: 0x04000011 RID: 17
        private Button button_OK;

        // Token: 0x04000012 RID: 18
        private GroupBox groupBox1;

        // Token: 0x04000013 RID: 19
        private TextBox txtFileName;

        // Token: 0x04000014 RID: 20
        private GroupBox groupBox2;

        // Token: 0x04000015 RID: 21
        private Button button_filepath;
    }
}
