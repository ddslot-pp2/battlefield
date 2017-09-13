using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;

public class XmlUse : MonoBehaviour {
	

	TextAsset textAsset;
	
	XmlDocument XmlData;
	XmlNodeList hp_table;
	XmlNodeList speed_table;
	XmlNodeList bullet_speed_table;
	XmlNodeList bullet_power_table;
	XmlNodeList bullet_distance_table;
	XmlNodeList bullet_reload_table;


	void Start()
	{
		ReadXMLResource();
	}
	
	
	void ReadXMLResource()
	{
		
	
		textAsset = Resources.Load("character_stat" , typeof(TextAsset)) as TextAsset; 

		if (textAsset == null) 
		{
			Debug.Log ("character_stat xml null");
			return;

		}

		byte[] byteArray = Encoding.UTF8.GetBytes(textAsset.text); 

		Debug.Log ("textAsset.text" + textAsset.text);

		
 		MemoryStream ms = new MemoryStream(byteArray); 
		
		XmlData = new XmlDocument();
	
		XmlData.Load(ms);
		
		XmlElement RootNode = XmlData.DocumentElement; 
	
		hp_table = XmlData.GetElementsByTagName ("max_hp");
		speed_table = XmlData.GetElementsByTagName ("speed");
		bullet_speed_table = XmlData.GetElementsByTagName ("bullet_speed");
		bullet_power_table = XmlData.GetElementsByTagName ("bullet_power");
		bullet_distance_table = XmlData.GetElementsByTagName ("bullet_distance");
		bullet_reload_table = XmlData.GetElementsByTagName ("reload_time");


	}
	
	public int GetHpValue( int tankType, int level)
	{
		return int.Parse(hp_table [tankType * 10 + level].InnerText);
	}

	public int GetSpeedValue( int tankType, int level)
	{
		return int.Parse(speed_table [tankType * 10 + level -1].InnerText);
	}

	public int GetBulletSpeedValue( int tankType, int level)
	{
		return int.Parse(bullet_speed_table [tankType * 10 + level -1].InnerText);
	}

	public int GetBulletPowerValue( int tankType, int level)
	{
		return int.Parse(bullet_power_table [tankType * 10 + level -1].InnerText);
	}

	public int GetBulletDistanceValue( int tankType, int level)
	{
		return int.Parse(bullet_distance_table [tankType * 10 + level -1].InnerText);
	}

	public int GetBulletReloadValue( int tankType, int level)
	{
		return int.Parse(bullet_reload_table [tankType * 10 + level -1].InnerText);
	}
	
}