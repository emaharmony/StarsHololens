using UnityEngine;
using System.Collections;
//using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]
public class User 
{
	public string face; //username;
	public string scale; //gameID;
	public string faceColor; //score;
	public string xPos; //currentLvl;
	public string yPos; //realTime;
	public string zPos; //gameTime;
	public string xRot; //code;
	public string yRot; //code;
	public string zRot; //code;

	public string code;

	public User()
	{
		this.face = "1";
		this.scale = "0.0";
		this.faceColor = "3";
		this.xPos = "0.0";
		this.yPos = "0.0";
		this.zPos = "0.0";
		this.xRot = "0.0";
		this.yRot = "0.0";
		this.zRot = "0.0";

		setUpCode();
	}

	void setUpCode()
	{
		code = this.face + this.scale + this.faceColor + this.xPos + this.yPos + this.zPos + this.xRot + this.yRot + this.zRot;
		code = code.GetHashCode().ToString();
	}

	public User(User user)
	{
		this.face = user.face.Trim();
		this.scale = user.scale.Trim();
		this.faceColor = user.faceColor.Trim();
		this.xPos = user.xPos.Trim();
		this.yPos = user.yPos.Trim();
		this.zPos = user.zPos.Trim();
		this.xRot = user.xRot.Trim();
		this.yRot = user.yRot.Trim();
		this.zRot = user.zRot.Trim();

		setUpCode();
	}
	
	public User(string face2, string scale2, string faceColor2, string xPos2, string yPos2, string zPos2, string xRot2, string yRot2, string zRot2)
	{
		face = face2.Trim();
		scale = scale2.Trim();
		faceColor = faceColor2.Trim();
		this.xPos = xPos2.Trim();
		this.yPos = yPos2.Trim();
		this.zPos = zPos2.Trim();
		this.xRot = xRot2.Trim();
		this.yRot = yRot2.Trim();
		this.zRot = zRot2.Trim();

		setUpCode();
	}
	
	override public string ToString()
	{
		string c = System.Environment.NewLine;
		
		return face +c+
			scale +c+
			faceColor +c+
			xPos +c+
			yPos +c+
			zPos +c+
			xRot +c+
			yRot +c+
			zRot +c+
			code;
	}
	
}

