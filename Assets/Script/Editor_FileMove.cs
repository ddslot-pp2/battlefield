
#if UNITY_EDITOR

using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Collections.Generic;

public class Editor_FileMove : ScriptableObject {	

	[MenuItem("CandyLib/Make Game Data/Collision Make")]


	public static void CollisionMake ()
	{

		string szFath = "Assets/Resources/Collision.txt";
		if (File.Exists(szFath))
			File.Delete(szFath);
		StreamWriter kWriter = new StreamWriter(szFath);

		Debug.Log ("CollisionMake");
	
		Object[] selectObjects = Selection.objects;

		foreach (Object selectObject in selectObjects) 
		{
			GameObject kObject = selectObject as GameObject;

			BoxCollider[] boxcolliders = kObject.GetComponentsInChildren<BoxCollider>();


			foreach (BoxCollider boxcollider in boxcolliders)
			{
				string szstring = "";
				//szstring += "pos: ";
				//szstring += boxcollider.center.x;
				//szstring += ",";
				//szstring += boxcollider.center.z;

                szstring += boxcollider.center.x;
				kWriter.WriteLine(szstring);

                szstring = "";
                szstring += boxcollider.center.z;
                kWriter.WriteLine(szstring);

                szstring = "";
                szstring += boxcollider.size.x;
                kWriter.WriteLine(szstring);

                szstring = "";
                szstring += boxcollider.size.z;
                kWriter.WriteLine(szstring);

				//szstring = "scale";
                /*
				szstring = boxcollider.size.x;
				szstring += ",";
				szstring += boxcollider.size.z;
				kWriter.WriteLine(szstring);
                */            



			}
				
		}

        kWriter.Close();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
	
	}
}
#endif