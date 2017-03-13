using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ndBIM
{
    internal class ParameterManager
    {
        private UIApplication uiapp;
        private Document doc;
        private int num;
        private Dictionary<int, string> parameterMap;

        private List<List<string>> projectData;
        private List<string[]> projectDataArray;
        private List<Tuple<string, string>> projectParameters;
        private Dictionary<string, string> parameterTypeMap;

        internal ParameterManager(UIApplication uiapp, Document doc)
        {
            this.uiapp = uiapp;
            this.doc = doc;

            projectData = new List<List<string>>();
            projectParameters = new List<Tuple<string, string>>();
            parameterMap = new Dictionary<int, string>();
            parameterTypeMap = new Dictionary<string, string>();
        }

        internal void ExportExcel()
        {
            this.num = Utils.GetIterations(uiapp, doc);
            List<string> names = Parameters.ProjectParameterNames(num);
            ParameterDispatcher();

            UtilsExcel.CreateExcel(names, projectData, num);
        }

        internal void ImportExcel()
        {
            projectDataArray = UtilsExcel.ReadExcel();
            if(projectDataArray == null)
            {
                return;
            }
            this.num = Parameters.ImportNum(projectDataArray.First().Length);
            ParameterPopuate();
        }

        internal void ParameterPopuate()
        {
            // UPDATE AFTER COLUMNS ADJUSTMENT
            parameterMap = Parameters.parameterMap(num);
            parameterTypeMap = Parameters.parameterTypeMap(num);

            using (Transaction t = new Transaction(doc, "Import parameters"))
            {
                t.Start();
                foreach (string[] arr in projectDataArray)
                {
                    try
                    {
                        Element el = doc.GetElement(arr[0]);
                        // UPDATE AFTER COLUMNS ADJUSTMENT (2 becomes 1 - last two numbers)
                        for(int i = 1; i < parameterMap.Count-2; i ++)
                        {
                            string name = parameterMap[i];
                            if (Parameters.IsInsanceParameter(name, parameterTypeMap))
                            {
                                Parameter p = el.LookupParameter(parameterMap[i]);
                                if (p != null)
                                {
                                    p.Set(arr[i]);
                                }
                            }
                            else
                            {
                                Element type = doc.GetElement(el.GetTypeId());
                                {
                                    if(type != null)
                                    {
                                        Parameter p = el.LookupParameter(parameterMap[i]);
                                        if (p != null)
                                        {
                                            p.Set(arr[i]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        TaskDialog.Show("Failed.", ex.ToString());
                    }
                }
                t.Commit();
            }
        }
        internal void ParameterDispatcher()
        {
            CategorySet catSet = Utils.AssignableCategories(uiapp, doc);
            projectParameters = Parameters.ProjectData(num);

            // For each Category
            foreach (Category cat in catSet)
            {
                IList<Element> isntanceCollector = new FilteredElementCollector(doc).OfCategoryId(cat.Id).WhereElementIsNotElementType().ToElements();
                string catName = cat.Name;
                if (isntanceCollector.Count < 1) continue;

                // For each element
                foreach (Element el in isntanceCollector)
                {
                    try
                    {
                        List<string> elParameters = new List<string>();
                        Element type = doc.GetElement(el.GetTypeId());

                        elParameters.Add(el.UniqueId.ToString());

                        foreach(Tuple<string, string> tuple in projectParameters)
                        {
                            if(tuple.Item2.Equals("Instance"))
                            {
                                Parameter p = el.LookupParameter(tuple.Item1);
                                string s = "  ";
                                if (p != null && p.HasValue && !String.IsNullOrEmpty(p.AsString())) s = p.AsString();
                                elParameters.Add(s);
                            }
                            else
                            {
                                if (type == null)
                                {
                                    elParameters.Add("No Type");
                                    continue;
                                }
                                Parameter p = type.LookupParameter(tuple.Item1);
                                string s = "  ";
                                if (p != null && p.HasValue) s = p.AsString();
                                elParameters.Add(s);
                            }
                        }
                        if(type == null)
                        {
                            elParameters.Add("No Type");
                        }
                        else
                        {
                            elParameters.Add(type.Name); 
                        }
                        elParameters.Add(catName);
                        projectData.Add(elParameters);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

    }
}
