using Untipic.UI.UntiUI.DrawPad;

namespace Untipic.UI
{
    sealed partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.mtsEdit = new Untipic.UI.UntiUI.UntiToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.tlpView = new System.Windows.Forms.TableLayoutPanel();
            this.mtsTool = new Untipic.UI.UntiUI.UntiToolStrip();
            this.tsbToolSelection = new Untipic.UI.UntiUI.UntiToolStripButton();
            this.tsbToolDirectSel = new Untipic.UI.UntiUI.UntiToolStripButton();
            this.tsbToolText = new Untipic.UI.UntiUI.UntiToolStripButton();
            this.tsbToolShape = new Untipic.UI.UntiUI.Extensions.ToolStripShapeSelectorButton();
            this.tsbToolBrush = new Untipic.UI.UntiUI.UntiToolStripButton();
            this.tsbToolEraser = new Untipic.UI.UntiUI.UntiToolStripButton();
            this.tsbToolBucket = new Untipic.UI.UntiUI.UntiToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbToolOutline = new Untipic.UI.UntiUI.Extensions.ToolStripOutlineButton();
            this.tsbToolFill = new Untipic.UI.UntiUI.Extensions.ToolStripFillButton();
            this.statusBar = new Untipic.UI.UntiUI.UntiStatusStrip();
            this.nameBar = new Untipic.UI.UntiUI.UntiNameBar();
            this.drawPad = new DrawPad();
            this.tlpMain.SuspendLayout();
            this.mtsEdit.SuspendLayout();
            this.tlpView.SuspendLayout();
            this.mtsTool.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.BackColor = System.Drawing.Color.White;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.mtsEdit, 0, 0);
            this.tlpMain.Controls.Add(this.tlpView, 0, 2);
            this.tlpMain.Controls.Add(this.statusBar, 0, 3);
            this.tlpMain.Controls.Add(this.nameBar, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(5, 65);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 4;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(897, 626);
            this.tlpMain.TabIndex = 3;
            // 
            // mtsEdit
            // 
            this.mtsEdit.BackColor = System.Drawing.Color.White;
            this.mtsEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtsEdit.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mtsEdit.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.mtsEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripSeparator1,
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripSeparator2,
            this.toolStripButton7,
            this.toolStripButton8,
            this.toolStripButton9,
            this.toolStripSeparator3,
            this.toolStripButton10,
            this.toolStripButton11});
            this.mtsEdit.Location = new System.Drawing.Point(0, 0);
            this.mtsEdit.Name = "mtsEdit";
            this.mtsEdit.Size = new System.Drawing.Size(897, 39);
            this.mtsEdit.TabIndex = 0;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::Untipic.UI.Properties.Resources.New;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::Untipic.UI.Properties.Resources.Open;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::Untipic.UI.Properties.Resources.Save;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton3.Text = "toolStripButton3";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::Untipic.UI.Properties.Resources.SaveAs;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton4.Text = "toolStripButton4";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::Untipic.UI.Properties.Resources.Undo;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton5.Text = "toolStripButton5";
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::Untipic.UI.Properties.Resources.Redo;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton6.Text = "toolStripButton6";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = global::Untipic.UI.Properties.Resources.ZoomIn;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton7.Text = "toolStripButton7";
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton8.Image = global::Untipic.UI.Properties.Resources.FitSize;
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton8.Text = "toolStripButton8";
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton9.Image = global::Untipic.UI.Properties.Resources.ZoomOut;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton9.Text = "toolStripButton9";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton10.Image = global::Untipic.UI.Properties.Resources.Font;
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton10.Text = "toolStripButton10";
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton11.Image = global::Untipic.UI.Properties.Resources.Accounts;
            this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton11.Name = "toolStripButton11";
            this.toolStripButton11.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton11.Text = "toolStripButton11";
            // 
            // tlpView
            // 
            this.tlpView.ColumnCount = 3;
            this.tlpView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpView.Controls.Add(this.mtsTool, 0, 0);
            this.tlpView.Controls.Add(this.drawPad, 1, 0);
            this.tlpView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpView.Location = new System.Drawing.Point(0, 69);
            this.tlpView.Margin = new System.Windows.Forms.Padding(0);
            this.tlpView.Name = "tlpView";
            this.tlpView.RowCount = 1;
            this.tlpView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpView.Size = new System.Drawing.Size(897, 527);
            this.tlpView.TabIndex = 1;
            // 
            // mtsTool
            // 
            this.mtsTool.AutoSize = false;
            this.mtsTool.BackColor = System.Drawing.Color.White;
            this.mtsTool.Dock = System.Windows.Forms.DockStyle.Left;
            this.mtsTool.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mtsTool.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.mtsTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbToolSelection,
            this.tsbToolDirectSel,
            this.tsbToolText,
            this.tsbToolShape,
            this.tsbToolBrush,
            this.tsbToolEraser,
            this.tsbToolBucket,
            this.toolStripSeparator4,
            this.tsbToolOutline,
            this.tsbToolFill});
            this.mtsTool.Location = new System.Drawing.Point(0, 0);
            this.mtsTool.Name = "mtsTool";
            this.mtsTool.Size = new System.Drawing.Size(60, 527);
            this.mtsTool.TabIndex = 0;
            // 
            // tsbToolSelection
            // 
            this.tsbToolSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbToolSelection.Image = global::Untipic.UI.Properties.Resources.Selection;
            this.tsbToolSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbToolSelection.IsDropDownButton = false;
            this.tsbToolSelection.Name = "tsbToolSelection";
            this.tsbToolSelection.Size = new System.Drawing.Size(58, 52);
            // 
            // tsbToolDirectSel
            // 
            this.tsbToolDirectSel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbToolDirectSel.Image = global::Untipic.UI.Properties.Resources.DirectSelection;
            this.tsbToolDirectSel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbToolDirectSel.IsDropDownButton = false;
            this.tsbToolDirectSel.Name = "tsbToolDirectSel";
            this.tsbToolDirectSel.Size = new System.Drawing.Size(58, 52);
            // 
            // tsbToolText
            // 
            this.tsbToolText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbToolText.Image = global::Untipic.UI.Properties.Resources.Text;
            this.tsbToolText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbToolText.IsDropDownButton = false;
            this.tsbToolText.Name = "tsbToolText";
            this.tsbToolText.Size = new System.Drawing.Size(58, 52);
            // 
            // tsbToolShape
            // 
            this.tsbToolShape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbToolShape.Image = global::Untipic.UI.Properties.Resources.Line;
            this.tsbToolShape.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbToolShape.IsDropDownButton = true;
            this.tsbToolShape.Name = "tsbToolShape";
            this.tsbToolShape.Size = new System.Drawing.Size(58, 52);
            // 
            // tsbToolBrush
            // 
            this.tsbToolBrush.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbToolBrush.Image = global::Untipic.UI.Properties.Resources.Brush;
            this.tsbToolBrush.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbToolBrush.IsDropDownButton = false;
            this.tsbToolBrush.Name = "tsbToolBrush";
            this.tsbToolBrush.Size = new System.Drawing.Size(58, 52);
            // 
            // tsbToolEraser
            // 
            this.tsbToolEraser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbToolEraser.Image = global::Untipic.UI.Properties.Resources.Eraser;
            this.tsbToolEraser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbToolEraser.IsDropDownButton = false;
            this.tsbToolEraser.Name = "tsbToolEraser";
            this.tsbToolEraser.Size = new System.Drawing.Size(58, 52);
            // 
            // tsbToolBucket
            // 
            this.tsbToolBucket.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbToolBucket.Image = global::Untipic.UI.Properties.Resources.Bucket;
            this.tsbToolBucket.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbToolBucket.IsDropDownButton = false;
            this.tsbToolBucket.Name = "tsbToolBucket";
            this.tsbToolBucket.Size = new System.Drawing.Size(58, 52);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(58, 6);
            // 
            // tsbToolOutline
            // 
            this.tsbToolOutline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbToolOutline.Image = ((System.Drawing.Image)(resources.GetObject("tsbToolOutline.Image")));
            this.tsbToolOutline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbToolOutline.IsDropDownButton = true;
            this.tsbToolOutline.Name = "tsbToolOutline";
            this.tsbToolOutline.OutlineColor = System.Drawing.Color.Black;
            this.tsbToolOutline.OutlineDash = System.Drawing.Drawing2D.DashStyle.Solid;
            this.tsbToolOutline.OutlineWidth = 2F;
            this.tsbToolOutline.Size = new System.Drawing.Size(58, 52);
            // 
            // tsbToolFill
            // 
            this.tsbToolFill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbToolFill.FillColor = System.Drawing.Color.Transparent;
            this.tsbToolFill.Image = ((System.Drawing.Image)(resources.GetObject("tsbToolFill.Image")));
            this.tsbToolFill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbToolFill.IsDropDownButton = true;
            this.tsbToolFill.Name = "tsbToolFill";
            this.tsbToolFill.Size = new System.Drawing.Size(58, 52);
            // 
            // statusBar
            // 
            this.statusBar.AutoSize = false;
            this.statusBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.statusBar.Location = new System.Drawing.Point(0, 596);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(897, 30);
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = 2;
            // 
            // nameBar
            // 
            this.nameBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.nameBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nameBar.Font = new System.Drawing.Font("Segoe UI Light", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameBar.Location = new System.Drawing.Point(0, 39);
            this.nameBar.Margin = new System.Windows.Forms.Padding(0);
            this.nameBar.Name = "nameBar";
            this.nameBar.ProjectName = "Untitled";
            this.nameBar.Size = new System.Drawing.Size(897, 30);
            this.nameBar.TabIndex = 3;
            // 
            // drawPad
            // 
            this.drawPad.AutoScroll = true;
            this.drawPad.AutoScrollMargin = new System.Drawing.Size(10, 10);
            //this.drawPad.AutoScrollMinSize = new System.Drawing.Size(540, 240);
            this.drawPad.BackColor = System.Drawing.SystemColors.Control;
            this.drawPad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawPad.Location = new System.Drawing.Point(63, 0);
            this.drawPad.Margin = new System.Windows.Forms.Padding(0);
            this.drawPad.Name = "drawPad";
            this.drawPad.Size = new System.Drawing.Size(831, 527);
            this.drawPad.TabIndex = 1;
            //this.drawPad.Resolution = 0F;
            //this.drawPad.ViewportHeith = 0;
            //this.drawPad.ViewportWidth = 0;
            this.drawPad.Zoom = 1F;

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(907, 696);
            this.Controls.Add(this.tlpMain);
            this.Name = "MainForm";
            this.Text = "Untipic";
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.mtsEdit.ResumeLayout(false);
            this.mtsEdit.PerformLayout();
            this.tlpView.ResumeLayout(false);
            this.tlpView.PerformLayout();
            this.mtsTool.ResumeLayout(false);
            this.mtsTool.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private UntiUI.UntiToolStrip mtsEdit;
        private System.Windows.Forms.TableLayoutPanel tlpView;
        private UntiUI.UntiStatusStrip statusBar;
        private UntiUI.UntiToolStrip mtsTool;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStripButton toolStripButton11;
        private UntiUI.UntiToolStripButton tsbToolSelection;
        private UntiUI.UntiToolStripButton tsbToolDirectSel;
        private UntiUI.UntiToolStripButton tsbToolText;
        private UntiUI.Extensions.ToolStripShapeSelectorButton tsbToolShape;
        private UntiUI.UntiToolStripButton tsbToolBrush;
        private UntiUI.UntiToolStripButton tsbToolEraser;
        private UntiUI.UntiToolStripButton tsbToolBucket;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private UntiUI.Extensions.ToolStripOutlineButton tsbToolOutline;
        private UntiUI.Extensions.ToolStripFillButton tsbToolFill;
        private UntiUI.UntiNameBar nameBar;
        private DrawPad drawPad;
    }
}
