namespace Reflection
{
    partial class Form1
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
            this.gbTextBoxes = new System.Windows.Forms.GroupBox();
            this.btnPobierz = new System.Windows.Forms.Button();
            this.gbFinanse = new System.Windows.Forms.GroupBox();
            this.gbUlubione = new System.Windows.Forms.GroupBox();
            this.gbPracownicy = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // gbTextBoxes
            // 
            this.gbTextBoxes.Location = new System.Drawing.Point(12, 12);
            this.gbTextBoxes.Name = "gbTextBoxes";
            this.gbTextBoxes.Padding = new System.Windows.Forms.Padding(4);
            this.gbTextBoxes.Size = new System.Drawing.Size(334, 513);
            this.gbTextBoxes.TabIndex = 0;
            this.gbTextBoxes.TabStop = false;
            this.gbTextBoxes.Text = "Dane podstawowe";
            // 
            // btnPobierz
            // 
            this.btnPobierz.Location = new System.Drawing.Point(481, 502);
            this.btnPobierz.Name = "btnPobierz";
            this.btnPobierz.Size = new System.Drawing.Size(75, 23);
            this.btnPobierz.TabIndex = 0;
            this.btnPobierz.Text = "Pobierz";
            this.btnPobierz.UseVisualStyleBackColor = true;
            this.btnPobierz.Click += new System.EventHandler(this.btnPobierz_Click);
            // 
            // gbFinanse
            // 
            this.gbFinanse.Location = new System.Drawing.Point(352, 12);
            this.gbFinanse.Name = "gbFinanse";
            this.gbFinanse.Padding = new System.Windows.Forms.Padding(4);
            this.gbFinanse.Size = new System.Drawing.Size(334, 476);
            this.gbFinanse.TabIndex = 1;
            this.gbFinanse.TabStop = false;
            this.gbFinanse.Text = "Inne dane";
            // 
            // gbUlubione
            // 
            this.gbUlubione.Location = new System.Drawing.Point(692, 12);
            this.gbUlubione.Name = "gbUlubione";
            this.gbUlubione.Padding = new System.Windows.Forms.Padding(4);
            this.gbUlubione.Size = new System.Drawing.Size(274, 187);
            this.gbUlubione.TabIndex = 2;
            this.gbUlubione.TabStop = false;
            this.gbUlubione.Text = "Ulubione";
            // 
            // gbPracownicy
            // 
            this.gbPracownicy.Location = new System.Drawing.Point(693, 205);
            this.gbPracownicy.Name = "gbPracownicy";
            this.gbPracownicy.Size = new System.Drawing.Size(273, 178);
            this.gbPracownicy.TabIndex = 3;
            this.gbPracownicy.TabStop = false;
            this.gbPracownicy.Text = "Pracownicy";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(562, 502);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Update View";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(678, 502);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(116, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Update Model";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 537);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gbPracownicy);
            this.Controls.Add(this.btnPobierz);
            this.Controls.Add(this.gbUlubione);
            this.Controls.Add(this.gbFinanse);
            this.Controls.Add(this.gbTextBoxes);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTextBoxes;
        private System.Windows.Forms.Button btnPobierz;
        private System.Windows.Forms.GroupBox gbFinanse;
        private System.Windows.Forms.GroupBox gbUlubione;
        private System.Windows.Forms.GroupBox gbPracownicy;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

