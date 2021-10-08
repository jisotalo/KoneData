/*
MIT License

Copyright (c) 2021 Jussi Isotalo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KoneData
{
    public partial class frmApp : Form
    {
        private KoneService koneService = new KoneService();

        public frmApp()
        {
            InitializeComponent();
            treeData.Nodes.Add("Ladataan...");
            this.Show();

            PopulateBrands();
        }



        /// <summary>
        /// Fetches all tractor brands and populates the tree node
        /// </summary>
        private async void PopulateBrands()
        {
            try
            {
                var brands = await koneService.GetTractorBrands();

                treeData.Nodes.Clear();

                brands.ForEach(brand =>
                {
                    var node = new TreeNode
                    {
                        Text = brand.Name,
                        Tag = brand
                    };

                    node.Nodes.Add("Ladataan...");

                    treeData.Nodes.Add(node);
                });


            } catch (Exception ex)
            {
                MessageBox.Show($"Virhe hakiessa traktorimerkkejä: {ex.Message}", "Virhe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /// <summary>
        /// Adds given tractor model to compare table
        /// </summary>
        /// <param name="model"></param>
        private void addModelToCompare(TractorModel model)
        {
            try
            {
                var list = listData;
                list.BeginUpdate();


                if (list.Columns.Count == 0)
                {
                    list.Columns.Add("");
                }

                //Some properties are always added
                if (list.Items.Count == 0)
                {
                    list.Items.Add(new ListViewItem
                    {
                        Text = "Merkki",
                        Font = new Font(list.Font, FontStyle.Bold),
                        UseItemStyleForSubItems = false
                    });

                    list.Items.Add(new ListViewItem
                    {
                        Text = "Sarja",
                        Font = new Font(list.Font, FontStyle.Bold),
                        UseItemStyleForSubItems = false
                    });

                    list.Items.Add(new ListViewItem
                    {
                        Text = "Malli",
                        Font = new Font(list.Font, FontStyle.Bold),
                        UseItemStyleForSubItems = false
                    });

                    list.Items.Add("");
                }

                //Adding values to static properties
                list.Columns.Add(model.Name);
                list.Items[0].SubItems.Add(model.Series.Brand.Name);
                list.Items[1].SubItems.Add(model.Series.Name);
                list.Items[2].SubItems.Add(model.Name);
                list.Items[3].SubItems.Add("");

                //Adding all (dynamic) model properties
                model.Properties.ForEach(prop =>
                {
                    ListViewItem found = null;

                    foreach (ListViewItem i in list.Items)
                    {
                        if (i.SubItems[0].Text == prop.NameFI)
                        {
                            found = i;
                            break;
                        }
                    }

                    //If not found, this is a new property -> add it
                    if (found == null)
                    {
                        found = list.Items.Add(new ListViewItem
                        {
                            Text = prop.NameFI,
                            Font = new Font(list.Font, FontStyle.Bold),
                            UseItemStyleForSubItems = false
                        });
                    }

                    //Finally adding the value
                    found.SubItems.Add(prop.Value);
                });


                //Make sure that every column has at least empty value
                //so that values are kept at correct column (ListView was not the best choise)
                foreach (ListViewItem item in list.Items)
                {
                    while (item.SubItems.Count < list.Columns.Count)
                    {
                        item.SubItems.Add("");
                    }
                }

                //Auto resize columns by data and header text
                foreach (ColumnHeader col in list.Columns)
                {
                    col.Width = -2;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Virhe lisätessä traktorimallia vertailuun: {ex.Message}", "Virhe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                listData.EndUpdate();
            }
        }





        /// <summary>
        /// Expands given tree node (= loads data)
        /// </summary>
        /// <param name="expandedNode"></param>
        /// <returns></returns>
        private async Task ExpandTreeNode(TreeNode expandedNode)
        {
            try
            {
                //If it's already loaded, nothing to do
                if (expandedNode.Nodes[0].Text != "Ladataan...")
                    return;

                if (expandedNode.Tag is TractorBrand)
                {
                    //Brand expanded
                    var brand = expandedNode.Tag  as TractorBrand;
                    var series = await koneService.GetTractorBrandSeries(brand.Url);
                    expandedNode.Nodes.Clear();

                    series.ForEach(serie =>
                    {
                        serie.Brand = brand;

                        var node = new TreeNode
                        {
                            Text = serie.Name,
                            Tag = serie
                        };

                        node.Nodes.Add("Ladataan...");

                        expandedNode.Nodes.Add(node);
                    });
                }
                else if (expandedNode.Tag is TractorSeries)
                {
                    //Series expanded
                    var serie = expandedNode.Tag as TractorSeries;
                    var models = await koneService.GetTractorModels(serie.Url);
                    expandedNode.Nodes.Clear();

                    models.ForEach(model =>
                    {
                        //Adding brand & series to model data
                        model.Series = serie;

                        var node = new TreeNode
                        {
                            Text = model.Name,
                            Tag = model
                        };


                        expandedNode.Nodes.Add(node);
                    });
                }
                else
                {
                    throw new Exception("Unknown tree node expanded?");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Virhe hakiessa traktoritietoja: {ex.Message}", "Virhe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /// <summary>
        /// Called when tree view node has been expanded by user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void treeData_AfterExpand(object sender, TreeViewEventArgs e)
        {
            await ExpandTreeNode(e.Node);
        }


        /// <summary>
        /// Tree view node is double-clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeData_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is TractorModel)
            {
                addModelToCompare(e.Node.Tag as TractorModel);
            }
        }


        /// <summary>
        /// Tree view right click menu -> Open in browser is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openInBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var node = treeData.SelectedNode;

                if (node.Tag is IHasUrlAddr || node.Parent.Tag is IHasUrlAddr)
                {
                    var process = new Process();
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = node.Tag is IHasUrlAddr ? (node.Tag as IHasUrlAddr).Url : (node.Parent.Tag as IHasUrlAddr).Url;
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Virhe avatessa URL-osoitetta selaimessa: {ex.Message}", "Virhe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /// <summary>
        /// Tree view right click menu -> Add to compare is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void addToCompareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var node = treeData.SelectedNode;

                if (node.Tag is TractorModel)
                {
                    addModelToCompare(node.Tag as TractorModel);
                }
                else if (node.Tag is TractorSeries)
                {
                    //We might need to fetch data first
                    await ExpandTreeNode(node);

                    foreach (TreeNode modelNode in node.Nodes)
                    {
                        addModelToCompare(modelNode.Tag as TractorModel);
                    }

                    //Displaying for the user
                    node.Expand();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Virhe lisätessä vertailuun: {ex.Message}", "Virhe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /// <summary>
        /// Tree view node is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeData_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeData.SelectedNode = e.Node;

            if(e.Button == MouseButtons.Right && e.Node.Tag != null)
            {
                //Allow adding to compare only for series + models
                addToCompareToolStripMenuItem.Enabled = !(e.Node.Tag is TractorBrand);
                menuTree.Show();
            }
        }


        /// <summary>
        /// Data table right click menu -> Empty list is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void emptyListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listData.Items.Clear();
            listData.Columns.Clear();
        }


        /// <summary>
        /// Data table is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listData_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    var item = listData.HitTest(e.Location);

                    if (item != null)
                    {
                        var clickedColumn = item.Item.SubItems.IndexOf(item.SubItem);

                        removeTractorToolStripMenuItem.Enabled = clickedColumn > 0;
                        removeTractorToolStripMenuItem.Tag = clickedColumn;
                        removeTractorToolStripMenuItem.Text = clickedColumn > 0 ? $"Poista {listData.Items[0].SubItems[clickedColumn].Text} {listData.Items[2].SubItems[clickedColumn].Text}" : "Poista";
                    }

                    menuList.Show(listData.PointToScreen(e.Location));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Virhe taulukon käsittelyssä: {ex.Message}", "Virhe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Data table right click menu -> Remove tractor is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeTractorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (removeTractorToolStripMenuItem.Tag != null && removeTractorToolStripMenuItem.Tag is int)
                {
                    var column = (int)removeTractorToolStripMenuItem.Tag;

                    listData.BeginUpdate();
                    listData.Columns.RemoveAt(column);

                    foreach (ListViewItem item in listData.Items)
                    {
                        item.SubItems.RemoveAt(column);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Virhe traktorin poistossa: {ex.Message}", "Virhe", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                listData.EndUpdate();
            }
        }
    }
}
