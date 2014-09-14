namespace Sapper
{
    partial class SaveLoadForm
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
            this.saves = new System.Windows.Forms.DataGridView();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.save = new System.Windows.Forms.Button();
            this.load = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.saves)).BeginInit();
            this.SuspendLayout();
            // 
            // saves
            // 
            this.saves.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.saves.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.saves.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.saves.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.date});
            this.saves.Location = new System.Drawing.Point(48, 51);
            this.saves.MultiSelect = false;
            this.saves.Name = "saves";
            this.saves.ReadOnly = true;
            this.saves.RowHeadersVisible = false;
            this.saves.RowTemplate.Height = 24;
            this.saves.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.saves.Size = new System.Drawing.Size(352, 218);
            this.saves.TabIndex = 0;
            // 
            // name
            // 
            this.name.HeaderText = "Название сохранения";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // date
            // 
            this.date.HeaderText = "Дата/Время";
            this.date.Name = "date";
            this.date.ReadOnly = true;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(422, 316);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(89, 23);
            this.save.TabIndex = 1;
            this.save.Text = "Сохранить";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // load
            // 
            this.load.Location = new System.Drawing.Point(422, 51);
            this.load.Name = "load";
            this.load.Size = new System.Drawing.Size(89, 23);
            this.load.TabIndex = 2;
            this.load.Text = "Загрузить";
            this.load.UseVisualStyleBackColor = true;
            this.load.Click += new System.EventHandler(this.load_Click);
            // 
            // remove
            // 
            this.remove.Location = new System.Drawing.Point(422, 97);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(89, 23);
            this.remove.TabIndex = 3;
            this.remove.Text = "Удалить";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(237, 317);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(167, 22);
            this.textBox1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 322);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Введите название сохранения";
            // 
            // SaveLoadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 446);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.load);
            this.Controls.Add(this.save);
            this.Controls.Add(this.saves);
            this.Name = "SaveLoadForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SaveLoadForm";
            this.Load += new System.EventHandler(this.SaveLoadForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.saves)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView saves;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button load;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}