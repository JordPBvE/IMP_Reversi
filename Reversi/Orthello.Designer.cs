namespace Reversi
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
            this.boardpanel = new System.Windows.Forms.Panel();
            this.scorepanel = new System.Windows.Forms.Panel();
            this.redscorelabel = new System.Windows.Forms.Label();
            this.bluescorelabel = new System.Windows.Forms.Label();
            this.gamestatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.boardpanel.Location = new System.Drawing.Point(64, 256);
            this.boardpanel.Name = "panel1";
            this.boardpanel.Size = new System.Drawing.Size(858, 800);
            this.boardpanel.TabIndex = 0;
            // 
            // panel2
            // 
            this.scorepanel.Location = new System.Drawing.Point(144, 79);
            this.scorepanel.Name = "panel2";
            this.scorepanel.Size = new System.Drawing.Size(241, 118);
            this.scorepanel.TabIndex = 1;
            // 
            // textBox1
            // 
            this.redscorelabel.BackColor = System.Drawing.SystemColors.Control;
            this.redscorelabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.redscorelabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.redscorelabel.Location = new System.Drawing.Point(64, 92);
            this.redscorelabel.Name = "textBox1";
            this.redscorelabel.Size = new System.Drawing.Size(78, 31);
            this.redscorelabel.TabIndex = 2;
            this.redscorelabel.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.bluescorelabel.BackColor = System.Drawing.SystemColors.Control;
            this.bluescorelabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.bluescorelabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bluescorelabel.Location = new System.Drawing.Point(64, 139);
            this.bluescorelabel.Name = "textBox2";
            this.bluescorelabel.Size = new System.Drawing.Size(54, 31);
            this.bluescorelabel.TabIndex = 3;
            this.bluescorelabel.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // textBox3
            // 
            this.gamestatus.BackColor = System.Drawing.SystemColors.Control;
            this.gamestatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gamestatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gamestatus.Location = new System.Drawing.Point(64, 203);
            this.gamestatus.Name = "textBox3";
            this.gamestatus.Size = new System.Drawing.Size(266, 31);
            this.gamestatus.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 1055);
            this.Controls.Add(this.gamestatus);
            this.Controls.Add(this.bluescorelabel);
            this.Controls.Add(this.redscorelabel);
            this.Controls.Add(this.scorepanel);
            this.Controls.Add(this.boardpanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel boardpanel;
        private System.Windows.Forms.Panel scorepanel;
        private System.Windows.Forms.Label redscorelabel;
        private System.Windows.Forms.Label bluescorelabel;
        private System.Windows.Forms.Label gamestatus;
    }
}

