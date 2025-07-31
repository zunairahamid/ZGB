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
        public DataGridView dataGridView;

        public Form1()
        {
            InitializeComponent();
            // Initialize Neo4j driver (update URI and credentials)
            _driver = GraphDatabase.Driver(
                "bolt://localhost:7687",
                AuthTokens.Basic("neo4j", "yourpassword"));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ProductData(); // Load data when form loads
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _driver?.Dispose(); // Clean up driver when form closes
        }

        private async void ProductData()
        {
            try
            {
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

                    var dt = new DataTable();
                    dt.Columns.Add("ProductID", typeof(int));
                    dt.Columns.Add("ProductName", typeof(string));
                    dt.Columns.Add("ProductCategory", typeof(string));
                    dt.Columns.Add("ProductCompany", typeof(string));
                    dt.Columns.Add("ProductPrice", typeof(decimal));
                    dt.Columns.Add("ProductQuantity", typeof(int));

                    foreach (var record in result)
                    {
                        dt.Rows.Add(
                            record["ProductID"].As<int>(),
                            record["ProductName"].As<string>(),
                            record["ProductCategory"].As<string>(),
                            record["ProductCompany"].As<string>(),
                            record["ProductPrice"].As<decimal>(),
                            record["ProductQuantity"].As<int>());
                    }

                    if (dataGridView != null && !dataGridView.IsDisposed)
                    {
                        dataGridView.Invoke((MethodInvoker)delegate {
                            dataGridView.DataSource = dt;
                            dataGridView.Refresh();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnInsert_Click_1(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
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
                    await session.ExecuteWriteAsync(async tx =>
                    {
                        await tx.RunAsync(
                            "CREATE (p:Product {id: $id, name: $name, category: $category, " +
                            "company: $company, price: $price, quantity: $quantity})",
                            parameters);
                    });
                }

                MessageBox.Show("Product inserted successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await Task.Run(() => ProductData());
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting product: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
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
                }

                MessageBox.Show("Product updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await Task.Run(() => ProductData());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                }

                MessageBox.Show("Product deleted successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await Task.Run(() => ProductData());
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting product: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (dataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView.SelectedRows[0];
                tbxID.Text = row.Cells["ProductID"].Value.ToString();
                tbxName.Text = row.Cells["ProductName"].Value.ToString();
                tbxCategory.Text = row.Cells["ProductCategory"].Value.ToString();
                tbxCompany.Text = row.Cells["ProductCompany"].Value.ToString();
                tbxPrice.Text = row.Cells["ProductPrice"].Value.ToString();
                tbxQuantity.Text = row.Cells["ProductQuantity"].Value.ToString();
            }
        }
    }
}