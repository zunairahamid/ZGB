namespace ZGB
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
            this.lblProduct = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblCompany = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.tbxID = new System.Windows.Forms.TextBox();
            this.tbxName = new System.Windows.Forms.TextBox();
            this.tbxCategory = new System.Windows.Forms.TextBox();
            this.tbxCompany = new System.Windows.Forms.TextBox();
            this.tbxPrice = new System.Windows.Forms.TextBox();
            this.tbxQuantity = new System.Windows.Forms.TextBox();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.grpViewDatabase = new System.Windows.Forms.GroupBox();
            this.rdbViewAllDB = new System.Windows.Forms.RadioButton();
            this.rdbViewProductDB = new System.Windows.Forms.RadioButton();
            this.rdbViewMarketDB = new System.Windows.Forms.RadioButton();
            this.rdbViewDefaultDB = new System.Windows.Forms.RadioButton();
            this.grpOperationDatabase = new System.Windows.Forms.GroupBox();
            this.lblCurrentOperationDB = new System.Windows.Forms.Label();
            this.rdbOperationProductDB = new System.Windows.Forms.RadioButton();
            this.rdbOperationMarketDB = new System.Windows.Forms.RadioButton();
            this.rdbOperationDefaultDB = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.grpViewDatabase.SuspendLayout();
            this.grpOperationDatabase.SuspendLayout();
            this.SuspendLayout();

            // lblProduct
            this.lblProduct.AutoSize = true;
            this.lblProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblProduct.Location = new System.Drawing.Point(350, 20);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(270, 24);
            this.lblProduct.TabIndex = 0;
            this.lblProduct.Text = "Neo4j Product Manager";

            // lblID
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(50, 70);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(58, 13);
            this.lblID.TabIndex = 1;
            this.lblID.Text = "Product ID";

            // lblProductName
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(50, 110);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(75, 13);
            this.lblProductName.TabIndex = 2;
            this.lblProductName.Text = "Product Name";

            // lblCategory
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(50, 150);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(89, 13);
            this.lblCategory.TabIndex = 3;
            this.lblCategory.Text = "Product Category";

            // lblCompany
            this.lblCompany.AutoSize = true;
            this.lblCompany.Location = new System.Drawing.Point(50, 190);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(91, 13);
            this.lblCompany.TabIndex = 4;
            this.lblCompany.Text = "Product Company";

            // lblPrice
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(50, 230);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(71, 13);
            this.lblPrice.TabIndex = 5;
            this.lblPrice.Text = "Product Price";

            // lblQuantity
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(50, 270);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(86, 13);
            this.lblQuantity.TabIndex = 6;
            this.lblQuantity.Text = "Product Quantity";

            // tbxID
            this.tbxID.Location = new System.Drawing.Point(200, 70);
            this.tbxID.Name = "tbxID";
            this.tbxID.Size = new System.Drawing.Size(200, 20);
            this.tbxID.TabIndex = 7;

            // tbxName
            this.tbxName.Location = new System.Drawing.Point(200, 110);
            this.tbxName.Name = "tbxName";
            this.tbxName.Size = new System.Drawing.Size(200, 20);
            this.tbxName.TabIndex = 8;

            // tbxCategory
            this.tbxCategory.Location = new System.Drawing.Point(200, 150);
            this.tbxCategory.Name = "tbxCategory";
            this.tbxCategory.Size = new System.Drawing.Size(200, 20);
            this.tbxCategory.TabIndex = 9;

            // tbxCompany
            this.tbxCompany.Location = new System.Drawing.Point(200, 190);
            this.tbxCompany.Name = "tbxCompany";
            this.tbxCompany.Size = new System.Drawing.Size(200, 20);
            this.tbxCompany.TabIndex = 10;

            // tbxPrice
            this.tbxPrice.Location = new System.Drawing.Point(200, 230);
            this.tbxPrice.Name = "tbxPrice";
            this.tbxPrice.Size = new System.Drawing.Size(200, 20);
            this.tbxPrice.TabIndex = 11;

            // tbxQuantity
            this.tbxQuantity.Location = new System.Drawing.Point(200, 270);
            this.tbxQuantity.Name = "tbxQuantity";
            this.tbxQuantity.Size = new System.Drawing.Size(200, 20);
            this.tbxQuantity.TabIndex = 12;

            // btnInsert
            this.btnInsert.Location = new System.Drawing.Point(450, 70);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(120, 35);
            this.btnInsert.TabIndex = 13;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;

            // btnUpdate
            this.btnUpdate.Location = new System.Drawing.Point(450, 120);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(120, 35);
            this.btnUpdate.TabIndex = 14;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(450, 170);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 35);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;

            // dataGridView1
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(50, 350);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(900, 250);
            this.dataGridView1.TabIndex = 16;

            // grpViewDatabase
            this.grpViewDatabase.Controls.Add(this.rdbViewAllDB);
            this.grpViewDatabase.Controls.Add(this.rdbViewProductDB);
            this.grpViewDatabase.Controls.Add(this.rdbViewMarketDB);
            this.grpViewDatabase.Controls.Add(this.rdbViewDefaultDB);
            this.grpViewDatabase.Location = new System.Drawing.Point(600, 70);
            this.grpViewDatabase.Name = "grpViewDatabase";
            this.grpViewDatabase.Size = new System.Drawing.Size(350, 100);
            this.grpViewDatabase.TabIndex = 17;
            this.grpViewDatabase.TabStop = false;
            this.grpViewDatabase.Text = "View Database";

            // rdbViewAllDB
            this.rdbViewAllDB.AutoSize = true;
            this.rdbViewAllDB.Location = new System.Drawing.Point(250, 40);
            this.rdbViewAllDB.Name = "rdbViewAllDB";
            this.rdbViewAllDB.Size = new System.Drawing.Size(70, 17);
            this.rdbViewAllDB.TabIndex = 3;
            this.rdbViewAllDB.Text = "All DBs";
            this.rdbViewAllDB.UseVisualStyleBackColor = true;

            // rdbViewProductDB
            this.rdbViewProductDB.AutoSize = true;
            this.rdbViewProductDB.Location = new System.Drawing.Point(150, 40);
            this.rdbViewProductDB.Name = "rdbViewProductDB";
            this.rdbViewProductDB.Size = new System.Drawing.Size(80, 17);
            this.rdbViewProductDB.TabIndex = 2;
            this.rdbViewProductDB.Text = "Product DB";
            this.rdbViewProductDB.UseVisualStyleBackColor = true;

            // rdbViewMarketDB
            this.rdbViewMarketDB.AutoSize = true;
            this.rdbViewMarketDB.Location = new System.Drawing.Point(50, 40);
            this.rdbViewMarketDB.Name = "rdbViewMarketDB";
            this.rdbViewMarketDB.Size = new System.Drawing.Size(75, 17);
            this.rdbViewMarketDB.TabIndex = 1;
            this.rdbViewMarketDB.Text = "Market DB";
            this.rdbViewMarketDB.UseVisualStyleBackColor = true;

            // rdbViewDefaultDB
            this.rdbViewDefaultDB.AutoSize = true;
            this.rdbViewDefaultDB.Checked = true;
            this.rdbViewDefaultDB.Location = new System.Drawing.Point(50, 20);
            this.rdbViewDefaultDB.Name = "rdbViewDefaultDB";
            this.rdbViewDefaultDB.Size = new System.Drawing.Size(75, 17);
            this.rdbViewDefaultDB.TabIndex = 0;
            this.rdbViewDefaultDB.TabStop = true;
            this.rdbViewDefaultDB.Text = "Default DB";
            this.rdbViewDefaultDB.UseVisualStyleBackColor = true;

            // grpOperationDatabase
            this.grpOperationDatabase.Controls.Add(this.lblCurrentOperationDB);
            this.grpOperationDatabase.Controls.Add(this.rdbOperationProductDB);
            this.grpOperationDatabase.Controls.Add(this.rdbOperationMarketDB);
            this.grpOperationDatabase.Controls.Add(this.rdbOperationDefaultDB);
            this.grpOperationDatabase.Location = new System.Drawing.Point(600, 180);
            this.grpOperationDatabase.Name = "grpOperationDatabase";
            this.grpOperationDatabase.Size = new System.Drawing.Size(350, 100);
            this.grpOperationDatabase.TabIndex = 18;
            this.grpOperationDatabase.TabStop = false;
            this.grpOperationDatabase.Text = "Operation Database";

            // lblCurrentOperationDB
            this.lblCurrentOperationDB.AutoSize = true;
            this.lblCurrentOperationDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblCurrentOperationDB.Location = new System.Drawing.Point(20, 70);
            this.lblCurrentOperationDB.Name = "lblCurrentOperationDB";
            this.lblCurrentOperationDB.Size = new System.Drawing.Size(150, 13);
            this.lblCurrentOperationDB.TabIndex = 3;
            this.lblCurrentOperationDB.Text = "Current Operation DB: neo4j";

            // rdbOperationProductDB
            this.rdbOperationProductDB.AutoSize = true;
            this.rdbOperationProductDB.Location = new System.Drawing.Point(250, 40);
            this.rdbOperationProductDB.Name = "rdbOperationProductDB";
            this.rdbOperationProductDB.Size = new System.Drawing.Size(80, 17);
            this.rdbOperationProductDB.TabIndex = 2;
            this.rdbOperationProductDB.Text = "Product DB";
            this.rdbOperationProductDB.UseVisualStyleBackColor = true;

            // rdbOperationMarketDB
            this.rdbOperationMarketDB.AutoSize = true;
            this.rdbOperationMarketDB.Location = new System.Drawing.Point(150, 40);
            this.rdbOperationMarketDB.Name = "rdbOperationMarketDB";
            this.rdbOperationMarketDB.Size = new System.Drawing.Size(75, 17);
            this.rdbOperationMarketDB.TabIndex = 1;
            this.rdbOperationMarketDB.Text = "Market DB";
            this.rdbOperationMarketDB.UseVisualStyleBackColor = true;

            // rdbOperationDefaultDB
            this.rdbOperationDefaultDB.AutoSize = true;
            this.rdbOperationDefaultDB.Checked = true;
            this.rdbOperationDefaultDB.Location = new System.Drawing.Point(50, 40);
            this.rdbOperationDefaultDB.Name = "rdbOperationDefaultDB";
            this.rdbOperationDefaultDB.Size = new System.Drawing.Size(75, 17);
            this.rdbOperationDefaultDB.TabIndex = 0;
            this.rdbOperationDefaultDB.TabStop = true;
            this.rdbOperationDefaultDB.Text = "Default DB";
            this.rdbOperationDefaultDB.UseVisualStyleBackColor = true;

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.Controls.Add(this.grpOperationDatabase);
            this.Controls.Add(this.grpViewDatabase);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.tbxQuantity);
            this.Controls.Add(this.tbxPrice);
            this.Controls.Add(this.tbxCompany);
            this.Controls.Add(this.tbxCategory);
            this.Controls.Add(this.tbxName);
            this.Controls.Add(this.tbxID);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblCompany);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.lblProduct);
            this.Name = "Form1";
            this.Text = "Neo4j Product Management System";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.grpViewDatabase.ResumeLayout(false);
            this.grpViewDatabase.PerformLayout();
            this.grpOperationDatabase.ResumeLayout(false);
            this.grpOperationDatabase.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.TextBox tbxID;
        private System.Windows.Forms.TextBox tbxName;
        private System.Windows.Forms.TextBox tbxCategory;
        private System.Windows.Forms.TextBox tbxCompany;
        private System.Windows.Forms.TextBox tbxPrice;
        private System.Windows.Forms.TextBox tbxQuantity;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox grpViewDatabase;
        private System.Windows.Forms.RadioButton rdbViewAllDB;
        private System.Windows.Forms.RadioButton rdbViewProductDB;
        private System.Windows.Forms.RadioButton rdbViewMarketDB;
        private System.Windows.Forms.RadioButton rdbViewDefaultDB;
        private System.Windows.Forms.GroupBox grpOperationDatabase;
        private System.Windows.Forms.Label lblCurrentOperationDB;
        private System.Windows.Forms.RadioButton rdbOperationProductDB;
        private System.Windows.Forms.RadioButton rdbOperationMarketDB;
        private System.Windows.Forms.RadioButton rdbOperationDefaultDB;
    }
}