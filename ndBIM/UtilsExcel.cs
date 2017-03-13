using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Data;
using System.Drawing;
using System.IO;

namespace ndBIM
{
    public static class UtilsExcel
    {
        public static void CreateExcel(List<string> names, List<List<string>> projectdata, string name)
        {
            CreateExcel(names, projectdata, 0, name);
        }
        public static void CreateExcel(List<string> names, List<List<string>> projectdata, int n, string name)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xlsx)|*.xlsx";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "Export Excel File To";

            DataSet data = CreateDataset(names, projectdata, name);

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                clsGeneral cls = new clsGeneral();
                cls.GenerateExcel2007(saveFileDialog.FileName, data, n);
            }
        }
        public static List<string []> ReadExcel()
        {
            List<string []> dataStrings = new List<string []>();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Execl files (*.xlsx)|*.xlsx";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = "Import Excel File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                clsGeneral cls = new clsGeneral();
                DataTable data = cls.ReadExcel2007(openFileDialog.FileName, true);
                
                foreach(DataRow row in data.Rows)
                {
                    dataStrings.Add(row.ItemArray.OfType<string>().ToArray());
                }
            }
            return dataStrings;
        }
        private static DataSet CreateDataset(List<string> names, List<List<string>> data, string name)
        {
            // Create two DataTable instances.
            DataTable table1 = new DataTable(name);
            DataRow row = null;
            int index = 0;

            foreach(string n in names)
            {
                table1.Columns.Add(n);
            }
            foreach(List<string> list in data)
            {
                row = table1.NewRow();
                index = 0;
                foreach(string s in list)
                {
                    row[index] = s;
                    index++;
                }
                table1.Rows.Add(row);
            }            

            // Create a DataSet and put both tables in it.
            DataSet set = new DataSet("Budget");
            set.Tables.Add(table1);

            return set;
        }
    }  
    public class clsGeneral
    {
        public DataTable ReadExcel2007(string p_strPath, bool hasHeader)
        {
            DataTable read = new DataTable();
            byte[] file = null;
            try
            {
                file = File.ReadAllBytes(p_strPath);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed opening file. File read-only or opened by user. Please, close the file before proceeding.");
            }
            using (MemoryStream ms = new MemoryStream(file))
            using (ExcelPackage package = new ExcelPackage(ms))
            {
                if (package.Workbook.Worksheets.Count == 0)
                {
                    string strError = "Your Excel file does not contain any work sheets";
                    System.Windows.Forms.MessageBox.Show(strError);
                }
                else
                {
                    int atLeastOne = 0;
                    foreach (ExcelWorksheet worksheet in package.Workbook.Worksheets)
                    {
                        if(worksheet.Name.Equals("Budget Preparation"))
                        {
                            atLeastOne++;
                            foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                            {
                                read.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                            }
                            var startRow = hasHeader ? 2 : 1;
                            for (int rowNum = startRow; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                            {
                                var wsRow = worksheet.Cells[rowNum, 1, rowNum, worksheet.Dimension.End.Column];
                                DataRow row = read.Rows.Add();
                                foreach (var cell in wsRow)
                                {
                                    row[cell.Start.Column - 1] = cell.Text;
                                }
                            }
                        }
                    }
                    if(atLeastOne == 0)
                    {
                        string strError = "No 'Budget Preparation' Worksheet found.";
                        System.Windows.Forms.MessageBox.Show(strError);
                    }
                }
            }

            return read;
        }
        public void GenerateExcel2007(string p_strPath, DataSet p_dsSrc, int n)
        {
            using (ExcelPackage objExcelPackage = new ExcelPackage())
            {
                foreach (DataTable dtSrc in p_dsSrc.Tables)
                {
                    //Create the worksheet    
                    ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add(dtSrc.TableName);
                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1    
                    objWorksheet.Cells["A1"].LoadFromDataTable(dtSrc, true);
                    objWorksheet.Cells.Style.Font.SetFromFont(new Font("Arial", 7));
                    objWorksheet.Cells.AutoFitColumns();
                    objWorksheet.Cells.Style.Locked = true;

                    if(n > 0)
                    {
                        objWorksheet.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        objWorksheet.Cells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        objWorksheet.Protection.IsProtected = true;
                    }

                    var start = objWorksheet.Dimension.Start;
                    var end = objWorksheet.Dimension.End;

                    //(Un)Lock the columns
                    for (int column = start.Column; column < end.Column; column++)
                    {
                        if(n > 0 && column > 1 && column < 3 * (n + 1) + 3)
                        {
                            ExcelColumn columns = objWorksheet.Column(column);
                            columns.Style.Locked = false;
                            columns.Style.Fill.PatternType = ExcelFillStyle.None;
                            columns.Width = 17.5;
                        }
                        else
                        {
                            ExcelColumn columns = objWorksheet.Column(column);
                            columns.Width = 25;
                        }
                    }
                    //Set the height of the rows
                    for (int row = start.Row; row <= end.Row; row++)
                    { // Row by row...
                        objWorksheet.Row(row).Height = (row == 1) ? 35 : 12.5;
                        /*
                        for (int col = start.Column; col <= end.Column; col++)
                        { // ... Cell by cell...
                            object cellValue = workSheet.Cells[row, col].Text; // This got me the actual value I needed.
                        }
                        */
                    }
                    //Format the header    
                    using (ExcelRange objRange = objWorksheet.Cells["A1:XFD1"])
                    {
                        objRange.Style.Font.Bold = true;
                        objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        objRange.Style.Fill.PatternType = ExcelFillStyle.None;
                        objRange.Style.Font.Color.SetColor(Color.Blue);
                        objRange.Style.WrapText = true;
                    }
                }
                try
                {
                    //Write it back to the client    
                    if (File.Exists(p_strPath))
                        File.Delete(p_strPath);

                    //Create excel file on physical disk    
                    FileStream objFileStrm = File.Create(p_strPath);
                    objFileStrm.Close();

                    //Write content to excel file    
                    File.WriteAllBytes(p_strPath, objExcelPackage.GetAsByteArray());
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
