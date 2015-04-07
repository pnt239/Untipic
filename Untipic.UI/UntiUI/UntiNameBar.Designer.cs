namespace Untipic.Controls
{
    partial class MetroNameBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtName = new System.Windows.Forms.TextBox();
            this.lbName = new System.Windows.Forms.Label();
            this.btnYes = new Untipic.MetroUI.MetroButton();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtName.Location = new System.Drawing.Point(10, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(260, 18);
            this.txtName.TabIndex = 0;
            this.txtName.Visible = false;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.lbName.ForeColor = System.Drawing.Color.White;
            this.lbName.Location = new System.Drawing.Point(7, 16);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(51, 17);
            this.lbName.TabIndex = 1;
            this.lbName.Text = "lbName";
            this.lbName.Click += new System.EventHandler(this.lbName_Click);
            this.lbName.MouseEnter += new System.EventHandler(this.lbName_MouseEnter);
            this.lbName.MouseLeave += new System.EventHandler(this.lbName_MouseLeave);
            // 
            // btnYes
            // 
            this.btnYes.Font = new System.Drawing.Font("Webdings", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnYes.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnYes.Location = new System.Drawing.Point(81, 12);
            this.btnYes.Name = "btnYes";
            this.btnYes.PressColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.btnYes.Size = new System.Drawing.Size(21, 18);
            this.btnYes.TabIndex = 2;
            this.btnYes.Text = "a";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Visible = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // MetroNameBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.txtName);
            this.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MetroNameBar";
            this.Size = new System.Drawing.Size(159, 33);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lbName;
        private MetroUI.MetroButton btnYes;

    }
}
