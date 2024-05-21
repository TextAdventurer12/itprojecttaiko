namespace taikoclone
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
            this.components = new System.ComponentModel.Container();
            this.pictureBoxClickCircle = new System.Windows.Forms.PictureBox();
            this.GameUpdate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClickCircle)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxClickCircle
            // 
            this.pictureBoxClickCircle.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxClickCircle.Name = "pictureBoxClickCircle";
            this.pictureBoxClickCircle.Size = new System.Drawing.Size(100, 100);
            this.pictureBoxClickCircle.TabIndex = 0;
            this.pictureBoxClickCircle.TabStop = false;
            // 
            // GameUpdate
            // 
            this.GameUpdate.Enabled = true;
            this.GameUpdate.Interval = 10;
            this.GameUpdate.Tick += new System.EventHandler(this.GameUpdate_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBoxClickCircle);
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClickCircle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxClickCircle;
        private System.Windows.Forms.Timer GameUpdate;
    }
}

