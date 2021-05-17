namespace CacheDataSimulator.View
{
    partial class CodeEditor
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.CodeEditRTB = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.LineNumberRTB = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1244, 758);
            this.panel1.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(35, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 758);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.CodeEditRTB);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(35, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1209, 758);
            this.panel3.TabIndex = 4;
            // 
            // CodeEditRTB
            // 
            this.CodeEditRTB.AcceptsTab = true;
            this.CodeEditRTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.CodeEditRTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CodeEditRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CodeEditRTB.Font = new System.Drawing.Font("Courier New", 10.15F);
            this.CodeEditRTB.ForeColor = System.Drawing.SystemColors.Window;
            this.CodeEditRTB.Location = new System.Drawing.Point(0, 0);
            this.CodeEditRTB.Name = "CodeEditRTB";
            this.CodeEditRTB.Size = new System.Drawing.Size(1209, 758);
            this.CodeEditRTB.TabIndex = 2;
            this.CodeEditRTB.Text = "";
            this.CodeEditRTB.VScroll += new System.EventHandler(this.CodeEditRTB_VScroll);
            this.CodeEditRTB.TextChanged += new System.EventHandler(this.CodeEditRTB_TextChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.panel2.Controls.Add(this.LineNumberRTB);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(35, 758);
            this.panel2.TabIndex = 3;
            // 
            // LineNumberRTB
            // 
            this.LineNumberRTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.LineNumberRTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LineNumberRTB.Dock = System.Windows.Forms.DockStyle.Right;
            this.LineNumberRTB.Font = new System.Drawing.Font("Courier New", 10.15F);
            this.LineNumberRTB.ForeColor = System.Drawing.SystemColors.Window;
            this.LineNumberRTB.Location = new System.Drawing.Point(7, 0);
            this.LineNumberRTB.Name = "LineNumberRTB";
            this.LineNumberRTB.ReadOnly = true;
            this.LineNumberRTB.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.LineNumberRTB.Size = new System.Drawing.Size(28, 758);
            this.LineNumberRTB.TabIndex = 0;
            this.LineNumberRTB.Text = "1";
            // 
            // CodeEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "CodeEditor";
            this.Size = new System.Drawing.Size(1244, 758);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox CodeEditRTB;
        private System.Windows.Forms.RichTextBox LineNumberRTB;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
    }
}
