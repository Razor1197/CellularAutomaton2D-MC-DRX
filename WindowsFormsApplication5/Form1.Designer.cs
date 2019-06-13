namespace WindowsFormsApplication5
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.homogeneous_button = new System.Windows.Forms.Button();
            this.Radius_button = new System.Windows.Forms.Button();
            this.Random_button = new System.Windows.Forms.Button();
            this.Stop_button = new System.Windows.Forms.Button();
            this.Simulation_button = new System.Windows.Forms.Button();
            this.State_box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Radius_box = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Mesh_box = new System.Windows.Forms.CheckBox();
            this.Empty_box = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.Column_box = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Row_box = new System.Windows.Forms.TextBox();
            this.Neigbourhood_box = new System.Windows.Forms.ComboBox();
            this.Neighbourhood_label = new System.Windows.Forms.Label();
            this.MC_button = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.MC_Iterations_box = new System.Windows.Forms.TextBox();
            this.Cells_button = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.kt_box = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.Percent_box = new System.Windows.Forms.TextBox();
            this.DeltaT_box = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.Recrystalization_button = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.B_box = new System.Windows.Forms.TextBox();
            this.A_box = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.Energy_button = new System.Windows.Forms.Button();
            this.Dislocation_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.DarkGray;
            this.pictureBox1.Location = new System.Drawing.Point(16, 125);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1221, 743);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UserClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Width";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 95);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(132, 22);
            this.textBox1.TabIndex = 10;
            this.textBox1.Text = "30";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(159, 95);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(132, 22);
            this.textBox2.TabIndex = 12;
            this.textBox2.Text = "30";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(159, 75);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "Height";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Periodic",
            "Absorbing"});
            this.comboBox2.Location = new System.Drawing.Point(19, 30);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(132, 24);
            this.comboBox2.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 17);
            this.label4.TabIndex = 13;
            this.label4.Text = "Boundary Conditions";
            // 
            // homogeneous_button
            // 
            this.homogeneous_button.Location = new System.Drawing.Point(510, 2);
            this.homogeneous_button.Name = "homogeneous_button";
            this.homogeneous_button.Size = new System.Drawing.Size(106, 32);
            this.homogeneous_button.TabIndex = 15;
            this.homogeneous_button.Text = "Homogenous";
            this.homogeneous_button.UseVisualStyleBackColor = true;
            this.homogeneous_button.Click += new System.EventHandler(this.Homogenous_button_Click);
            // 
            // Radius_button
            // 
            this.Radius_button.Location = new System.Drawing.Point(510, 31);
            this.Radius_button.Name = "Radius_button";
            this.Radius_button.Size = new System.Drawing.Size(106, 32);
            this.Radius_button.TabIndex = 16;
            this.Radius_button.Text = "Radius";
            this.Radius_button.UseVisualStyleBackColor = true;
            this.Radius_button.Click += new System.EventHandler(this.Radius_button_Click);
            // 
            // Random_button
            // 
            this.Random_button.Location = new System.Drawing.Point(510, 60);
            this.Random_button.Name = "Random_button";
            this.Random_button.Size = new System.Drawing.Size(106, 32);
            this.Random_button.TabIndex = 17;
            this.Random_button.Text = "Random";
            this.Random_button.UseVisualStyleBackColor = true;
            this.Random_button.Click += new System.EventHandler(this.Random_button_Click);
            // 
            // Stop_button
            // 
            this.Stop_button.Location = new System.Drawing.Point(1025, 64);
            this.Stop_button.Margin = new System.Windows.Forms.Padding(4);
            this.Stop_button.Name = "Stop_button";
            this.Stop_button.Size = new System.Drawing.Size(181, 42);
            this.Stop_button.TabIndex = 19;
            this.Stop_button.Text = "Stop";
            this.Stop_button.UseVisualStyleBackColor = true;
            this.Stop_button.Click += new System.EventHandler(this.Stop_button_Click);
            // 
            // Simulation_button
            // 
            this.Simulation_button.Location = new System.Drawing.Point(1025, 14);
            this.Simulation_button.Margin = new System.Windows.Forms.Padding(4);
            this.Simulation_button.Name = "Simulation_button";
            this.Simulation_button.Size = new System.Drawing.Size(181, 42);
            this.Simulation_button.TabIndex = 20;
            this.Simulation_button.Text = "Simulation Start";
            this.Simulation_button.UseVisualStyleBackColor = true;
            this.Simulation_button.Click += new System.EventHandler(this.Simulation_button_Click);
            // 
            // State_box
            // 
            this.State_box.Location = new System.Drawing.Point(311, 96);
            this.State_box.Margin = new System.Windows.Forms.Padding(4);
            this.State_box.Name = "State_box";
            this.State_box.Size = new System.Drawing.Size(132, 22);
            this.State_box.TabIndex = 22;
            this.State_box.Text = "5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(308, 75);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 17);
            this.label1.TabIndex = 21;
            this.label1.Text = "Number of States";
            // 
            // Radius_box
            // 
            this.Radius_box.Location = new System.Drawing.Point(784, 30);
            this.Radius_box.Margin = new System.Windows.Forms.Padding(4);
            this.Radius_box.Name = "Radius_box";
            this.Radius_box.Size = new System.Drawing.Size(86, 22);
            this.Radius_box.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(781, 10);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 17);
            this.label5.TabIndex = 24;
            this.label5.Text = "Radius";
            // 
            // Mesh_box
            // 
            this.Mesh_box.AutoSize = true;
            this.Mesh_box.Checked = true;
            this.Mesh_box.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Mesh_box.Location = new System.Drawing.Point(784, 82);
            this.Mesh_box.Name = "Mesh_box";
            this.Mesh_box.Size = new System.Drawing.Size(64, 21);
            this.Mesh_box.TabIndex = 25;
            this.Mesh_box.Text = "Mesh";
            this.Mesh_box.UseVisualStyleBackColor = true;
            // 
            // Empty_box
            // 
            this.Empty_box.Location = new System.Drawing.Point(510, 87);
            this.Empty_box.Name = "Empty_box";
            this.Empty_box.Size = new System.Drawing.Size(106, 32);
            this.Empty_box.TabIndex = 26;
            this.Empty_box.Text = "Empty";
            this.Empty_box.UseVisualStyleBackColor = true;
            this.Empty_box.Click += new System.EventHandler(this.Empty_box_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(665, 9);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 17);
            this.label6.TabIndex = 28;
            this.label6.Text = "Columns";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // Column_box
            // 
            this.Column_box.Location = new System.Drawing.Point(665, 30);
            this.Column_box.Margin = new System.Windows.Forms.Padding(4);
            this.Column_box.Name = "Column_box";
            this.Column_box.Size = new System.Drawing.Size(86, 22);
            this.Column_box.TabIndex = 27;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(665, 63);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 17);
            this.label7.TabIndex = 30;
            this.label7.Text = "Rows";
            // 
            // Row_box
            // 
            this.Row_box.Location = new System.Drawing.Point(665, 84);
            this.Row_box.Margin = new System.Windows.Forms.Padding(4);
            this.Row_box.Name = "Row_box";
            this.Row_box.Size = new System.Drawing.Size(86, 22);
            this.Row_box.TabIndex = 29;
            // 
            // Neigbourhood_box
            // 
            this.Neigbourhood_box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Neigbourhood_box.FormattingEnabled = true;
            this.Neigbourhood_box.Items.AddRange(new object[] {
            "Neumann",
            "Moore",
            "Pentagonal Random",
            "Hexagonal Left",
            "Hexagonal Right",
            "Hexagonal Random",
            "Radius"});
            this.Neigbourhood_box.Location = new System.Drawing.Point(162, 30);
            this.Neigbourhood_box.Margin = new System.Windows.Forms.Padding(4);
            this.Neigbourhood_box.Name = "Neigbourhood_box";
            this.Neigbourhood_box.Size = new System.Drawing.Size(184, 24);
            this.Neigbourhood_box.TabIndex = 32;
            // 
            // Neighbourhood_label
            // 
            this.Neighbourhood_label.AutoSize = true;
            this.Neighbourhood_label.Location = new System.Drawing.Point(162, 10);
            this.Neighbourhood_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Neighbourhood_label.Name = "Neighbourhood_label";
            this.Neighbourhood_label.Size = new System.Drawing.Size(106, 17);
            this.Neighbourhood_label.TabIndex = 31;
            this.Neighbourhood_label.Text = "Neighbourhood";
            // 
            // MC_button
            // 
            this.MC_button.Enabled = false;
            this.MC_button.Location = new System.Drawing.Point(1251, 20);
            this.MC_button.Margin = new System.Windows.Forms.Padding(4);
            this.MC_button.Name = "MC_button";
            this.MC_button.Size = new System.Drawing.Size(178, 43);
            this.MC_button.TabIndex = 33;
            this.MC_button.Text = "Monte Carlo";
            this.MC_button.UseVisualStyleBackColor = true;
            this.MC_button.Click += new System.EventHandler(this.MC_button_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1282, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(160, 17);
            this.label8.TabIndex = 34;
            this.label8.Text = "Number of MC iterations";
            // 
            // MC_Iterations_box
            // 
            this.MC_Iterations_box.Location = new System.Drawing.Point(1285, 96);
            this.MC_Iterations_box.Name = "MC_Iterations_box";
            this.MC_Iterations_box.Size = new System.Drawing.Size(86, 22);
            this.MC_Iterations_box.TabIndex = 35;
            this.MC_Iterations_box.Text = "1";
            // 
            // Cells_button
            // 
            this.Cells_button.Location = new System.Drawing.Point(895, 14);
            this.Cells_button.Name = "Cells_button";
            this.Cells_button.Size = new System.Drawing.Size(103, 33);
            this.Cells_button.TabIndex = 36;
            this.Cells_button.Text = "Structure";
            this.Cells_button.UseVisualStyleBackColor = true;
            this.Cells_button.Click += new System.EventHandler(this.Cells_button_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1311, 125);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 17);
            this.label9.TabIndex = 38;
            this.label9.Text = "kt";
            // 
            // kt_box
            // 
            this.kt_box.Location = new System.Drawing.Point(1282, 143);
            this.kt_box.Margin = new System.Windows.Forms.Padding(4);
            this.kt_box.Name = "kt_box";
            this.kt_box.Size = new System.Drawing.Size(86, 22);
            this.kt_box.TabIndex = 37;
            this.kt_box.Text = "0,1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1282, 297);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(132, 17);
            this.label10.TabIndex = 43;
            this.label10.Text = "Percent to Package";
            // 
            // Percent_box
            // 
            this.Percent_box.Location = new System.Drawing.Point(1282, 318);
            this.Percent_box.Margin = new System.Windows.Forms.Padding(4);
            this.Percent_box.Name = "Percent_box";
            this.Percent_box.Size = new System.Drawing.Size(86, 22);
            this.Percent_box.TabIndex = 42;
            this.Percent_box.Text = "0,2";
            // 
            // DeltaT_box
            // 
            this.DeltaT_box.Location = new System.Drawing.Point(1285, 271);
            this.DeltaT_box.Name = "DeltaT_box";
            this.DeltaT_box.Size = new System.Drawing.Size(86, 22);
            this.DeltaT_box.TabIndex = 41;
            this.DeltaT_box.Text = "0,001";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1282, 250);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 17);
            this.label11.TabIndex = 40;
            this.label11.Text = "Delta t";
            // 
            // Recrystalization_button
            // 
            this.Recrystalization_button.Location = new System.Drawing.Point(1251, 192);
            this.Recrystalization_button.Margin = new System.Windows.Forms.Padding(4);
            this.Recrystalization_button.Name = "Recrystalization_button";
            this.Recrystalization_button.Size = new System.Drawing.Size(178, 43);
            this.Recrystalization_button.TabIndex = 39;
            this.Recrystalization_button.Text = "Recrystalization";
            this.Recrystalization_button.UseVisualStyleBackColor = true;
            this.Recrystalization_button.Click += new System.EventHandler(this.Recrystalization_button_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1279, 403);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 17);
            this.label12.TabIndex = 47;
            this.label12.Text = "B";
            // 
            // B_box
            // 
            this.B_box.Location = new System.Drawing.Point(1282, 424);
            this.B_box.Margin = new System.Windows.Forms.Padding(4);
            this.B_box.Name = "B_box";
            this.B_box.Size = new System.Drawing.Size(147, 22);
            this.B_box.TabIndex = 46;
            this.B_box.Text = "9,41268203527779";
            // 
            // A_box
            // 
            this.A_box.Location = new System.Drawing.Point(1282, 377);
            this.A_box.Name = "A_box";
            this.A_box.Size = new System.Drawing.Size(147, 22);
            this.A_box.TabIndex = 45;
            this.A_box.Text = "86710969050178,5";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1279, 356);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 17);
            this.label13.TabIndex = 44;
            this.label13.Text = "A";
            // 
            // Energy_button
            // 
            this.Energy_button.Location = new System.Drawing.Point(895, 47);
            this.Energy_button.Name = "Energy_button";
            this.Energy_button.Size = new System.Drawing.Size(103, 33);
            this.Energy_button.TabIndex = 48;
            this.Energy_button.Text = "Energy";
            this.Energy_button.UseVisualStyleBackColor = true;
            this.Energy_button.Click += new System.EventHandler(this.Energy_button_Click);
            // 
            // Dislocation_button
            // 
            this.Dislocation_button.Location = new System.Drawing.Point(895, 78);
            this.Dislocation_button.Name = "Dislocation_button";
            this.Dislocation_button.Size = new System.Drawing.Size(103, 33);
            this.Dislocation_button.TabIndex = 49;
            this.Dislocation_button.Text = "Dislocations";
            this.Dislocation_button.UseVisualStyleBackColor = true;
            this.Dislocation_button.Click += new System.EventHandler(this.Dislocation_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1451, 886);
            this.Controls.Add(this.Dislocation_button);
            this.Controls.Add(this.Energy_button);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.B_box);
            this.Controls.Add(this.A_box);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.Percent_box);
            this.Controls.Add(this.DeltaT_box);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.Recrystalization_button);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.kt_box);
            this.Controls.Add(this.Cells_button);
            this.Controls.Add(this.MC_Iterations_box);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.MC_button);
            this.Controls.Add(this.Neigbourhood_box);
            this.Controls.Add(this.Neighbourhood_label);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Row_box);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Column_box);
            this.Controls.Add(this.Empty_box);
            this.Controls.Add(this.Mesh_box);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Radius_box);
            this.Controls.Add(this.State_box);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Simulation_button);
            this.Controls.Add(this.Stop_button);
            this.Controls.Add(this.Random_button);
            this.Controls.Add(this.Radius_button);
            this.Controls.Add(this.homogeneous_button);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button homogeneous_button;
        private System.Windows.Forms.Button Radius_button;
        private System.Windows.Forms.Button Random_button;
        private System.Windows.Forms.Button Stop_button;
        private System.Windows.Forms.Button Simulation_button;
        private System.Windows.Forms.TextBox State_box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Radius_box;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox Mesh_box;
        private System.Windows.Forms.Button Empty_box;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Column_box;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox Row_box;
        private System.Windows.Forms.ComboBox Neigbourhood_box;
        private System.Windows.Forms.Label Neighbourhood_label;
        private System.Windows.Forms.Button MC_button;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox MC_Iterations_box;
        private System.Windows.Forms.Button Cells_button;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox kt_box;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox Percent_box;
        private System.Windows.Forms.TextBox DeltaT_box;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button Recrystalization_button;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox B_box;
        private System.Windows.Forms.TextBox A_box;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button Energy_button;
        private System.Windows.Forms.Button Dislocation_button;
    }
}

