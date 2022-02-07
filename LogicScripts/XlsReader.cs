using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

/// <summary>
/// Reads names and descriptions for each card off a spreadsheet and outputs a deck of cards
/// </summary>
public class XlsReader
    {
        public XlsReader() { }

        // Writes the stack
        public CardStack WriteStack(CardStack c, int cell1, int cell2)
        {
            using (var package =
                   new ExcelPackage(
                       new FileInfo("/Users/anthonygavriel/RiderProjects/FeatureAlpha/PropertyTycoonCardData.xlsx")))
            {
                var sheet = package.Workbook.Worksheets["Default"];

                var dict = ExtractCardDetails(sheet, cell1, cell2);


                foreach (var pl in dict)
                {
                    var card = new Card(pl.Key, pl.Value);
                    c.AddCard(card);
                }
            }
            return c;
        }
        
        private static Dictionary<string, string> ExtractCardDetails(ExcelWorksheet sheet, int cell1, int cell2)
        {
            var cardStack = new Dictionary<string, string>();

            for (var i = cell1; i < cell2 + 1; i++)
            {
                var rs1 = "";
                var rs2 = "";
                //fix formatting 
                if (sheet.Cells["A" + i].ToText().Contains("?"))
                {
                    rs1 = sheet.Cells["A" + i].ToText().Replace('?', '£');
                }

                if (sheet.Cells["B" + i].ToText().Contains("?"))
                {
                    rs2 = sheet.Cells["B" + i].ToText().Replace('?', '£');
                }

                //check if formatted cells need to be added to dictionary
                if (string.IsNullOrEmpty(rs1) && string.IsNullOrEmpty(rs2))
                {
                    cardStack.Add(sheet.Cells["A" + i].ToText(), sheet.Cells["B" + i].ToText());
                }
                else if (string.IsNullOrEmpty(rs1) && !string.IsNullOrEmpty(rs2))
                {
                    cardStack.Add(sheet.Cells["A" + i].ToText(), rs2);
                }
                else if (!string.IsNullOrEmpty(rs1) && string.IsNullOrEmpty(rs2))
                {
                    cardStack.Add(rs1, sheet.Cells["B" + i].ToText());
                }
                else
                {
                    cardStack.Add(rs1, rs2);
                }

                rs1 = null;
                rs2 = null;
            }
            return cardStack;
        }
    }
