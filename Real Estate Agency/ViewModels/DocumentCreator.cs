using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using REA.Models;

namespace REA.ViewModels
{
    static class DocumentCreator
    {
        public static void CreateWordDoc(string document, Deal deal)
        {
            File.Copy(@"..\..\Templates\Contract.docx", document, true);
            Employee employee = App.UnitOfWork.GenericRepository<Employee>().Get(CurrentState.UserEmail);
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                Regex[] regexText = new Regex[] {
                    new Regex("date"),
                    new Regex("DealId"),
                    new Regex("ClientName"),
                    new Regex("ClientSurname"),
                    new Regex("ClientPatronymic"),
                    new Regex("OwnerName"),
                    new Regex("OwnerSurname"),
                    new Regex("OwnerPatronymic"),
                    new Regex("EmployeeName"),
                    new Regex("EmployeeSurname"),
                    new Regex("EmployeePatronymic"),
                    new Regex("RealtyPrice"),
                    new Regex("RealtyCurrency"),
                    new Regex("RealtyType"),
                    new Regex("RealtyName"),
                    new Regex("RealtyAddress"),
                    new Regex("RealtySquare"),
                    new Regex("TypeOfDealPerCent"),
                    new Regex("DealTypeOfDeal"),
                    new Regex("ClientAddress"),
                    new Regex("ClientPassport"),
                    new Regex("ClientBirthday"),
                    new Regex("ClientPhone"),
                    new Regex("OwnerPassport"),
                    new Regex("OwnerBirthday"),
                    new Regex("OwnerPhone")
                };

                string[] replaceText = {
                    deal.Date.Date.ToShortDateString(),
                    deal.Id.ToString(),
                    deal.Client.Name,
                    deal.Client.Surname,
                    deal.Client.Patronymic,
                    deal.Realty.Owner.Name,
                    deal.Realty.Owner.Surname,
                    deal.Realty.Owner.Patronymic,
                    employee.Name,
                    employee.Surname,
                    employee.Patronymic,
                    deal.Realty.Price.ToString(),
                    deal.Realty.Currency,
                    deal.Realty.Type.Name,
                    deal.Realty.Name,
                    deal.Realty.Address,
                    deal.Realty.Square.ToString(),
                    deal.GetCommission().ToString(),
                    deal.Type.Name,
                    deal.Client.Address??new string(' ', 20),
                    deal.Client.Passport??new string(' ', 20),
                    deal.Client.Birthday.HasValue?deal.Client.Birthday.Value.ToShortDateString():new string(' ', 20),
                    deal.Client.Phone??new string(' ', 20),
                    deal.Realty.Owner.Passport??new string(' ', 20),
                    deal.Realty.Owner.Birthday.HasValue?deal.Realty.Owner.Birthday.Value.ToShortDateString():new string(' ', 20),
                    deal.Realty.Owner.Phone??new string(' ', 20)
                };

                for (int i = 0; i < regexText.Count(); i++) 
                {
                    docText = regexText[i].Replace(docText, replaceText[i]);
                }

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }

                ProcessStartInfo startInfo = new ProcessStartInfo { FileName = document };
                Process.Start(startInfo);
            }
        }

        public static void CreateExcelDoc<T>(string fileName, List<T> items) where T:class
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name =  typeof(T).Name};
                sheets.Append(sheet);
                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());
                Row row = new Row();

                List<PropertyInfo> properties = typeof(T).GetProperties().Where(t => t.PropertyType == typeof(int) || t.PropertyType == typeof(string) || t.PropertyType == typeof(DateTime) ||
                                                                                     t.PropertyType == typeof(DateTime?) || t.PropertyType == typeof(decimal)).ToList();

                string[] headers = properties.Where(x => x.Name != "Item" && x.Name != "Error").Select(x => x.Name).ToArray();

                Cell[] cells = new Cell[headers.Count()];

                for (int i = 0; i < headers.Count(); i++) 
                {
                    cells[i] = ConstructCell(headers[i], CellValues.String);
                }

                row.Append(cells);
                sheetData.AppendChild(row);


                CellValues type = new CellValues();
                foreach(var item in items)
                {
                    row = new Row();
                    for (int i = 0; i < headers.Count(); i++) 
                    {
                        if(properties[i].PropertyType == typeof(int))
                        {
                            type = CellValues.Number;
                        }
                        else if(properties[i].PropertyType == typeof(DateTime))
                        {
                            type = CellValues.Date;
                        }
                        else
                        {
                            type = CellValues.String;
                        }
                        cells[i] = ConstructCell(properties[i].GetValue(item)??"", type);
                    }
                    row.Append(cells);
                    sheetData.AppendChild(row);
                }

                worksheetPart.Worksheet.Save();

                ProcessStartInfo startInfo = new ProcessStartInfo { FileName = fileName };
                Process.Start(startInfo);
            }
        }

        private static Cell ConstructCell(object value, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(value.ToString()),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }
    }
}
