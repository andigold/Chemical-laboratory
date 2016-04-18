namespace ChemLab
{
    partial class FmStoichiometry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmStoichiometry));
            this.tlpStoichiometry = new System.Windows.Forms.TableLayoutPanel();
            this.tbReaction = new System.Windows.Forms.TextBox();
            this.btBalance = new System.Windows.Forms.Button();
            this.rtbReaction = new System.Windows.Forms.RichTextBox();
            this.lbInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tlpStoichiometry
            // 
            this.tlpStoichiometry.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tlpStoichiometry.AutoSize = true;
            this.tlpStoichiometry.BackColor = System.Drawing.Color.White;
            this.tlpStoichiometry.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlpStoichiometry.ColumnCount = 5;
            this.tlpStoichiometry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStoichiometry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStoichiometry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStoichiometry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStoichiometry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStoichiometry.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tlpStoichiometry.Location = new System.Drawing.Point(12, 111);
            this.tlpStoichiometry.Name = "tlpStoichiometry";
            this.tlpStoichiometry.RowCount = 1;
            this.tlpStoichiometry.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tlpStoichiometry.Size = new System.Drawing.Size(956, 63);
            this.tlpStoichiometry.TabIndex = 1;
            // 
            // tbReaction
            // 
            this.tbReaction.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbReaction.Location = new System.Drawing.Point(12, 16);
            this.tbReaction.MaxLength = 128;
            this.tbReaction.Name = "tbReaction";
            this.tbReaction.Size = new System.Drawing.Size(831, 26);
            this.tbReaction.TabIndex = 2;
            // 
            // btBalance
            // 
            this.btBalance.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btBalance.Location = new System.Drawing.Point(856, 13);
            this.btBalance.Name = "btBalance";
            this.btBalance.Size = new System.Drawing.Size(112, 32);
            this.btBalance.TabIndex = 3;
            this.btBalance.Text = "Изравняване";
            this.btBalance.UseVisualStyleBackColor = true;
            this.btBalance.Click += new System.EventHandler(this.btBalance_Click);
            // 
            // rtbReaction
            // 
            this.rtbReaction.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbReaction.Location = new System.Drawing.Point(12, 55);
            this.rtbReaction.MaxLength = 128;
            this.rtbReaction.Multiline = false;
            this.rtbReaction.Name = "rtbReaction";
            this.rtbReaction.ReadOnly = true;
            this.rtbReaction.Size = new System.Drawing.Size(956, 40);
            this.rtbReaction.TabIndex = 4;
            this.rtbReaction.Text = "";
            // 
            // lbInfo
            // 
            this.lbInfo.AutoEllipsis = true;
            this.lbInfo.BackColor = System.Drawing.Color.White;
            this.lbInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbInfo.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbInfo.Location = new System.Drawing.Point(12, 185);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(956, 429);
            this.lbInfo.TabIndex = 5;
            this.lbInfo.Text = resources.GetString("lbInfo.Text");
            // 
            // FmStoichiometry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::ChemLab.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(978, 630);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.rtbReaction);
            this.Controls.Add(this.btBalance);
            this.Controls.Add(this.tbReaction);
            this.Controls.Add(this.tlpStoichiometry);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FmStoichiometry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ХимЛаб - количествени зависимости";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpStoichiometry;
        private System.Windows.Forms.TextBox tbReaction;
        private System.Windows.Forms.Button btBalance;
        private System.Windows.Forms.RichTextBox rtbReaction;
        private System.Windows.Forms.Label lbInfo;
    }
}