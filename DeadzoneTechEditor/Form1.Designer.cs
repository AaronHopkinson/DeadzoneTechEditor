namespace DeadzoneTechEditor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            btnSelectSave = new Button();
            lblStatus = new Label();
            btnPatch = new Button();
            txtNewTech = new TextBox();
            SuspendLayout();
            // 
            // btnSelectSave
            // 
            btnSelectSave.BackColor = SystemColors.Control;
            btnSelectSave.Location = new Point(410, 184);
            btnSelectSave.Name = "btnSelectSave";
            btnSelectSave.Size = new Size(112, 34);
            btnSelectSave.TabIndex = 0;
            btnSelectSave.Text = "Select Save";
            btnSelectSave.UseVisualStyleBackColor = false;
            btnSelectSave.Click += btnSelectSave_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.BackColor = Color.RoyalBlue;
            lblStatus.ForeColor = SystemColors.ButtonHighlight;
            lblStatus.Location = new Point(62, 189);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(145, 25);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "No save selected";
            // 
            // btnPatch
            // 
            btnPatch.BackColor = SystemColors.Control;
            btnPatch.Location = new Point(386, 516);
            btnPatch.Name = "btnPatch";
            btnPatch.Size = new Size(169, 34);
            btnPatch.TabIndex = 2;
            btnPatch.Text = "Change Your Tech!";
            btnPatch.UseVisualStyleBackColor = false;
            btnPatch.Click += btnPatch_Click;
            // 
            // txtNewTech
            // 
            txtNewTech.BackColor = SystemColors.Control;
            txtNewTech.Location = new Point(33, 518);
            txtNewTech.Name = "txtNewTech";
            txtNewTech.PlaceholderText = "Enter desired amount here";
            txtNewTech.Size = new Size(218, 31);
            txtNewTech.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(587, 572);
            Controls.Add(txtNewTech);
            Controls.Add(btnPatch);
            Controls.Add(lblStatus);
            Controls.Add(btnSelectSave);
            Name = "Form1";
            Text = "Deadzone: Rogue Tech Editor";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSelectSave;
        private Label lblStatus;
        private Button btnPatch;
        private TextBox txtNewTech;
    }
}
