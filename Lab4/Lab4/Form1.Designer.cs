
namespace Lab4
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
            this.myButton = new System.Windows.Forms.Button();
            this.myTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // myButton
            // 
            this.myButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.myButton.Location = new System.Drawing.Point(12, 12);
            this.myButton.Name = "myButton";
            this.myButton.Size = new System.Drawing.Size(251, 52);
            this.myButton.TabIndex = 0;
            this.myButton.Text = "Setup Integration";
            this.myButton.UseVisualStyleBackColor = true;
            this.myButton.Click += new System.EventHandler(this.myButton_Click);
            // 
            // myTextBox
            // 
            this.myTextBox.Location = new System.Drawing.Point(12, 70);
            this.myTextBox.Multiline = true;
            this.myTextBox.Name = "myTextBox";
            this.myTextBox.ReadOnly = true;
            this.myTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.myTextBox.Size = new System.Drawing.Size(251, 310);
            this.myTextBox.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 392);
            this.Controls.Add(this.myTextBox);
            this.Controls.Add(this.myButton);
            this.Name = "Form1";
            this.Text = "MediaIntegrator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button myButton;
        private System.Windows.Forms.TextBox myTextBox;
    }
}

