namespace ndBIM
{
    partial class ndBIMInterface
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
            this.budgetLbl = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.hor2 = new System.Windows.Forms.Label();
            this.flowPanel_02 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowPanel_01 = new System.Windows.Forms.FlowLayoutPanel();
            this.BIMLbl = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.populateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // budgetLbl
            // 
            this.budgetLbl.AutoSize = true;
            this.budgetLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.budgetLbl.Location = new System.Drawing.Point(3, 3);
            this.budgetLbl.Name = "budgetLbl";
            this.budgetLbl.Size = new System.Drawing.Size(47, 13);
            this.budgetLbl.TabIndex = 4;
            this.budgetLbl.Text = "Budget";
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainPanel.Controls.Add(this.numericUpDown1);
            this.mainPanel.Controls.Add(this.label1);
            this.mainPanel.Controls.Add(this.hor2);
            this.mainPanel.Controls.Add(this.flowPanel_02);
            this.mainPanel.Controls.Add(this.flowPanel_01);
            this.mainPanel.Controls.Add(this.BIMLbl);
            this.mainPanel.Controls.Add(this.budgetLbl);
            this.mainPanel.Location = new System.Drawing.Point(12, 12);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(273, 410);
            this.mainPanel.TabIndex = 6;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.Location = new System.Drawing.Point(203, 30);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(37, 20);
            this.numericUpDown1.TabIndex = 10;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(4, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 2);
            this.label1.TabIndex = 8;
            // 
            // hor2
            // 
            this.hor2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hor2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.hor2.Location = new System.Drawing.Point(3, 186);
            this.hor2.Name = "hor2";
            this.hor2.Size = new System.Drawing.Size(237, 2);
            this.hor2.TabIndex = 2;
            // 
            // flowPanel_02
            // 
            this.flowPanel_02.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowPanel_02.AutoScroll = true;
            this.flowPanel_02.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPanel_02.Location = new System.Drawing.Point(3, 190);
            this.flowPanel_02.Name = "flowPanel_02";
            this.flowPanel_02.Size = new System.Drawing.Size(264, 214);
            this.flowPanel_02.TabIndex = 7;
            this.flowPanel_02.WrapContents = false;
            // 
            // flowPanel_01
            // 
            this.flowPanel_01.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowPanel_01.AutoScroll = true;
            this.flowPanel_01.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPanel_01.Location = new System.Drawing.Point(3, 54);
            this.flowPanel_01.Name = "flowPanel_01";
            this.flowPanel_01.Size = new System.Drawing.Size(264, 106);
            this.flowPanel_01.TabIndex = 6;
            this.flowPanel_01.WrapContents = false;
            // 
            // BIMLbl
            // 
            this.BIMLbl.AutoSize = true;
            this.BIMLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BIMLbl.Location = new System.Drawing.Point(3, 169);
            this.BIMLbl.Name = "BIMLbl";
            this.BIMLbl.Size = new System.Drawing.Size(94, 13);
            this.BIMLbl.TabIndex = 5;
            this.BIMLbl.Text = "BIM 4D and 5D";
            // 
            // populateButton
            // 
            this.populateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.populateButton.Location = new System.Drawing.Point(12, 428);
            this.populateButton.Name = "populateButton";
            this.populateButton.Size = new System.Drawing.Size(75, 23);
            this.populateButton.TabIndex = 4;
            this.populateButton.Text = "Populate";
            this.populateButton.UseVisualStyleBackColor = true;
            this.populateButton.Click += new System.EventHandler(this.populateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.Location = new System.Drawing.Point(204, 428);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // ndBIMInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 461);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.populateButton);
            this.MaximumSize = new System.Drawing.Size(450, 500);
            this.MinimumSize = new System.Drawing.Size(240, 500);
            this.Name = "ndBIMInterface";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label budgetLbl;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.FlowLayoutPanel flowPanel_02;
        private System.Windows.Forms.FlowLayoutPanel flowPanel_01;
        private System.Windows.Forms.Label BIMLbl;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label hor2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button populateButton;
        private System.Windows.Forms.Button cancelButton;
    }
}

