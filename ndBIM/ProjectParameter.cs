using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace ndBIM
{
    class ProjectParameter
    {
        private string name;
        private string type;
        private string value;
        private string tooltip;
        private string groupName;

        public ProjectParameter(string name, string group)
        {
            this.name = name;
            this.tooltip = "";
            this.type = "instance";
            this.groupName = group;
        }

        public ProjectParameter(string name, string tooltip, string type, string group) 
        {
            this.name = name;
            this.tooltip = tooltip;
            this.type = type;
            this.groupName = group;
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public string Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        public string Tooltip
        {
            get
            {
                return tooltip;
            }

            set
            {
                tooltip = value;
            }
        }

        public string GroupName {
            get
            {
                return groupName;
            }
            internal set
            {
                groupName = value;
            }
        }
    }
}
