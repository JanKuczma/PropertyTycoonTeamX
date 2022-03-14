using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class BoardData_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Excels/BoardData.xlsx";
	private static readonly string exportPath = "Assets/Excels/BoardData.asset";
	private static readonly string[] sheetNames = { "Sheet1", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			BoardEntities data = (BoardEntities)AssetDatabase.LoadAssetAtPath (exportPath, typeof(BoardEntities));
			if (data == null) {
				data = ScriptableObject.CreateInstance<BoardEntities> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					BoardEntities.Sheet s = new BoardEntities.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						BoardEntities.Param p = new BoardEntities.Param ();
						
					cell = row.GetCell(0); p.Position = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.Space = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.Group = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.Buy = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.Cost = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.Rent = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.Single = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.Double = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.Triple = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.Quad = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.Hotel = (int)(cell == null ? 0 : cell.NumericCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
