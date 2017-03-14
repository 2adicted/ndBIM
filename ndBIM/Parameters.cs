using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ndBIM
{
    static class Parameters
    {
        internal static int ImportNum(int count)
        {
            // UPDATE AFTER COLUMNS ADJUSTMENT (9 to 8 - minus 1 column)
            return (count - 9) / 3;
        }
        internal static List<int> bannedRevitCategories()
        {
            List<int> list = new List<int>
            {
                (int) BuiltInCategory.OST_BeamAnalytical,
                (int) BuiltInCategory.OST_BraceAnalytical,
                (int) BuiltInCategory.OST_ColumnAnalytical,
                (int) BuiltInCategory.OST_FloorAnalytical,
                (int) BuiltInCategory.OST_FoundationSlabAnalytical,
                (int) BuiltInCategory.OST_IsolatedFoundationAnalytical,
                (int) BuiltInCategory.OST_AnalyticalNodes,
                (int) BuiltInCategory.OST_WallFoundationAnalytical,
                (int) BuiltInCategory.OST_WallAnalytical,
                (int) BuiltInCategory.OST_AreaLoads,
                (int) BuiltInCategory.OST_AreaSchemes,
                (int) BuiltInCategory.OST_Areas,
                (int) BuiltInCategory.OST_Assemblies,
                (int) BuiltInCategory.OST_DetailComponents,
                (int) BuiltInCategory.OST_FilledRegion,
                (int) BuiltInCategory.OST_MaskingRegion,
                (int) BuiltInCategory.OST_Entourage,
                (int) BuiltInCategory.OST_Grids,
                (int) BuiltInCategory.OST_HVAC_Zones,
                (int) BuiltInCategory.OST_Levels,
                (int) BuiltInCategory.OST_Materials,
                (int) BuiltInCategory.OST_ModelText,
                (int) BuiltInCategory.OST_PointClouds,
                (int) BuiltInCategory.OST_PointLoads,
                (int) BuiltInCategory.OST_ProjectInformation,
                (int) BuiltInCategory.OST_SitePropertyLineSegment,
                (int) BuiltInCategory.OST_RasterImages,
                (int) BuiltInCategory.OST_RvtLinks,
                (int) BuiltInCategory.OST_Sheets,
                (int) BuiltInCategory.OST_Views,
            };

            return list;
        }
        internal static Dictionary<int, string> parameterMap(int n)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();

            int index = 0;

            dict.Add(0, "ID");
            foreach(string s in group01(n))
            {
                dict.Add(++index, s);
            }
            foreach(string s in group02().Select(x => x.Item1).ToList())
            {
                dict.Add(++index, s);
            }
            dict.Add(++index, "Family");
            dict.Add(++index, "Category");

            return dict;
        }
        internal static List<Tuple<string, string>> ProjectData(int n)
        {
            List<Tuple<string, string>> data = new List<Tuple<string, string>>();
            data.AddRange(group01(n).Select(x => Tuple.Create(x,"Instance")));
            data.AddRange(group02().Select(x => Tuple.Create(x.Item1, x.Item3)));
            return data;
        }
        internal static List<Tuple<string, string>> ProjectData()
        {
            List<Tuple<string, string>> data = new List<Tuple<string, string>>();
            data.AddRange(group02().Select(x => Tuple.Create(x.Item1, x.Item3)));
            return data;
        }
        internal static Dictionary<string, string> parameterTypeMap(int n)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            
            foreach (string s in group01(n))
            {
                dict.Add(s, "Instance");
            }
            foreach (Tuple<string, string> tuple in group02().Select(x => new Tuple<string, string>(x.Item1, x.Item3)).ToList())
            {
                dict.Add(tuple.Item1, tuple.Item2);
            }

            return dict;
        }
        internal static bool IsInsanceParameter(string name, Dictionary<string, string> parameterTypeMap)
        {
            return String.Equals("Instance", parameterTypeMap[name]);
        }
        internal static List<string> ProjectParameterNames(int n)
        {
            List<string> names = new List<string>();
            
            names.Add("ID");
            names.AddRange(group01(n));

            List<string> temp = group02().Select((x, index) => String.Format("{0} - {1}", (index + 1).ToString("00"), x.Item1)).ToList();

            names.AddRange(temp);
            names.Add("Family Type");
            names.Add("Category");

            return names;
        }
        internal static List<string> ProjectParameterNames()
        {
            List<string> names = new List<string>();
            
            List<string> temp = group02().Select((x, index) => String.Format("{0} - {1}", (index + 1).ToString("00"), x.Item1)).ToList();
            names.AddRange(temp);
            names.Add("Family Type");

            return names;
        }
        internal static List<string> group01(int n)
        {
            List<string> parameters = new List<string>();
            for (int i = 0; i < n; i++)
            {
                parameters.Add(String.Format("Budget Code_{0}", (i + 1).ToString("00")));
                parameters.Add(String.Format("Unit_{0}", (i + 1).ToString("00")));
                parameters.Add(String.Format("Factor_{0}", (i + 1).ToString("00")));
            }

            return parameters;
        }
        internal static List<Tuple<string, string, string>> group02()
        {
            List<Tuple<string, string, string>> parametersTooltipType = new List<Tuple<string, string, string>>()
            {
                new Tuple<string, string, string>("BoQ Chapter",
                "Please write the number of the first 2 levels of the BoQ, using this configuration, 01.01.",
                "Type"),
                new Tuple<string, string, string>("Service Description",
                "Very short description of the service, using Camel configuration, Brick19+WhitePaint",
                "Type"),
                new Tuple<string, string, string>("MEP Specific Characteristic",
                "For MEP, write its characteristic like the diameter",
                "Instance"),
                new Tuple<string, string, string>("Windows and Doors Host",
                "This field is filled automatically based on the windows and doors host.",
                "Instance"),
                new Tuple<string, string, string>("Modelling Tool",
                "This field is filled automatically based on the type of tool you used for modelling the element.",
                "Type"),
                new Tuple<string, string, string>("BoQ Codes, Units and Factors",
                "This field is filled automatically based on the information on the Budget Group.",
                "Instance"),
            };

            return parametersTooltipType;
        }
    }
}
