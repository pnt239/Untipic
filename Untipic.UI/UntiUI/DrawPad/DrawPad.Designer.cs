namespace Untipic.UI.UntiUI.DrawPad
{
    partial class DrawPad
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.gdiArea = new GdiArea();
            this.tlpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.BackColor = System.Drawing.Color.Transparent;
            this.tlpMain.ColumnCount = 3;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Controls.Add(this.gdiArea, 1, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Size = new System.Drawing.Size(69, 65);
            this.tlpMain.TabIndex = 0;
            // 
            // gdiArea
            // 
            this.gdiArea.BackColor = System.Drawing.Color.White;
            this.gdiArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdiArea.Location = new System.Drawing.Point(9, 7);
            this.gdiArea.Name = "gdiArea";
            this.gdiArea.Size = new System.Drawing.Size(50, 50);
            this.gdiArea.TabIndex = 0;
            this.gdiArea.Text = "gdiArea";
            this.gdiArea.Click += new System.EventHandler(this.gdiArea_Click);
            this.gdiArea.Paint += new System.Windows.Forms.PaintEventHandler(this.gdiArea_Paint);
            this.gdiArea.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gdiArea_KeyDown);
            this.gdiArea.KeyUp += new System.Windows.Forms.KeyEventHandler(this.gdiArea_KeyUp);
            this.gdiArea.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gdiArea_MouseDown);
            this.gdiArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gdiArea_MouseMove);
            this.gdiArea.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gdiArea_MouseUp);
            // 
            // DrawPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tlpMain);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DrawPad";
            this.Size = new System.Drawing.Size(69, 65);
            this.Load += new System.EventHandler(this.DrawPad_Load);
            this.tlpMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private GdiArea gdiArea;

    }
}
