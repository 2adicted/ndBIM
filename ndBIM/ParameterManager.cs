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
        internal void ExportVICO()
        {
            this.num = 0;
            List<string> names = Parameters.ProjectParameterNames();
            ParameterDispatcher();

            UtilsExcel.CreateExcel(names, projectData, "VICO");
        }

        internal void ExportExcel()
        {
            this.num = Utils.GetIterations(uiapp, doc);
            List<string> names = Parameters.ProjectParameterNames(num);
            ParameterDispatcher();

            UtilsExcel.CreateExcel(names, projectData, num, "Budget Preparation");
        }

        internal void ImportExcel()
        {
            projectDataArray = UtilsExcel.ReadExcel();
            if(projectDataArray == null)
            {
                return;
            }
            this.num = Parameters.ImportNum(projectDataArray.First().Length);
            ParameterPopulate();
        }

        internal void ParameterPopulate()
        {
            // UPDATE AFTER COLUMNS ADJUSTMENT
            parameterMap = Parameters.parameterMap(num);
            parameterTypeMap = Parameters.parameterTypeMap(num);

            Dictionary<string, string> type = new Dictionary<string, string>();
            
                using (Transaction t = new Transaction(doc, "Import parameters"))
                {
                    t.Start();
                    foreach (string[] arr in projectDataArray)
                    {
                        Element el = doc.GetElement(arr[0]);
                        // UPDATE AFTER COLUMNS ADJUSTMENT (2 becomes 1 - last two numbers)
                        for (int i = 1; i < parameterMap.Count - 2; i++)
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
                                type[String.Format("{0}:{1}", el.GetTypeId().IntegerValue.ToString(), parameterMap[i])] = arr[i];
                            }
                        }
                    }
                    t.Commit();
                }

            using (Transaction t1 = new Transaction(doc, "Importe Type Parameters"))
            {
                t1.Start();
                foreach(var pair in type)
                {
                    string[] sp = pair.Key.Split(':');
                    if (sp[0].Equals("-1")) continue;
                    Element el = doc.GetElement(new ElementId(Convert.ToInt32(sp[0])));
                    Parameter p = el.LookupParameter(sp[1]);
                    if (p != null)
                    {
                        p.Set(pair.Value);
                    }
                }
                t1.Commit();
            }
        }
    
        internal void ParameterDispatcher()
        {
            CategorySet catSet = Utils.AssignableCategories(uiapp, doc);
            if(num > 0)
            {
                projectParameters = Parameters.ProjectData(num);
            }
            else
            {
                projectParameters = Parameters.ProjectData();
            }

            // For each Category
            foreach (Category cat in catSet)
            {
                IList<Element> isntanceCollector = new FilteredElementCollector(doc).OfCategoryId(cat.Id).WhereElementIsNotElementType().ToElements();
                HashSet<string> unique = new HashSet<string>();
                string catName = cat.Name;
                if (isntanceCollector.Count < 1) continue;

                // For each element
                foreach (Element el in isntanceCollector)
                {
                    try
                    {
                        List<string> elParameters = new List<string>();
                        Element type = doc.GetElement(el.GetTypeId());

                        if(num > 0) elParameters.Add(el.UniqueId.ToString());

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
                        if (num > 0)
                        {
                            elParameters.Add(catName);
                            projectData.Add(elParameters);
                        }
                        if (num == 0)
                        {
                            if (unique.Add(elParameters.Aggregate((i, j) => i + "," + j)))
                            {
                                projectData.Add(elParameters);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}
