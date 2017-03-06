using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ndBIM
{
    public partial class ndBIMInterface : System.Windows.Forms.Form
    {
        // Revit Scope
        private UIApplication uiapp;
        private UIDocument uidoc;
        private Autodesk.Revit.DB.Document doc;
        // End Revit Scope

        private string message;

        private string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                MessageBox.Show(value);
            }
        }

        private List<ProjectParameter> group_01, group_02;
        private readonly string title = "nbBIM";

        private float scale_x, scale_y;
        private int nBudget, pXSize;

        public ndBIMInterface(UIApplication uiapp)
        {
            InitializeComponent();
            
            // Revit Scope
            this.uiapp = uiapp;
            this.uidoc = uiapp.ActiveUIDocument;
            this.doc = uidoc.Document;
            if (doc == null)
            {
                return;
            }
            // End Revit scope

            this.scale_x = this.CreateGraphics().DpiX / 100;
            this.scale_y = this.CreateGraphics().DpiY / 100;

            this.nBudget = 1;
            this.pXSize = 100;

            ThisDocumentCollect();
        }

        internal void ThisDocumentCollect()
        {
            // Revit Scope
            this.uidoc = uiapp.ActiveUIDocument;
            this.doc = Utils.checkDoc(uidoc.Document);
            if (doc == null)
            {
                this.InvalidDocument();
                return;
            }
            this.Text = title + String.Format(" - {0}", Utils.Truncate(doc.Title, 10));
            // End Revit Scope
            SetParameters();
            DisplayData();
        }

        private void SetParameters()
        {
            group_01 = new List<ProjectParameter>();
            group_02 = new List<ProjectParameter>();

            foreach (string s in Parameters.group01(nBudget))
            {
                group_01.Add(new ProjectParameter(s, "Budget"));
            }
            foreach (Tuple<string, string, string> t in Parameters.group02())
            {
                group_02.Add(new ProjectParameter(t.Item1, t.Item2, t.Item3, "BIM 4D and 5D"));
            }
        }

        private void DisplayData()
        {
            flowPanel_01.Controls.Clear();
            flowPanel_02.Controls.Clear();

            flowPanel_01.WrapContents = false;

            List<System.Windows.Forms.Control> itemsLeft = new List<System.Windows.Forms.Control>();
            //List<System.Windows.Forms.Control> itemsRight = new List<System.Windows.Forms.Control>();

            foreach (ProjectParameter param in group_01)
            {
                System.Windows.Forms.Label lbl = createLabel(param);
                System.Windows.Forms.TextBox txtInput = createInputTextbox(param);

                itemsLeft.Add(lbl);
                //itemsRight.Add(txtInput);
            }
            
            this.flowPanel_01.Controls.AddRange(itemsLeft.ToArray());
            this.flowPanel_01.SetFlowBreak(itemsLeft.ToArray()[itemsLeft.Count - 1], true);
            //this.flowPanel_01.Controls.AddRange(itemsRight.ToArray());
            this.flowPanel_01.WrapContents = true;

            itemsLeft.Clear();
            //itemsRight.Clear();

            foreach (ProjectParameter param in group_02)
            {
                System.Windows.Forms.Label lbl = createLabel(param);
                System.Windows.Forms.TextBox txtInput = createInputTextbox(param);

                itemsLeft.Add(lbl);
                //itemsRight.Add(txtInput);
            }

            this.flowPanel_02.Controls.AddRange(itemsLeft.ToArray());
            this.flowPanel_02.SetFlowBreak(itemsLeft.ToArray()[itemsLeft.Count - 1], true);
            //this.flowPanel_02.Controls.AddRange(itemsRight.ToArray());
            this.flowPanel_02.WrapContents = true;
        }

        private System.Windows.Forms.Label createLabel(ProjectParameter item)
        {
            System.Windows.Forms.Label lbl = new System.Windows.Forms.Label();

            string s = item.Name;
            lbl.AutoSize = true;
            //lbl.MaximumSize = new Size(Convert.ToInt32(scale_x * 200), 0);
            lbl.MinimumSize = new Size(Convert.ToInt32(scale_x * 190), 0);
            //lbl.Font = new Font("Arial", 8);
            lbl.Margin = new Padding(0, 16, 0, 0);
            //txt.KeyDown += new KeyEventHandler(textBox_KeyDown);
            //txt.LostFocus += textBoxLostFocus;
            //txt.TextChanged += textBoxEdited;
            lbl.Name = s;
            lbl.Text = s;
            //lbl.BackColor = System.Drawing.SystemColors.Control;
            //lbl.BorderStyle = BorderStyle.None;
            //lbl.Padding = new Padding(3, 4, 3, 3);
            //lbl.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            lbl.Tag = item;
            toolTip1.SetToolTip(lbl, item.Tooltip);

            return lbl;
        }       

        private System.Windows.Forms.TextBox createInputTextbox(ProjectParameter item)
        {
            System.Windows.Forms.TextBox txt = new System.Windows.Forms.TextBox();

            string s = item.Name;

            txt.Size = new Size(Convert.ToInt32(scale_x * 205), 10);
            //txt.Dock = DockStyle.Fill;
            //txt.KeyDown += new KeyEventHandler(textBox_KeyDown);
            //txt.LostFocus += textBoxLostFocus;
            //txt.TextChanged += textBoxEdited;
            txt.Name = s + "_input";
            //txt.BackColor = System.Drawing.SystemColors.Control;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Margin = new Padding(0, 10, 0, 0);
            txt.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            txt.Tag = item;
            toolTip1.SetToolTip(txt, item.Tooltip);

            return txt;
        }


        /// <summary>
        /// Trigers if no valid Family Document is available on Refresh Document
        /// </summary>
        private void InvalidDocument()
        {
            this.mainPanel.Controls.Clear();
            this.mainPanel.Controls.Add(error("Please, run in a Family Document"));
            this.doc = null;
        }
        private Label error(string message)
        {
            Label error = new Label();
            error.Text = message;
            error.TextAlign = ContentAlignment.MiddleCenter;
            error.AutoSize = true;
            error.Padding = new Padding(Convert.ToInt32(scale_x * 55), 35, 0, 0); //check if it works
            error.ForeColor = System.Drawing.Color.DarkGray;

            return error;
        }

        #region Action
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if ((int)numericUpDown1.Value < 1) numericUpDown1.Value = 1;
            nBudget = (int)numericUpDown1.Value;
            ThisDocumentCollect();
        }

        private void populateButton_Click(object sender, EventArgs e)
        {
            foreach(ProjectParameter parameter in group_01)
            {
                if (!Populate(parameter))
                {
                    Cancel();
                    return;
                }
            }
            foreach(ProjectParameter parameter in group_02)
            {
                if (!Populate(parameter))
                {
                    Cancel();
                    return;
                }
            }
            Message = "Sucessfully created shared parameters.";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Message = "Cancelled.";
            Cancel();
        }
        private void Cancel()
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Message = "Cancelled.";
                Cancel();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        private bool Populate(ProjectParameter parameter)
        {
            if(Utils.IsAlreadyBound(doc, parameter.Name))
            {
                Cancel();
                Message = "Parameter already exists";
                return false;
            }
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Create per doc Parameters");
                // get the current shared params definition file
                DefinitionFile sharedParamsFile = Utils.GetSharedParamsFile(uiapp.Application);
                if (null == sharedParamsFile)
                {
                    Message = "Error getting the shared params file.";
                    return false;
                }
                // get or create the shared params group
                DefinitionGroup sharedParamsGroup = Utils.GetOrCreateSharedParamsGroup(
                  sharedParamsFile, parameter.GroupName);

                if (null == sharedParamsGroup)
                {
                    Message = "Error getting the shared params group.";
                    return false;
                }
                // param
                Definition docParamDef = Utils.GetOrCreateSharedParamsDefinition(
                  sharedParamsGroup, ParameterType.Text, parameter.Name, parameter.Tooltip, true);

                if (null == docParamDef)
                {
                    Message = "Error creating visible per-doc parameter.";
                    return false;
                }
                try
                {
                    CategorySet catSet = uiapp.Application.Create.NewCategorySet();
                    Categories categories = doc.Settings.Categories;

                    foreach(Category cat in categories)
                    {
                        if (cat.AllowsBoundParameters)
                        {
                            catSet.Insert(cat);
                        }                        
                    }

                    Autodesk.Revit.DB.Binding binding = null;

                    if (parameter.Type.Equals("Instance"))
                    {
                        binding = uiapp.Application.Create.NewInstanceBinding(catSet);
                        if (parameter.GroupName.Equals("Budget"))
                        {
                            doc.ParameterBindings.Insert(docParamDef, binding);
                        }
                        else
                        {
                            doc.ParameterBindings.Insert(docParamDef, binding, BuiltInParameterGroup.PG_CONSTRUCTION);
                        }
                    }
                    else
                    {
                        binding = uiapp.Application.Create.NewTypeBinding(catSet);
                        if (parameter.GroupName.Equals("Budget"))
                        {
                            doc.ParameterBindings.Insert(docParamDef, binding);
                        }
                        else
                        {
                            doc.ParameterBindings.Insert(docParamDef, binding, BuiltInParameterGroup.PG_CONSTRUCTION);
                        }
                    }
                }
                catch (Exception e)
                {
                    Message = "Error binding shared parameter: " + e.Message;
                    return false;
                }

                tx.Commit();

                return true;
            }
        }
    }
}
