using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardEntities : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public int Position;
		public string Space;
		public string Group;
		public string Buy;
		public int Cost;
		public int Rent;
		public int Single;
		public int Double;
		public int Triple;
		public int Quad;
		public int Hotel;
	}
}

