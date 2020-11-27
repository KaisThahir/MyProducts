namespace MadaTec
{
    partial class Reporting
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
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxCustomerName = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxItemName = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.textBoxElementName = new System.Windows.Forms.TextBox();
            this.dateTimePickerTo2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerFrom2 = new System.Windows.Forms.DateTimePicker();
            this.btnGeneral = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "جرد عميل";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxCustomerName
            // 
            this.textBoxCustomerName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBoxCustomerName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxCustomerName.Location = new System.Drawing.Point(107, 14);
            this.textBoxCustomerName.Name = "textBoxCustomerName";
            this.textBoxCustomerName.Size = new System.Drawing.Size(158, 20);
            this.textBoxCustomerName.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "تقرير مبيعات";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(107, 44);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(116, 20);
            this.dateTimePickerFrom.TabIndex = 3;
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerTo.Location = new System.Drawing.Point(229, 44);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(116, 20);
            this.dateTimePickerTo.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 71);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "تكلفة الجهاز";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBoxItemName
            // 
            this.textBoxItemName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBoxItemName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxItemName.Location = new System.Drawing.Point(107, 71);
            this.textBoxItemName.Name = "textBoxItemName";
            this.textBoxItemName.Size = new System.Drawing.Size(100, 20);
            this.textBoxItemName.TabIndex = 6;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(13, 101);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "شراء مادة";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBoxElementName
            // 
            this.textBoxElementName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.textBoxElementName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxElementName.Location = new System.Drawing.Point(107, 101);
            this.textBoxElementName.Name = "textBoxElementName";
            this.textBoxElementName.Size = new System.Drawing.Size(100, 20);
            this.textBoxElementName.TabIndex = 8;
            // 
            // dateTimePickerTo2
            // 
            this.dateTimePickerTo2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerTo2.Location = new System.Drawing.Point(229, 129);
            this.dateTimePickerTo2.Name = "dateTimePickerTo2";
            this.dateTimePickerTo2.Size = new System.Drawing.Size(116, 20);
            this.dateTimePickerTo2.TabIndex = 10;
            // 
            // dateTimePickerFrom2
            // 
            this.dateTimePickerFrom2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerFrom2.Location = new System.Drawing.Point(107, 129);
            this.dateTimePickerFrom2.Name = "dateTimePickerFrom2";
            this.dateTimePickerFrom2.Size = new System.Drawing.Size(116, 20);
            this.dateTimePickerFrom2.TabIndex = 9;
            // 
            // btnGeneral
            // 
            this.btnGeneral.Location = new System.Drawing.Point(13, 130);
            this.btnGeneral.Name = "btnGeneral";
            this.btnGeneral.Size = new System.Drawing.Size(75, 23);
            this.btnGeneral.TabIndex = 11;
            this.btnGeneral.Text = "General";
            this.btnGeneral.UseVisualStyleBackColor = true;
            this.btnGeneral.Click += new System.EventHandler(this.btnGeneral_Click);
            // 
            // Reporting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 321);
            this.Controls.Add(this.btnGeneral);
            this.Controls.Add(this.dateTimePickerTo2);
            this.Controls.Add(this.dateTimePickerFrom2);
            this.Controls.Add(this.textBoxElementName);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBoxItemName);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBoxCustomerName);
            this.Controls.Add(this.button1);
            this.Name = "Reporting";
            this.Text = "Reporting";
            this.Load += new System.EventHandler(this.Reporting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxCustomerName;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBoxItemName;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBoxElementName;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo2;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom2;
        private System.Windows.Forms.Button btnGeneral;
    }
}