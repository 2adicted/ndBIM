using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ndBIM
{
    class Utils
    {

        public const string Caption = "Courtesy of Revit API Labs";
        /// <summary>
        /// truncate string and add '..' at the end
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string Truncate(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
                return source + "..";
            }
            else
            {
                return source;
            }
        }
        /// <summary>
        /// Check if a string contains unallowed characters
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        internal static bool UnallowedChacarcters(string text)
        {
            var regexItem = new Regex("^[a-zA-Z0-9 _!-]+$");
            bool result = regexItem.IsMatch(text);
            return (result ? true : false);
        }
        /// <summary>
        /// Check if it's a Family Document
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        internal static Autodesk.Revit.DB.Document checkDoc(Autodesk.Revit.DB.Document document)
        {
            if (!document.IsFamilyDocument)
            {
                return document;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// MessageBox wrapper for error message.
        /// </summary>
        public static void ErrorMsg(string msg)
        {
            Debug.WriteLine(msg);

            //WinForms.MessageBox.Show( msg, Caption, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error );

            TaskDialog d = new TaskDialog(Caption);
            d.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
            d.MainInstruction = msg;
            d.Show();
        }
        public static DefinitionFile GetSharedParamsFile(
      Autodesk.Revit.ApplicationServices.Application app)
        {
            // Get current shared params file name
            string sharedParamsFileName;
            try
            {
                sharedParamsFileName = app.SharedParametersFilename;
            }
            catch (Exception ex)
            {
                ErrorMsg("No shared params file set:" + ex.Message);
                return null;
            }
            if (0 == sharedParamsFileName.Length)
            {
                var filename = "";

                using (var ofd = new OpenFileDialog())
                {
                    DialogResult result = ofd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName))
                    {
                        filename = ofd.FileName;
                    }
                }
                StreamWriter stream;
                stream = new StreamWriter(filename);
                stream.Close();
                app.SharedParametersFilename = filename;
                sharedParamsFileName = app.SharedParametersFilename;
            }
            // Get the current file object and return it
            DefinitionFile sharedParametersFile;
            try
            {
                sharedParametersFile = app.OpenSharedParameterFile();
            }
            catch (Exception ex)
            {
                ErrorMsg("Cannnot open shared params file:" + ex.Message);
                sharedParametersFile = null;
            }
            return sharedParametersFile;
        }

        /// <summary>
        /// Helper to get shared params group.
        /// </summary>
        public static DefinitionGroup GetOrCreateSharedParamsGroup(
         DefinitionFile sharedParametersFile,
         string groupName)
        {
            DefinitionGroup g = sharedParametersFile.Groups.get_Item(groupName);
            if (null == g)
            {
                try
                {
                    g = sharedParametersFile.Groups.Create(groupName);
                }
                catch (Exception)
                {
                    g = null;
                }
            }
            return g;
        }
        public static bool IsAlreadyBound(Autodesk.Revit.DB.Document doc, string paramName)
        {
            DefinitionBindingMapIterator iter
                 = doc.ParameterBindings.ForwardIterator();

            while (iter.MoveNext())
            {
                Definition def = iter.Key;
                ElementBinding elemBind
                  = (ElementBinding)iter.Current;

                // Got param name match

                if (paramName.Equals(def.Name,
                  StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Helper to get shared params definition.
        /// </summary>
        public static Definition GetOrCreateSharedParamsDefinition(
          DefinitionGroup defGroup,
          ParameterType defType,
          string defName,
          string tooltip,
          bool visible)
        {
            Definition definition
              = defGroup.Definitions.get_Item(
                defName);

            if (null == definition)
            {
                try
                {
                    //definition = defGroup.Definitions.Create( defName, defType, visible ); // 2014

                    ExternalDefinitionCreationOptions opt
                      = new ExternalDefinitionCreationOptions(
                        defName, defType); // 2015

                    opt.Visible = visible;
                    opt.Description = tooltip;
                    definition = defGroup.Definitions.Create(opt); // 2015
                }
                catch (Exception)
                {
                    TaskDialog.Show("Parameter exists.", "Parameter already exist. No action taken.");
                    definition = null;
                }
            }
            return definition;
        }

        /// <summary>
        /// Get GUID for a given shared param name.
        /// </summary>
        /// <param name="app">Revit application</param>
        /// <param name="defGroup">Definition group name</param>
        /// <param name="defName">Definition name</param>
        /// <returns>GUID</returns>
        public static Guid SharedParamGUID(Autodesk.Revit.ApplicationServices.Application app, string defGroup, string defName)
        {
            Guid guid = Guid.Empty;
            try
            {
                DefinitionFile file = app.OpenSharedParameterFile();
                DefinitionGroup group = file.Groups.get_Item(defGroup);
                Definition definition = group.Definitions.get_Item(defName);
                ExternalDefinition externalDefinition = definition as ExternalDefinition;
                guid = externalDefinition.GUID;
            }
            catch (Exception)
            {
            }
            return guid;
        }
        public static CategorySet AssignableCategories(UIApplication uiapp, Document doc)
        {
            CategorySet catSet = uiapp.Application.Create.NewCategorySet();
            Categories categories = doc.Settings.Categories;

            foreach (Category cat in categories)
            {
                if (cat.AllowsBoundParameters)
                {
                    catSet.Insert(cat);
                }
            }
            return catSet;
        }
    }
}
