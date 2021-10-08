
namespace KoneData
{
    partial class frmApp
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listData = new System.Windows.Forms.ListView();
            this.menuList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeTractorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.emptyListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeData = new System.Windows.Forms.TreeView();
            this.menuTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToCompareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.openInBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // listData
            // 
            this.listData.AllowDrop = true;
            this.listData.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.listData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listData.FullRowSelect = true;
            this.listData.GridLines = true;
            this.listData.HideSelection = false;
            this.listData.Location = new System.Drawing.Point(0, 0);
            this.listData.Name = "listData";
            this.listData.Size = new System.Drawing.Size(646, 475);
            this.listData.TabIndex = 0;
            this.listData.UseCompatibleStateImageBehavior = false;
            this.listData.View = System.Windows.Forms.View.Details;
            this.listData.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listData_MouseClick);
            // 
            // menuList
            // 
            this.menuList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeTractorToolStripMenuItem,
            this.toolStripMenuItem1,
            this.emptyListToolStripMenuItem});
            this.menuList.Name = "menuList";
            this.menuList.Size = new System.Drawing.Size(171, 54);
            // 
            // removeTractorToolStripMenuItem
            // 
            this.removeTractorToolStripMenuItem.Name = "removeTractorToolStripMenuItem";
            this.removeTractorToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.removeTractorToolStripMenuItem.Text = "Poista kone";
            this.removeTractorToolStripMenuItem.Click += new System.EventHandler(this.removeTractorToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(167, 6);
            // 
            // emptyListToolStripMenuItem
            // 
            this.emptyListToolStripMenuItem.Name = "emptyListToolStripMenuItem";
            this.emptyListToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.emptyListToolStripMenuItem.Text = "Tyhjennä taulukko";
            this.emptyListToolStripMenuItem.Click += new System.EventHandler(this.emptyListToolStripMenuItem_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 475);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeData);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listData);
            this.splitContainer1.Size = new System.Drawing.Size(893, 475);
            this.splitContainer1.SplitterDistance = 243;
            this.splitContainer1.TabIndex = 3;
            // 
            // treeData
            // 
            this.treeData.ContextMenuStrip = this.menuTree;
            this.treeData.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeData.Location = new System.Drawing.Point(0, 0);
            this.treeData.Name = "treeData";
            this.treeData.Size = new System.Drawing.Size(243, 475);
            this.treeData.TabIndex = 5;
            this.treeData.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeData_AfterExpand);
            this.treeData.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeData_NodeMouseClick);
            this.treeData.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeData_NodeMouseDoubleClick);
            // 
            // menuTree
            // 
            this.menuTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToCompareToolStripMenuItem,
            this.toolStripMenuItem2,
            this.openInBrowserToolStripMenuItem});
            this.menuTree.Name = "menuTree";
            this.menuTree.Size = new System.Drawing.Size(184, 54);
            // 
            // addToCompareToolStripMenuItem
            // 
            this.addToCompareToolStripMenuItem.Name = "addToCompareToolStripMenuItem";
            this.addToCompareToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.addToCompareToolStripMenuItem.Text = "Lisää taulukkoon";
            this.addToCompareToolStripMenuItem.Click += new System.EventHandler(this.addToCompareToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(180, 6);
            // 
            // openInBrowserToolStripMenuItem
            // 
            this.openInBrowserToolStripMenuItem.Name = "openInBrowserToolStripMenuItem";
            this.openInBrowserToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.openInBrowserToolStripMenuItem.Text = "Avaa sivu selaimessa";
            this.openInBrowserToolStripMenuItem.Click += new System.EventHandler(this.openInBrowserToolStripMenuItem_Click);
            // 
            // frmApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 475);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmApp";
            this.Text = "KoneData";
            this.menuList.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuTree.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listData;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip menuTree;
        private System.Windows.Forms.TreeView treeData;
        private System.Windows.Forms.ToolStripMenuItem openInBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToCompareToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip menuList;
        private System.Windows.Forms.ToolStripMenuItem removeTractorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem emptyListToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    }
}

