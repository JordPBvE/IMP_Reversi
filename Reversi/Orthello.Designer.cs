namespace Reversi
{
    partial class Reversi
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
            this.gamestatus = new System.Windows.Forms.Label();
            this.hintbutton = new System.Windows.Forms.Button();
            this.resetbutton = new System.Windows.Forms.Button();
            this.nTrackbar = new System.Windows.Forms.TrackBar();
            this.bluescorelabel = new System.Windows.Forms.Label();
            this.sizelabel = new System.Windows.Forms.Label();
            this.player1 = new System.Windows.Forms.ComboBox();
            this.player2 = new System.Windows.Forms.ComboBox();
            this.player1label = new System.Windows.Forms.Label();
            this.player2label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nTrackbar)).BeginInit();
            this.SuspendLayout();
            // 
            // boardpanel
            // 
            this.boardpanel.Location = new System.Drawing.Point(70, 172);
            this.boardpanel.Name = "boardpanel";
            this.boardpanel.Size = new System.Drawing.Size(680, 621);
            this.boardpanel.TabIndex = 0;
            // 
            // scorepanel
            // 
            this.scorepanel.Location = new System.Drawing.Point(125, 17);
            this.scorepanel.Name = "scorepanel";
            this.scorepanel.Size = new System.Drawing.Size(97, 118);
            this.scorepanel.TabIndex = 1;
            // 
            // redscorelabel
            // 
            this.redscorelabel.BackColor = System.Drawing.SystemColors.Control;
            this.redscorelabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.redscorelabel.Location = new System.Drawing.Point(51, 24);
            this.redscorelabel.Name = "redscorelabel";
            this.redscorelabel.Size = new System.Drawing.Size(78, 31);
            this.redscorelabel.TabIndex = 2;
            this.redscorelabel.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // gamestatus
            // 
            this.gamestatus.BackColor = System.Drawing.SystemColors.Control;
            this.gamestatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gamestatus.Location = new System.Drawing.Point(119, 138);
            this.gamestatus.Name = "gamestatus";
            this.gamestatus.Size = new System.Drawing.Size(402, 31);
            this.gamestatus.TabIndex = 4;
            // 
            // hintbutton
            // 
            this.hintbutton.Location = new System.Drawing.Point(391, 79);
            this.hintbutton.Name = "hintbutton";
            this.hintbutton.Size = new System.Drawing.Size(130, 56);
            this.hintbutton.TabIndex = 5;
            this.hintbutton.Text = "HELP";
            this.hintbutton.UseVisualStyleBackColor = true;
            // 
            // resetbutton
            // 
            this.resetbutton.Location = new System.Drawing.Point(255, 79);
            this.resetbutton.Name = "resetbutton";
            this.resetbutton.Size = new System.Drawing.Size(130, 56);
            this.resetbutton.TabIndex = 6;
            this.resetbutton.Text = "RESET";
            this.resetbutton.UseVisualStyleBackColor = true;
            // 
            // nTrackbar
            // 
            this.nTrackbar.Location = new System.Drawing.Point(306, 12);
            this.nTrackbar.Maximum = 6;
            this.nTrackbar.Minimum = 2;
            this.nTrackbar.Name = "nTrackbar";
            this.nTrackbar.Size = new System.Drawing.Size(215, 56);
            this.nTrackbar.TabIndex = 7;
            this.nTrackbar.Value = 3;
            // 
            // bluescorelabel
            // 
            this.bluescorelabel.BackColor = System.Drawing.SystemColors.Control;
            this.bluescorelabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bluescorelabel.Location = new System.Drawing.Point(51, 79);
            this.bluescorelabel.Name = "bluescorelabel";
            this.bluescorelabel.Size = new System.Drawing.Size(78, 31);
            this.bluescorelabel.TabIndex = 8;
            // 
            // sizelabel
            // 
            this.sizelabel.AutoSize = true;
            this.sizelabel.Location = new System.Drawing.Point(261, 24);
            this.sizelabel.Name = "sizelabel";
            this.sizelabel.Size = new System.Drawing.Size(39, 16);
            this.sizelabel.TabIndex = 9;
            this.sizelabel.Text = "SIZE:";
            this.sizelabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.sizelabel.Click += new System.EventHandler(this.sizelabel_Click);
            // 
            // player1
            // 
            this.player1.FormattingEnabled = true;
            this.player1.Location = new System.Drawing.Point(583, 28);
            this.player1.Name = "player1";
            this.player1.Size = new System.Drawing.Size(192, 24);
            this.player1.TabIndex = 10;
            // 
            // player2
            // 
            this.player2.FormattingEnabled = true;
            this.player2.Location = new System.Drawing.Point(583, 86);
            this.player2.Name = "player2";
            this.player2.Size = new System.Drawing.Size(192, 24);
            this.player2.TabIndex = 11;
            // 
            // player1label
            // 
            this.player1label.AutoSize = true;
            this.player1label.Location = new System.Drawing.Point(551, 31);
            this.player1label.Name = "player1label";
            this.player1label.Size = new System.Drawing.Size(26, 16);
            this.player1label.TabIndex = 12;
            this.player1label.Text = "P1:";
            // 
            // player2label
            // 
            this.player2label.AutoSize = true;
            this.player2label.Location = new System.Drawing.Point(551, 92);
            this.player2label.Name = "player2label";
            this.player2label.Size = new System.Drawing.Size(26, 16);
            this.player2label.TabIndex = 13;
            this.player2label.Text = "P2:";
            // 
            // Reversi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 838);
            this.Controls.Add(this.player2label);
            this.Controls.Add(this.player1label);
            this.Controls.Add(this.player2);
            this.Controls.Add(this.player1);
            this.Controls.Add(this.sizelabel);
            this.Controls.Add(this.bluescorelabel);
            this.Controls.Add(this.nTrackbar);
            this.Controls.Add(this.resetbutton);
            this.Controls.Add(this.hintbutton);
            this.Controls.Add(this.gamestatus);
            this.Controls.Add(this.redscorelabel);
            this.Controls.Add(this.scorepanel);
            this.Controls.Add(this.boardpanel);
            this.Name = "Reversi";
            this.Text = "Reversi";
            ((System.ComponentModel.ISupportInitialize)(this.nTrackbar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel boardpanel;
        private System.Windows.Forms.Panel scorepanel;
        private System.Windows.Forms.Label redscorelabel;
        private System.Windows.Forms.Label bluescorelabel;
        private System.Windows.Forms.Label gamestatus;
        private System.Windows.Forms.Button hintbutton;
        private System.Windows.Forms.Button resetbutton;
        private System.Windows.Forms.TrackBar nTrackbar;
        private System.Windows.Forms.Label sizelabel;
        private System.Windows.Forms.ComboBox player1;
        private System.Windows.Forms.ComboBox player2;
        private System.Windows.Forms.Label player1label;
        private System.Windows.Forms.Label player2label;
    }
}

