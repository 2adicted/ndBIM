#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Media.Imaging;
#endregion

namespace ndBIM
{
    class App : IExternalApplication
    {
        // define a method that will create our tab and button
        static void AddRibbonPanel(UIControlledApplication application)
        {
            // Create a custom ribbon tab
            String tabName = "ndBIM";
            application.CreateRibbonTab(tabName);

            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "Budget");

            // Get dll assembly path
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // create push button for Budget Parameters
            PushButtonData b1Data = new PushButtonData(
                "Budget Parameters",
                "Budget Parameters",
                thisAssemblyPath,
                "ndBIM.cmdBudgetParameters");

            PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;
            pb1.ToolTip = "Creates and populates current project with predefined Shared Parameters.";
            BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/ndBIM;component/Resources/favicon.png"));
            pb1.LargeImage = pb1Image;
            // create push button for Tools and Hosts
            PushButtonData b2Data = new PushButtonData(
                "Tools & Hosts",
                "Tools & Hosts",
                thisAssemblyPath,
                "ndBIM.cmdToolsAndHosts");

            PushButton pb2 = ribbonPanel.AddItem(b2Data) as PushButton;
            pb2.ToolTip = "Automatically populate Budget parameters. Only use after using Budget Parameters first.";
            BitmapImage pb2Image = new BitmapImage(new Uri("pack://application:,,,/ndBIM;component/Resources/angry.png"));
            pb2.LargeImage = pb2Image;
        }

        public Result OnStartup(UIControlledApplication a)
        {
            AddRibbonPanel(a);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
