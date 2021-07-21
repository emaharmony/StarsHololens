using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using System.Collections.Generic;
//using System.Xml.Serialization;
using System.IO;
//using System;
//using System.Runtime.Serialization;
using System.Text;
//using System.Runtime.Serialization.Formatters.Binary;


public class SaveToFile
{

	public SaveToFile()
	{

	}

	private void createDir()
	{
		if (Directory.Exists("user_pref")) 
		{
			return;
		}
		Directory.CreateDirectory("user_pref");
	}

	private void createDir(string dir)
	{
		if (Directory.Exists(dir)) 
		{
			return;
		}
		Directory.CreateDirectory(dir);
	}
	/*
	public void removeDir(string dir)
	{
		if (Directory.Exists(dir)) 
		{
			Debug.Log(dir);
			User[] ids = LoadAllGames(dir);
			foreach(User s in ids)
			{
				//Debug.Log("gameid: "+s.gameID);
				deleteFile(dir,"MannyQuinn_save.ews2");
			}
			Directory.Delete(dir);
		}
	}*/


	public string SaveAFile(string directory,string fileName,string content)
	{
		if(((fileName = fileName.Trim()) == "") || (fileName.Contains("\\")) || (fileName.Contains("/")) || (fileName.Contains("<")) || (fileName.Contains(">")))
		{
			return "Failed";
		}

		if(((directory = directory.Trim()) == "") || (directory.Contains("<")) || (directory.Contains(">")))
		{
			return "Failed";
		}

		createDir(directory);

		File.WriteAllText(directory+"\\"+fileName.ToString(), content);

		return "Passed";
	}


	public string[] LoadAFile(string fileName)
	{
		if (File.Exists(fileName.ToString()))
		{
			string s = File.ReadAllText(fileName.ToString());

			string[] tokens = s.Split('\n');


			return tokens;

		}
		return (new string[]{"file not found"});
	}


	public string[] LoadAFile(string directory,string fileName)
	{
		if (File.Exists(directory+"\\"+fileName.ToString()))
		{
			string s = File.ReadAllText(directory+"\\"+fileName.ToString());

			string[] tokens = s.Split('\n');

			return tokens;
		}
		return (new string[]{"file not found"});
	}

	public bool CheckFileExists(string directory,string fileName)
	{
		if (File.Exists(directory+"\\"+fileName.ToString()))
		{
			return true;
		}
		return false;
	}

	public bool CheckFileExists(string fileName)
	{
		if (File.Exists(fileName))
		{
			return true;
		}
		return false;
	}


//////////////////////////////////////////////// Plain Text SAVE/LOAD FILE //////////////////////////////////////////////////////////////

	///<summary> saves game as pure text </summary>
	public string SaveGame(string face2, string scale2, string faceColor2, string xPos2, string yPos2, string zPos2, string xRot2, string yRot2, string zRot2)
	{
		createDir();
		User myUser = new User(face2,  scale2, faceColor2, xPos2, yPos2, zPos2, xRot2, yRot2, zRot2);
	
		System.IO.File.WriteAllText("user_pref\\MannyQuinn_save.txt", myUser.ToString());

		return "Passed";
	}

	public string SaveGame(User obj)
	{
		return SaveGame(obj.face,obj.scale,obj.faceColor,obj.xPos, obj.yPos, obj.zPos, obj.xRot, obj.yRot, obj.zRot);
	}

	///<summary>load game from plain text</summary>
	public string[] LoadGame_text(string gameID)
	{
		if (File.Exists("user_pref\\MannyQuinn_save.txt"))
		{
			string s = System.IO.File.ReadAllText("user_pref\\MannyQuinn_save.txt");

			string[] tokens = s.Split('\n');

			User myUser = new User(tokens[0],tokens[1],tokens[2],tokens[3],tokens[4],tokens[5],tokens[6],tokens[7],tokens[8]);

			if(tokens[tokens.Length-1].Trim() == (myUser.code))
				return tokens;

			return null;
		}
		return (new string[]{"N/A","0000","0","0:00:0","-1","-1"});
	}

	public User LoadGame(string gameID)
	{
		if (File.Exists("user_pref\\MannyQuinn_save.txt"))
		{
			string s = System.IO.File.ReadAllText("user_pref\\MannyQuinn_save.txt");

			string[] tokens = s.Split('\n');

			User myUser = new User(tokens[0],tokens[1],tokens[2],tokens[3],tokens[4],tokens[5],tokens[6],tokens[7],tokens[8]);

			if(tokens[tokens.Length-1].Trim() == (myUser.code))
				return myUser;

			return null;
		}
		return null;
	}


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////// XML SAVE/LOAD FILE //////////////////////////////////////////////////////////////

	/*///<summary> saves game in XML format </summary>
	public string SaveGame_xml(string username, string gameID,string score, string currentLvl,string time)
	{
		if(((username = username.Trim()) == "") || (username.Contains("\\")) || (username.Contains("/")) || (username.Contains("<")) || (username.Contains(">"))  )
		{
			return "Failed";
		}

		if(((gameID = gameID.Trim()) == "") || (gameID.Contains("\\")) || (gameID.Contains("/")) || (gameID.Contains("<")) || (gameID.Contains(">")))
		{
			return "Failed";
		}

		createDir();
		User myUser = new User(username,gameID, score, currentLvl,time);

		FileStream FS = new FileStream("Saved Games\\"+gameID+".xml", FileMode.OpenOrCreate, FileAccess.ReadWrite);

		XmlSerializer xmlSerial = new XmlSerializer(myUser.GetType());

		xmlSerial.Serialize(FS,myUser);

		//Console.WriteLine();
		//Console.ReadLine();
		FS.Close();

		return "Passed";
	}

	public string SaveGame_xml(User obj)
	{
		return SaveGame_xml(obj.username,obj.gameID,obj.score, obj.currentLvl,obj.gameTime);
	}


	///load game from XML format
	public object LoadGame_xml(string gameID)
	{

		if (File.Exists("Saved Games\\"+gameID.ToString()+".xml"))
		{
//			Debug.Log("1");
			FileStream FS = new FileStream("Saved Games\\"+gameID.ToString()+".xml", FileMode.Open, FileAccess.Read);
			//Debug.Log("2");
			XmlSerializer xml = new XmlSerializer(typeof(User));
			//Debug.Log("3");
			User o = xml.Deserialize(FS) as User;
			//Debug.Log("4");

			FS.Close();
			//Debug.Log("5");
			if(o.code == new User(o.username,o.gameID,o.score,o.currentLvl,o.gameTime).code)
			{
				return o;
			}
			//Debug.LogError("File has been tampered with!");
			return null;
		}

		return new User();
	}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


/////////////////////////////////////////// EWS BYTE ARRAY SAVE/LOAD FILE //////////////////////////////////////////////////////////////////////////



	public string SaveGame_bytes(User obj)
	{
		return SaveGame_bytes(obj.username,obj.gameID, obj.score, obj.currentLvl,obj.gameTime);
	}

	//<summary> soon to be implemented. saves game as machine code </summary>
	public string SaveGame_bytes(string username, string gameID,string score, string currentLvl,string time)
	{
		if(((username = username.Trim()) == "") || (username.Contains("\\")) || (username.Contains("/")) || (username.Contains("<")) || (username.Contains(">")))
		{
			return "Failed";
		}

		if(((gameID = gameID.Trim()) == "") || (gameID.Contains("\\")) || (gameID.Contains("/")) || (gameID.Contains("<")) || (gameID.Contains(">")))
		{
			return "Failed";
		}

		createDir();
		User myUser = new User(username,gameID, score, currentLvl,time);


		byte[] str = Encoding.ASCII.GetBytes( myUser.ToString());

		for(int i = 0;i<str.Length;i++)
		{
			str[i] += 127;
		}


		System.IO.File.WriteAllBytes("Saved Games\\"+gameID.ToString()+".ews",str);

		return "Passed";
	}


	///<summary>load game from byte array</summary>
	public string[] LoadGame_bytes(string gameID)
	{
		if (File.Exists("Saved Games\\"+gameID.ToString()+".ews"))
		{

			byte[] str =System.IO.File.ReadAllBytes("Saved Games\\"+gameID.ToString()+".ews");


			for(int i = 0;i<str.Length;i++)
			{
				str[i] -= 127;
			}

			string s = Encoding.ASCII.GetString(str);

			string[] tokens = s.Split('\n');

			User myUser = new User(tokens[0],tokens[1],tokens[2],tokens[3],tokens[4]);


			if(tokens[tokens.Length-1].Trim() == (myUser.code))
				return tokens;
			
			return null;
		}

		return (new string[]{"N/A","0000","0","0:00:0","-1","-1"});
	}

	public User LoadGame_bytes_obj(string gameID)
	{
		if (File.Exists("Saved Games\\"+gameID.ToString()+".ews"))
		{

			byte[] str =System.IO.File.ReadAllBytes("Saved Games\\"+gameID.ToString()+".ews");


			for(int i = 0;i<str.Length;i++)
			{
				str[i] += 15;
			}

			string s = Encoding.ASCII.GetString(str);

			string[] tokens = s.Split('\n');

			User myUser = new User(tokens[0],tokens[1],tokens[2],tokens[3],tokens[4]);


			if(tokens[tokens.Length-1].Trim() == (myUser.code))
				return myUser;

			return null;
		}

		return (new User("N/A","-1","0000","0","0:00:0"));
	}
	*/
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/////////////////////////////////ENCRYPTED OBJECT///////////////////////////////////////////////////////////////////////////////////
/*	public string SaveGame_obj(User user,string directory)
	{

		createDir(directory);
		User myUser = new User(user);
		//SerializeNow(myUser);

		byte[] str = ObjectToByteArray(myUser);

		if(str == null || str.Length == 0)
			return "Failed";
		
		for(int i = 0;i<str.Length;i++)
		{
			str[i] += 100;
		}

		File.WriteAllBytes("LeaderBoard\\MannyQuinn_save.ews2",str);
		return "Passed";
	}


	public void SerializeNow(User user)
	{
		User c = user;
		//File f = new File("temp.dat");
		Stream s = File.Open("MannyQuinn_save.ews2",FileMode.Create);
		System.Runtime.Serialization.Formatters.Binary.BinaryFormatter b = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		b.Serialize(s,c);
		s.Close();
	}

	private byte[] ObjectToByteArray(object obj)
	{
		if(obj == null)
			return null;

		BinaryFormatter bf = new BinaryFormatter();
		using (MemoryStream ms = new MemoryStream())
		{
			bf.Serialize(ms, obj);
			return ms.ToArray();
		}
	}

	private object ByteArrayToObject(byte[] arrBytes)
	{

		MemoryStream memStream = new MemoryStream();
		BinaryFormatter binForm = new BinaryFormatter();
		memStream.Write(arrBytes, 0, arrBytes.Length);
		memStream.Seek(0, SeekOrigin.Begin);
		object obj = (object) binForm.Deserialize(memStream);
		return obj;
	}

	public object LoadGame_obj(string fileName)
	{
		if (File.Exists("Saved Games\\"+fileName.ToString()+".ews2"))
		{
			byte[] str = System.IO.File.ReadAllBytes("Saved Games\\"+fileName.ToString()+".ews2");

			for(int i = 0;i<str.Length;i++)
			{
				str[i] -= 100;
			}

			User user = (User)ByteArrayToObject(str);
			User myUser = new User(user);

			if(user.code == (myUser.code))
				return user;

			return null;
		}

		return (new User());
	}

	public string[] LoadGame_obj_str(string fileName)
	{
		if (File.Exists("Saved Games\\"+fileName.ToString()+".ews2"))
		{

			byte[] str = File.ReadAllBytes("Saved Games\\"+fileName.ToString()+".ews2");


			for(int i = 0;i<str.Length;i++)
			{
				str[i] -= 100;
			}

			User user = (User)ByteArrayToObject(str);
			User myUser = new User(user);


			if(user.code == (myUser.code))
			{
				string[] tokens = {user.face, user.scale, user.faceColor, user.xPos, user.yPos, user.zPos, user.xRot, user.yRot, user.zRot, user.code };
				return tokens;
			}

			return null;
		}
		return (new string[]{"0","0","0","0.0","0.0","0.0","0.0","0.0","0.0","0.0"});
	}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public User[] LoadAllGames()
	{
		string[] files = Directory.GetFiles("Saved Games\\");
		User[] myUser = new User[files.Length];
		int i = 0;
		//Debug.Log("file length = "+files.Length);
		foreach(string s in files)
		{
			if(s.Contains(".ews2"))
			{
				myUser[i++] = (User)LoadGame_obj_helper(s);
			}
		}

		User[] finalUsers = new User[i];

		i=0;
		foreach(User s in myUser)
		{
			if(i<finalUsers.Length)
				finalUsers[i++] = s;
			else
				break;

		}

		return finalUsers;
	}

	public User[] LoadAllGames(string dir)
	{
		string[] files = Directory.GetFiles(dir+"\\");
		User[] myUser = new User[files.Length];
		int i = 0;

		foreach(string s in files)
		{
			if(s.Contains(".ews2"))
			{
				myUser[i++] = (User)LoadGame_obj_helper(s);
			}
		}

		User[] finalUsers = new User[i];

		i=0;
		foreach(User s in myUser)
		{
			if(i<finalUsers.Length)
				finalUsers[i++] = s;
			else
				break;
		}
			
		return finalUsers;
	}

	private object LoadGame_obj_helper(string gameID)
	{
		if (File.Exists(gameID.ToString()))
		{

			byte[] str = File.ReadAllBytes(gameID.ToString());


			for(int i = 0;i<str.Length;i++)
			{
				str[i] -= 100;
			}


			User user = (User)ByteArrayToObject(str);

			User myUser = new User(user);


			if(user.code == (myUser.code))
				return user;

			return null;
		}
		else
		{
			Debug.Log("failed to open file: "+gameID);
		}

		return (new User());
	}
	*/

	public void deleteFile(string gameID)
	{
		File.Delete(gameID);
	}
	public void deleteFile(string directory,string fileName)
	{
		File.Delete(directory+"\\"+fileName);
	}

}

