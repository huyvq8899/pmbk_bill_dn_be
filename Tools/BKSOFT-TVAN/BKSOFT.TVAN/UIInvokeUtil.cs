using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BKSOFT.TVAN
{
    public class UIInvokeUtil
    {
        public static void InvokeAddNode(TreeView treeView, TreeNode node)
        {
            if (treeView.InvokeRequired)
            {
                treeView.Invoke(new MethodInvoker(() => { InvokeAddNode(treeView, node); }));
            }
            else
            {
                treeView.Nodes.Add(node);
            }
        }

        public static void AddNode(TreeView treeview, TreeNode node)
        {
            MethodInvoker mi = delegate
            {
                treeview.Nodes.Add(node);
            };

            if (treeview.InvokeRequired)
            {
                treeview.Invoke(mi);
            }
            else
            {
                mi();
            }
        }

        public static void ExpandAll(TreeView treeview)
        {
            MethodInvoker mi = delegate
            {
                treeview.ExpandAll();
            };

            if (treeview.InvokeRequired)
            {
                treeview.Invoke(mi);
            }
            else
            {
                mi();
            }
        }

        public static void ClearNodes(TreeView treeview)
        {
            MethodInvoker mi = delegate
            {
                treeview.Nodes.Clear();
            };

            if (treeview.InvokeRequired)
            {
                treeview.Invoke(mi);
            }
            else
            {
                mi();
            }
        }

        public static TreeNode GetSelectedNode(TreeView treeview)
        {
            TreeNode node = null;

            MethodInvoker mi = delegate
            {
                node = treeview.SelectedNode;
            };

            if (treeview.InvokeRequired)
                treeview.Invoke(mi);
            else
                mi();

            return node;
        }

        public static TreeNodeCollection GetNodes(TreeView treeview)
        {
            TreeNodeCollection collection = null;

            MethodInvoker mi = delegate
            {
                collection = treeview.Nodes;
            };

            if (treeview.InvokeRequired)
            {
                treeview.Invoke(mi);
            }
            else
            {
                mi();
            }

            return collection;
        }

        public static void ClearTreeViewNodes(TreeView tv)
        {
            if (tv.InvokeRequired)
            {
                tv.Invoke(new Action<TreeView>(ClearTreeViewNodes), new object[] { tv });
            }
            else
            {
                tv.Nodes.Clear();
            }
        }

        public static void InvokeMessageRichText(RichTextBox rtb, string msg)
        {
            // Display text.
            if (rtb.InvokeRequired)
            {
                rtb.Invoke(new MethodInvoker(delegate
                {
                    rtb.AppendText(msg);
                    rtb.ScrollToCaret();
                }));
            }
            else
            {
                rtb.AppendText(msg);
                rtb.ScrollToCaret();
            }
        }

        public static void InvokeMessageLableText(Label label, string msg)
        {
            // Display text.
            if (label.InvokeRequired)
            {
                label.Invoke(new MethodInvoker(delegate
                {
                    label.Text = msg;
                }));
            }
            else
            {
                label.Text = msg;
            }
        }
    }
}
