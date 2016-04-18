namespace ChemLab
{
    partial class FmAddReaction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FmAddReaction));
            this.lbFirstReactant = new System.Windows.Forms.Label();
            this.tbFirstReactant = new System.Windows.Forms.TextBox();
            this.tbSecondReactant = new System.Windows.Forms.TextBox();
            this.lbSecondReactant = new System.Windows.Forms.Label();
            this.tbProducts = new System.Windows.Forms.TextBox();
            this.lbProducs = new System.Windows.Forms.Label();
            this.lbPlus = new System.Windows.Forms.Label();
            this.lbArrow = new System.Windows.Forms.Label();
            this.btAdd = new System.Windows.Forms.Button();
            this.lbInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbFirstReactant
            // 
            this.lbFirstReactant.AutoSize = true;
            this.lbFirstReactant.BackColor = System.Drawing.Color.Transparent;
            this.lbFirstReactant.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbFirstReactant.Location = new System.Drawing.Point(26, 20);
            this.lbFirstReactant.Name = "lbFirstReactant";
            this.lbFirstReactant.Size = new System.Drawing.Size(93, 16);
            this.lbFirstReactant.TabIndex = 0;
            this.lbFirstReactant.Text = "първи реагент";
            // 
            // tbFirstReactant
            // 
            this.tbFirstReactant.BackColor = System.Drawing.Color.White;
            this.tbFirstReactant.Location = new System.Drawing.Point(12, 40);
            this.tbFirstReactant.MaxLength = 16;
            this.tbFirstReactant.Name = "tbFirstReactant";
            this.tbFirstReactant.Size = new System.Drawing.Size(152, 25);
            this.tbFirstReactant.TabIndex = 1;
            // 
            // tbSecondReactant
            // 
            this.tbSecondReactant.BackColor = System.Drawing.Color.White;
            this.tbSecondReactant.Location = new System.Drawing.Point(191, 40);
            this.tbSecondReactant.MaxLength = 16;
            this.tbSecondReactant.Name = "tbSecondReactant";
            this.tbSecondReactant.Size = new System.Drawing.Size(152, 25);
            this.tbSecondReactant.TabIndex = 3;
            // 
            // lbSecondReactant
            // 
            this.lbSecondReactant.AutoSize = true;
            this.lbSecondReactant.BackColor = System.Drawing.Color.Transparent;
            this.lbSecondReactant.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbSecondReactant.Location = new System.Drawing.Point(213, 20);
            this.lbSecondReactant.Name = "lbSecondReactant";
            this.lbSecondReactant.Size = new System.Drawing.Size(90, 16);
            this.lbSecondReactant.TabIndex = 2;
            this.lbSecondReactant.Text = "втори реагент";
            // 
            // tbProducts
            // 
            this.tbProducts.BackColor = System.Drawing.Color.White;
            this.tbProducts.Location = new System.Drawing.Point(385, 40);
            this.tbProducts.MaxLength = 64;
            this.tbProducts.Name = "tbProducts";
            this.tbProducts.Size = new System.Drawing.Size(377, 25);
            this.tbProducts.TabIndex = 5;
            // 
            // lbProducs
            // 
            this.lbProducs.AutoSize = true;
            this.lbProducs.BackColor = System.Drawing.Color.Transparent;
            this.lbProducs.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbProducs.Location = new System.Drawing.Point(550, 20);
            this.lbProducs.Name = "lbProducs";
            this.lbProducs.Size = new System.Drawing.Size(62, 16);
            this.lbProducs.TabIndex = 4;
            this.lbProducs.Text = "продукти";
            // 
            // lbPlus
            // 
            this.lbPlus.AutoSize = true;
            this.lbPlus.BackColor = System.Drawing.Color.Transparent;
            this.lbPlus.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbPlus.Location = new System.Drawing.Point(167, 41);
            this.lbPlus.Name = "lbPlus";
            this.lbPlus.Size = new System.Drawing.Size(21, 22);
            this.lbPlus.TabIndex = 6;
            this.lbPlus.Text = "+";
            // 
            // lbArrow
            // 
            this.lbArrow.AutoSize = true;
            this.lbArrow.BackColor = System.Drawing.Color.Transparent;
            this.lbArrow.Font = new System.Drawing.Font("Wingdings 3", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.lbArrow.Location = new System.Drawing.Point(346, 39);
            this.lbArrow.Name = "lbArrow";
            this.lbArrow.Size = new System.Drawing.Size(37, 30);
            this.lbArrow.TabIndex = 7;
            this.lbArrow.Text = "";
            // 
            // btAdd
            // 
            this.btAdd.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btAdd.Location = new System.Drawing.Point(768, 37);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(104, 30);
            this.btAdd.TabIndex = 8;
            this.btAdd.Text = "Добави";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // lbInfo
            // 
            this.lbInfo.AutoEllipsis = true;
            this.lbInfo.BackColor = System.Drawing.Color.White;
            this.lbInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbInfo.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbInfo.Location = new System.Drawing.Point(12, 95);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(860, 202);
            this.lbInfo.TabIndex = 9;
            this.lbInfo.Text = resources.GetString("lbInfo.Text");
            // 
            // FmAddReaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::ChemLab.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(884, 306);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.btAdd);
            this.Controls.Add(this.lbArrow);
            this.Controls.Add(this.lbPlus);
            this.Controls.Add(this.tbProducts);
            this.Controls.Add(this.lbProducs);
            this.Controls.Add(this.tbSecondReactant);
            this.Controls.Add(this.lbSecondReactant);
            this.Controls.Add(this.tbFirstReactant);
            this.Controls.Add(this.lbFirstReactant);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FmAddReaction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ХимЛаб - добавяне на реакция";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbFirstReactant;
        private System.Windows.Forms.TextBox tbFirstReactant;
        private System.Windows.Forms.TextBox tbSecondReactant;
        private System.Windows.Forms.Label lbSecondReactant;
        private System.Windows.Forms.TextBox tbProducts;
        private System.Windows.Forms.Label lbProducs;
        private System.Windows.Forms.Label lbPlus;
        private System.Windows.Forms.Label lbArrow;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Label lbInfo;
    }
}