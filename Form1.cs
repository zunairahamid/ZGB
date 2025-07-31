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
        

        public Form1()
        {
            InitializeComponent();

            // Initialize Neo4j driver (update with your credentials)
            _driver = GraphDatabase.Driver(
                "bolt://localhost:7687",
                AuthTokens.Basic("neo4j", "yourpassword"));

            // Set up DataTable and DataGridView
            InitializeDataTable();
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

            dataGridView1.DataSource = _productsTable;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                _productsTable.Rows.Clear();

                using (var session = _driver.AsyncSession())
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
                            record["ProductQuantity"].As<int>());
                    }
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

        private async void btnInsert_Click_1(object sender, EventArgs e)
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

                using (var session = _driver.AsyncSession())
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
                        MessageBox.Show("Product inserted successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                using (var session = _driver.AsyncSession())
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

                    MessageBox.Show("Product updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private async void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxID.Text) || !int.TryParse(tbxID.Text, out _))
            {
                MessageBox.Show("Please enter a valid Product ID", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this product?",
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

                using (var session = _driver.AsyncSession())
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

                    MessageBox.Show("Product deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
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
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _driver?.Dispose();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}