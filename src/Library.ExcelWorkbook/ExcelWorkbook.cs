using System;
using System.IO;
using Library.System;
using Microsoft.Office.Interop.Excel;

namespace Library.ExcelWorkBook
{
    public class ExcelWorkbook
    {
        private readonly bool _displayAlerts;
        private readonly bool _visible;

        public _Workbook Workbook;
        public _Worksheet Worksheet;
        public _Application App;

        public ExcelWorkbook(bool displayAlerts = false, bool visible = false)
        {
            _displayAlerts = displayAlerts;
            _visible = visible;
        }

        public void Open(string filePath, string nameTab, bool closeExcel = false) => OpenExcel(filePath, 1, nameTab, closeExcel);
        public void Open(string filePath, int indexTab, bool closeExcel = false) => OpenExcel(filePath, indexTab, null, closeExcel);

        private void OpenExcel(string filePath, int indexTab, string nameTab = null, bool closeExcel = false)
        {
            Print.Message("[Library.Workbook] - Abrindo planilha");

            if (closeExcel)
                SystemTool.Taskkill("EXCEL.*");

            if (!File.Exists(filePath))
            {
                string error = $"[Library.Workbook] Erro ao iniciar planilha! Arquivo \"{filePath}\" não existe";
                Print.Error(error);
                throw new Exception(error);
            }

            try
            {
                App = new Application
                {
                    DisplayAlerts = _displayAlerts,
                    Visible = _visible
                };

                Workbook = App.Workbooks.Open(filePath);
                Worksheet = null;

                if (nameTab != null)
                    Worksheet = (Worksheet)Workbook.Worksheets[nameTab];                
                else
                    Worksheet = (Worksheet)Workbook.Worksheets[indexTab];                  
            }
            catch (Exception x)
            {
                Print.Error("[Library.Workbook] Erro ao iniciar planilha!");
                throw new Exception(x.ToString());
            }
        }

        public void New(string tabName)
        {
            try
            {
                App = new Application
                {
                    DisplayAlerts = _displayAlerts,
                    Visible = _visible
                };
                Workbook = App.Workbooks.Add(Type.Missing);

                var sheets2 = Workbook.Sheets;
                var newSheet = (Worksheet)sheets2.Add(sheets2[1], Type.Missing, Type.Missing, Type.Missing);
                newSheet.Name = tabName;

                Worksheet = (Worksheet)Workbook.Worksheets[tabName];
            }
            catch (Exception x)
            {
                Print.Error("[Library.Workbook] Erro ao criar planilha!");
                throw new Exception(x.ToString());
            }
        }

        public void AddColumn(string column, string value)
        {
            Worksheet.Range[column].EntireColumn.Insert(XlInsertShiftDirection.xlShiftToRight);
            Worksheet.Range[column].Value = value;
        }
        public void AddColumn(int column, string value)
        {
            Worksheet.Range[column].EntireColumn.Insert(XlInsertShiftDirection.xlShiftToRight);
            Worksheet.Range[column].Value = value;
        }

        public void WorksheetReference(string tabName)
        {
            Worksheet = (Worksheet)Workbook.Worksheets[tabName];
        }
        public void WorksheetReference(int tabInt)
        {
            Worksheet = (Worksheet)Workbook.Worksheets[tabInt];
        }

        public FileInfo SaveAs(string file, bool createPath = false)
        {
            if (createPath)
            {
                if (!Directory.Exists(file))  
                    Directory.CreateDirectory(file);
                else
                    {
                        string error =
                            "[Library.WorkBook] Erro ao salvar planilha! Diretório informado não existe, crie ou ative o parametro \"createPath\" deste metódo para criação automática";
                        Print.Error(error);
                        throw new Exception(error);
                    }
                }
            try
            {
                Workbook.SaveAs(file, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Workbook.Close();
                App.Quit();
                FileInfo relatorio = new FileInfo(file + ".xlsm");
                Print.Info($"Relatório Ks salvo \"{file}\" ");
                return relatorio;
            }
            catch (Exception x)
            {
                Print.Error("> [library.Workbook] Erro ao salvar planilha \n" + x);
                return null;
            }
        }

        public void Save()
        {
            try
            {
                Workbook.Save();                                
            }
            catch (Exception x)
            {
                Print.Error("> [library.Workbook] Erro ao salvar planilha!\n" + x);
                throw x;
            }
        }

        public void RemoveDuplicates(int column) =>        
            Worksheet.UsedRange.RemoveDuplicates(new object[] { column }, XlYesNoGuess.xlYes);            
    }
}
