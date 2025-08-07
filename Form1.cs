using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZGB
{
    public partial class Form1 : Form
    {
        private readonly IDriver _driver;
        private DataTable _productsTable;
        private string _viewDatabase = "neo4j"; // For viewing data
        private string _operationDatabase = "neo4j"; // For CRUD operations

        public Form1()
        {
            InitializeComponent();

            // Initialize Neo4j driver (update with your credentials)
            _driver = GraphDatabase.Driver(
                "bolt://localhost:7687",
                AuthTokens.Basic("neo4j", "yourpassword"));

            InitializeDataTable();
            InitializeDatabaseControls();
            this.Load += async (sender, e) => await LoadProductsAsync();
        }

        private void InitializeDataTable()
        {
            _productsTable = new DataTable();
            _productsTable.Columns.Add("ProductID", typeof(int));
            _productsTable.Columns.Add("ProductName", typeof(string));
            _productsTable.Columns.Add("ProductCategory", typeof(string));
            _productsTable.Columns.Add("ProductCompany", typeof(string));
            _productsTable.Columns.Add("ProductPrice", typeof(decimal));
            _productsTable.Columns.Add("ProductQuantity", typeof(int));
            _productsTable.Columns.Add("Database", typeof(string));

            dataGridView1.DataSource = _productsTable;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void InitializeDatabaseControls()
        {
            // Database selection for viewing
            rdbViewMarketDB.Tag = "marketdb";
            rdbViewProductDB.Tag = "productdb";
            rdbViewDefaultDB.Tag = "neo4j";
            rdbViewDefaultDB.Checked = true;

            // Database selection for operations
            rdbOperationMarketDB.Tag = "marketdb";
            rdbOperationProductDB.Tag = "productdb";
            rdbOperationDefaultDB.Tag = "neo4j";
            rdbOperationDefaultDB.Checked = true;

            // Wire up event handlers
            rdbViewMarketDB.CheckedChanged += ViewDatabase_CheckedChanged;
            rdbViewProductDB.CheckedChanged += ViewDatabase_CheckedChanged;
            rdbViewDefaultDB.CheckedChanged += ViewDatabase_CheckedChanged;

            rdbOperationMarketDB.CheckedChanged += OperationDatabase_CheckedChanged;
            rdbOperationProductDB.CheckedChanged += OperationDatabase_CheckedChanged;
            rdbOperationDefaultDB.CheckedChanged += OperationDatabase_CheckedChanged;

            // Initialize buttons
            btnInsert.Click += btnInsert_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;

            // Initialize data grid view
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private async void ViewDatabase_CheckedChanged(object sender, EventArgs e)
        {
            var radio = sender as RadioButton;
            if (radio != null && radio.Checked)
            {
                _viewDatabase = radio.Tag.ToString();
                await LoadProductsAsync();
            }
        }

        private void OperationDatabase_CheckedChanged(object sender, EventArgs e)
        {
            var radio = sender as RadioButton;
            if (radio != null && radio.Checked)
            {
                _operationDatabase = radio.Tag.ToString();
                lblCurrentOperationDB.Text = $"Current Operation DB: {_operationDatabase}";
            }
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                _productsTable.Rows.Clear();

                // Load from all databases or filter by viewDatabase
                if (_viewDatabase == "all")
                {
                    await LoadFromDatabase("marketdb");
                    await LoadFromDatabase("productdb");
                    await LoadFromDatabase("neo4j");
                }
                else
                {
                    await LoadFromDatabase(_viewDatabase);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                dataGridView1.Refresh();
            }
        }

        private async Task LoadFromDatabase(string databaseName)
        {
            try
            {
                using (var session = _driver.AsyncSession(builder => builder.WithDatabase(databaseName)))
                {
                    var result = await session.ExecuteReadAsync(async tx =>
                    {
                        var cursor = await tx.RunAsync(
                            "MATCH (p:Product) " +
                            "RETURN p.id AS ProductID, p.name AS ProductName, " +
                            "p.category AS ProductCategory, p.company AS ProductCompany, " +
                            "p.price AS ProductPrice, p.quantity AS ProductQuantity " +
                            "ORDER BY p.id");

                        return await cursor.ToListAsync();
                    });

                    foreach (var record in result)
                    {
                        _productsTable.Rows.Add(
                            record["ProductID"].As<int>(),
                            record["ProductName"].As<string>(),
                            record["ProductCategory"].As<string>(),
                            record["ProductCompany"].As<string>(),
                            record["ProductPrice"].As<decimal>(),
                            record["ProductQuantity"].As<int>(),
                            databaseName);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("does not exist"))
                {
                    await CreateDatabase(databaseName);
                    await LoadFromDatabase(databaseName); // Try again after creation
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateDatabase(string databaseName)
        {
            try
            {
                // Need to use system database to create new databases
                using (var sysSession = _driver.AsyncSession(builder => builder.WithDatabase("system")))
                {
                    await sysSession.ExecuteWriteAsync(async tx =>
                    {
                        await tx.RunAsync($"CREATE DATABASE {databaseName}");
                    });
                }

                // Create initial schema in the new database
                using (var session = _driver.AsyncSession(builder => builder.WithDatabase(databaseName)))
                {
                    await session.ExecuteWriteAsync(async tx =>
                    {
                        await tx.RunAsync(
                            "CREATE CONSTRAINT product_id_unique IF NOT EXISTS " +
                            "FOR (p:Product) REQUIRE p.id IS UNIQUE");
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create database {databaseName}: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnInsert_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                var parameters = new Dictionary<string, object>
                {
                    { "id", int.Parse(tbxID.Text) },
                    { "name", tbxName.Text },
                    { "category", tbxCategory.Text },
                    { "company", tbxCompany.Text },
                    { "price", decimal.Parse(tbxPrice.Text) },
                    { "quantity", int.Parse(tbxQuantity.Text) }
                };

                using (var session = _driver.AsyncSession(builder => builder.WithDatabase(_operationDatabase)))
                {
                    var result = await session.ExecuteWriteAsync(async tx =>
                    {
                        var cursor = await tx.RunAsync(
                            "CREATE (p:Product {id: $id, name: $name, category: $category, " +
                            "company: $company, price: $price, quantity: $quantity}) " +
                            "RETURN p.id AS id",
                            parameters);

                        return await cursor.SingleAsync();
                    });

                    if (result["id"].As<int>() > 0)
                    {
                        MessageBox.Show($"Product inserted successfully into {_operationDatabase}!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadProductsAsync();
                        ClearForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting product: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                var parameters = new Dictionary<string, object>
                {
                    { "id", int.Parse(tbxID.Text) },
                    { "name", tbxName.Text },
                    { "category", tbxCategory.Text },
                    { "company", tbxCompany.Text },
                    { "price", decimal.Parse(tbxPrice.Text) },
                    { "quantity", int.Parse(tbxQuantity.Text) }
                };

                using (var session = _driver.AsyncSession(builder => builder.WithDatabase(_operationDatabase)))
                {
                    var result = await session.ExecuteWriteAsync(async tx =>
                    {
                        var cursor = await tx.RunAsync(
                            "MATCH (p:Product {id: $id}) " +
                            "SET p.name = $name, p.category = $category, " +
                            "p.company = $company, p.price = $price, p.quantity = $quantity " +
                            "RETURN count(p) AS updatedCount",
                            parameters);

                        return await cursor.SingleAsync();
                    });

                    if (result["updatedCount"].As<int>() == 0)
                    {
                        MessageBox.Show("No product found with the specified ID.",
                            "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MessageBox.Show($"Product updated successfully in {_operationDatabase}!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadProductsAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxID.Text) || !int.TryParse(tbxID.Text, out _))
            {
                MessageBox.Show("Please enter a valid Product ID", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete this product from {_operationDatabase}?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                var parameters = new Dictionary<string, object>
                {
                    { "id", int.Parse(tbxID.Text) }
                };

                using (var session = _driver.AsyncSession(builder => builder.WithDatabase(_operationDatabase)))
                {
                    var result = await session.ExecuteWriteAsync(async tx =>
                    {
                        var cursor = await tx.RunAsync(
                            "MATCH (p:Product {id: $id}) " +
                            "DELETE p " +
                            "RETURN count(p) AS deletedCount",
                            parameters);

                        return await cursor.SingleAsync();
                    });

                    if (result["deletedCount"].As<int>() == 0)
                    {
                        MessageBox.Show("No product found with the specified ID.",
                            "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MessageBox.Show($"Product deleted successfully from {_operationDatabase}!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadProductsAsync();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting product: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private bool ValidateInputs()
        {
            if (!int.TryParse(tbxID.Text, out _))
            {
                MessageBox.Show("Please enter a valid Product ID", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbxName.Text))
            {
                MessageBox.Show("Product Name is required", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!decimal.TryParse(tbxPrice.Text, out _) || decimal.Parse(tbxPrice.Text) <= 0)
            {
                MessageBox.Show("Please enter a valid Price", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(tbxQuantity.Text, out _) || int.Parse(tbxQuantity.Text) < 0)
            {
                MessageBox.Show("Please enter a valid Quantity", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            tbxID.Clear();
            tbxName.Clear();
            tbxCategory.Clear();
            tbxCompany.Clear();
            tbxPrice.Clear();
            tbxQuantity.Clear();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                tbxID.Text = row.Cells["ProductID"].Value.ToString();
                tbxName.Text = row.Cells["ProductName"].Value.ToString();
                tbxCategory.Text = row.Cells["ProductCategory"].Value.ToString();
                tbxCompany.Text = row.Cells["ProductCompany"].Value.ToString();
                tbxPrice.Text = row.Cells["ProductPrice"].Value.ToString();
                tbxQuantity.Text = row.Cells["ProductQuantity"].Value.ToString();

                // Set operation database to match the selected row's database
                string db = row.Cells["Database"].Value.ToString();
                switch (db)
                {
                    case "marketdb":
                        rdbOperationMarketDB.Checked = true;
                        break;
                    case "productdb":
                        rdbOperationProductDB.Checked = true;
                        break;
                    default:
                        rdbOperationDefaultDB.Checked = true;
                        break;
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _driver?.Dispose();
        }
    }
}