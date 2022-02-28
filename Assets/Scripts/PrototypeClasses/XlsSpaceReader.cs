// using System;
// using System.Collections.Generic;
// using System.IO;
// using OfficeOpenXml;
//
// /// <summary>
// /// Reads and returns the different Space types from an xls sheet
// /// </summary>
// public class XlsSpaceReader
// {
//     public List<Space.Property> WritePropertySpaces()
//     {
//         var propertySpaces = new List<Space.Property>();
//         using (var package =
//                new ExcelPackage(
//                    new FileInfo("/Users/anthonygavriel/OneDrive/Year 2/Spring/Software Engineering/" +
//                                 "SE Assignment/Input/BoardDataFormatted.xlsx")))
//         {
//             var sheet = package.Workbook.Worksheets["Sheet1"];
//
//             const int propertySpacesBegin = 6;
//             const int propertySpacesEnd = 27;
//
//             for (int i = propertySpacesBegin; i < propertySpacesEnd + 1; i++)
//             {
//                 Space.Property property = new Space.Property(
//                     Int32.Parse(sheet.Cells["A" + i].ToText()),
//                     sheet.Cells["B" + i].ToText(),
//                     Int32.Parse(sheet.Cells["H" + i].ToText()),
//                     Int32.Parse(sheet.Cells["I" + i].ToText())
//                 );
//
//                 propertySpaces.Add(property);
//             }
//         }
//         return propertySpaces;
//     }
//
//     public List<Space.Utility> WriteUtilitySpaces()
//     {
//         var utilitySpaces = new List<Space.Utility>();
//         using (var package =
//                new ExcelPackage(
//                    new FileInfo("/Users/anthonygavriel/OneDrive/Year 2/Spring/Software Engineering/" +
//                                 "SE Assignment/Input/BoardDataFormatted.xlsx")))
//         {
//             var sheet = package.Workbook.Worksheets["Sheet1"];
//
//             const int utilitySpacesBegin = 53;
//             const int utilitySpacesEnd = 54;
//
//             for (int i = utilitySpacesBegin; i < utilitySpacesEnd + 1; i++)
//             {
//                 Space.Utility utility = new Space.Utility(
//                     Int32.Parse(sheet.Cells["A" + i].ToText()),
//                     sheet.Cells["B" + i].ToText(),
//                     Int32.Parse(sheet.Cells["H" + i].ToText())
//                 );
//
//                 utilitySpaces.Add(utility);
//             }
//         }
//         return utilitySpaces;
//     }
//
//     public List<Space.Station> WriteStationSpaces()
//     {
//         var stationSpaces = new List<Space.Station>();
//         using (var package =
//                new ExcelPackage(
//                    new FileInfo("/Users/anthonygavriel/OneDrive/Year 2/Spring/Software Engineering/" +
//                                 "SE Assignment/Input/BoardDataFormatted.xlsx")))
//         {
//             var sheet = package.Workbook.Worksheets["Sheet1"];
//
//             const int stationSpacesBegin = 47;
//             const int stationSpacesEnd = 50;
//
//             for (int i = stationSpacesBegin; i < stationSpacesEnd + 1; i++)
//             {
//                 Space.Station station = new Space.Station(
//                     Int32.Parse(sheet.Cells["A" + i].ToText()),
//                     sheet.Cells["B" + i].ToText(),
//                     Int32.Parse(sheet.Cells["H" + i].ToText())
//                 );
//                 stationSpaces.Add(station);
//             }
//         }
//         return stationSpaces;
//     }
//
//     public Space.Go WriteGoSpace()
//     {
//         var goSpace = new Space.Go();
//         using (var package =
//                new ExcelPackage(
//                    new FileInfo("/Users/anthonygavriel/OneDrive/Year 2/Spring/Software Engineering/" +
//                                 "SE Assignment/Input/BoardDataFormatted.xlsx")))
//         {
//             var sheet = package.Workbook.Worksheets["Sheet1"];
//             var pos = Int32.Parse(sheet.Cells["A38"].ToText());
//             var name = sheet.Cells["B38"].ToText();
//             var s = new Space.Go(pos, name);
//             goSpace = s;
//         }
//         return goSpace;
//     }
//
//     public Space.IncomeTax WriteIncomeTaxSpace()
//     {
//         var incomeTaxSpace = new Space.IncomeTax();
//         using (var package =
//                new ExcelPackage(
//                    new FileInfo("/Users/anthonygavriel/OneDrive/Year 2/Spring/Software Engineering/" +
//                                 "SE Assignment/Input/BoardDataFormatted.xlsx")))
//         {
//             var sheet = package.Workbook.Worksheets["Sheet1"];
//             var pos = Int32.Parse(sheet.Cells["A39"].ToText());
//             var name = sheet.Cells["B39"].ToText();
//             var s = new Space.IncomeTax(pos, name);
//             incomeTaxSpace = s;
//         }
//         return incomeTaxSpace;
//     }
//
//     public Space.FreeParking WriteFreeParkingSpace()
//     {
//         var freeParkingSpace = new Space.FreeParking();
//         using (var package =
//                new ExcelPackage(
//                    new FileInfo("/Users/anthonygavriel/OneDrive/Year 2/Spring/Software Engineering/" +
//                                 "SE Assignment/Input/BoardDataFormatted.xlsx")))
//         {
//             var sheet = package.Workbook.Worksheets["Sheet1"];
//             var pos = Int32.Parse(sheet.Cells["A40"].ToText());
//             var name = sheet.Cells["B40"].ToText();
//             var s = new Space.FreeParking(pos, name);
//             freeParkingSpace = s;
//         }
//         return freeParkingSpace;
//     }
//     
//     public Space.SuperTax WriteSuperTaxSpace()
//     {
//         var superTaxSpace = new Space.SuperTax();
//         using (var package =
//                new ExcelPackage(
//                    new FileInfo("/Users/anthonygavriel/OneDrive/Year 2/Spring/Software Engineering/" +
//                                 "SE Assignment/Input/BoardDataFormatted.xlsx")))
//         {
//             var sheet = package.Workbook.Worksheets["Sheet1"];
//             var pos = Int32.Parse(sheet.Cells["A41"].ToText());
//             var name = sheet.Cells["B41"].ToText();
//             var s = new Space.SuperTax(pos, name);
//             superTaxSpace = s;
//         }
//         return superTaxSpace;
//     }
//     
//     public Space.GoToJail WriteGoToJailSpace()
//     {
//         var goToJailSpace = new Space.GoToJail();
//         using (var package =
//                new ExcelPackage(
//                    new FileInfo("/Users/anthonygavriel/OneDrive/Year 2/Spring/Software Engineering/" +
//                                 "SE Assignment/Input/BoardDataFormatted.xlsx")))
//         {
//             var sheet = package.Workbook.Worksheets["Sheet1"];
//             var pos = Int32.Parse(sheet.Cells["A42"].ToText());
//             var name = sheet.Cells["B42"].ToText();
//             var s = new Space.GoToJail(pos, name);
//             goToJailSpace = s;
//         }
//         return goToJailSpace;
//     }
//
//     public Space.JustVisiting WriteJustVisitingSpace()
//     {
//         var justVisitingSpace = new Space.JustVisiting();
//         using (var package =
//                new ExcelPackage(
//                    new FileInfo("/Users/anthonygavriel/OneDrive/Year 2/Spring/Software Engineering/" +
//                                 "SE Assignment/Input/BoardDataFormatted.xlsx")))
//         {
//             var sheet = package.Workbook.Worksheets["Sheet1"];
//             var pos = Int32.Parse(sheet.Cells["A43"].ToText());
//             var name = sheet.Cells["B43"].ToText();
//             var s = new Space.JustVisiting(pos, name);
//             justVisitingSpace = s;
//         }
//         return justVisitingSpace;
//     }
//
//     public List<Space.OpportunityKnocks> WriteOpportunityKnocksSpace()
//     {
//         var oppKnocksSpace = new List<Space.OpportunityKnocks>();
//         
//         using (var package =
//                new ExcelPackage(
//                    new FileInfo("/Users/anthonygavriel/OneDrive/Year 2/Spring/Software Engineering/" +
//                                 "SE Assignment/Input/BoardDataFormatted.xlsx")))
//         {
//             var sheet = package.Workbook.Worksheets["Sheet1"];
//
//             const int oppKnocksSpacesBegin = 33;
//             const int oppKnocksSpacesEnd = 35;
//
//             for (int i = oppKnocksSpacesBegin; i < oppKnocksSpacesEnd + 1; i++)
//             {
//                 Space.OpportunityKnocks cardSpace = new Space.OpportunityKnocks(
//                     Int32.Parse(sheet.Cells["A" + i].ToText()),
//                     sheet.Cells["B" + i].ToText()
//                 );
//                 oppKnocksSpace.Add(cardSpace);
//             }
//         }
//         return oppKnocksSpace;
//     }
//     
//     public List<Space.PotLuck> WritePotLuckSpaces()
//     {
//         var potLuckSpace = new List<Space.PotLuck>();
//         
//         using (var package =
//                new ExcelPackage(
//                    new FileInfo("/Users/anthonygavriel/OneDrive/Year 2/Spring/Software Engineering/" +
//                                 "SE Assignment/Input/BoardDataFormatted.xlsx")))
//         {
//             var sheet = package.Workbook.Worksheets["Sheet1"];
//
//             const int potLuckSpacesBegin = 30;
//             const int potLuckSpacesEnd = 32;
//
//             for (int i = potLuckSpacesBegin; i < potLuckSpacesEnd + 1; i++)
//             {
//                 Space.PotLuck cardSpace = new Space.PotLuck(
//                     Int32.Parse(sheet.Cells["A" + i].ToText()),
//                     sheet.Cells["B" + i].ToText()
//                 );
//                 potLuckSpace.Add(cardSpace);
//             }
//         }
//         return potLuckSpace;
//     }
// }