using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ndBIM
{
    static class Parameters
    {
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
