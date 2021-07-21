using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Monnit;
using Monnit.Mine;
using UnityEngine;

public class MonnitTest : MonoBehaviour {
	
	/*public static MonnitTest Instance{ get; private set;}

    [SerializeField] double startingAngle = 93, startingRoll = -2;
    [SerializeField] Transform testDummy;

	MineServer _server;
	Gateway _gateway;
	Sensor _sensor;
    List<double> data = new List<double>();

    bool _update = false, _init = false;
    float timer = 0;
    Vector3 targetRotation;

    void Awake()
	{
		Instance = this;
	}

    private void Update()
    {
        if (_update && data.Count > 0)
        {
            if (testDummy != null)
            {
                targetRotation = new Vector3(targetRotation.x, transform.eulerAngles.y/*targetRotation.y*, transform.eulerAngles.z);
                testDummy.transform.eulerAngles = Vector3.Lerp(testDummy.transform.eulerAngles, targetRotation, timer);
            }

            timer += Time.deltaTime;

            if (timer >= 1)
                TurnOffUpdate();
        }
    }
    // Use this for initialization
    public void StartMonnit () 
	{
        //Server studd
        _server = new MineServer(BaseApplication.eMineListenerProtocol.TCPAndUDP, IPAddress.Parse("10.0.1.17"), 3000);
        _server.StartServer();

        _server.SensorMessage += Handle_SensorMessage;		//messages from sensors
        _server.LogException += Handle_LogException;
        _server.PersistGateway += Handle_PersistGateway;
        _server.PersistSensor += Handle_PersistSensor;
        _server.GatewayMessage += Handle_GatewayMessage;

        _gateway = new Gateway (938538, BaseApplication.eGatewayType.Ethernet_3_0, "3.3.1.5", "2.5.2.0", "10.0.1.17", 3000);
        _gateway.UpdateReportInterval(1);
		_server.RegisterGateway (_gateway);
			
		_sensor = new Sensor (406617, BaseApplication.eSensorApplication.Tilt, "2.5.5.3", "GEN1");
        _sensor.UpdateReportInterval(.017);
        _sensor.UpdateLinkInterval(1);
        _sensor.UpdateRecoveryCount(0);
        _sensor.UpdateRetryCount(0);
        _server.RegisterSensor (938538, _sensor);

		//Login : String.Format("https://www.imonnit.com/xml/Logon/{0}", AuthToken);
		//Retrieve Sensor Data: String.Format("https://www.imonnit.com/xml/SensorDataMessages/{0}?sensorID={1}&fromDate={2}", AuthToken, NetworkID, DateTime.Now);
		Debug.Log ("Begin Monnit:  " + _gateway.GatewayID + " Status : " +   _gateway.ServerHostAddress + " Sensor: " + _server.FindSensor((uint)_sensor.SensorID) );

        
    }

    void Handle_GatewayMessage(object sender, GatewayMessageEventArgs e)
    {
        Debug.Log(string.Format("{0} GatewayID: {1} Type:{2} MessageCount: {3}", e.GatewayMessage.ReceivedDate, e.GatewayMessage.GatewayID, (BaseApplication.eGatewayMessageType)e.GatewayMessage.MessageType, e.GatewayMessage.MessageCount));
        if ((BaseApplication.eGatewayMessageType)e.GatewayMessage.MessageType == BaseApplication.eGatewayMessageType.Data)
        {
            Debug.Log(e.GatewayMessage.ToString());
        }
    }

    void Handle_PersistSensor(object sender, HandlePersistSensorEventArgs e) {
        Debug.Log("Sensor");
    }

    void Handle_PersistGateway(object sender, HandlePersistGatewayEventArgs e)
    {

        Gateway GatewayObj = _server.FindGateway(e.GatewayID);
        if (GatewayObj != null)
        {
            //Code to persist gateway to data store
            Debug.Log("Gateway");
        }
    }

    void Handle_LogException(object sender, HandleLogExceptionEventArgs e)
    {
        Debug.Log(string.Format("Exception - {0}", e.Exception.Message));
    }

    void OnApplicationQuit()
	{
        if (_server != null)
           	_server.StopServer ();
	}

	void Handle_SensorMessage(object sender, SensorMessagesEventArgs e) 
	{
        List<string> OutputList = new List<string>();
        data = new List<double>();
        foreach (SensorMessage message in e.SensorMessages)
        {
            foreach (Datum datum in message.Data)
            {
                double d = Math.Round(Double.Parse(datum.Data.ToString()));

                if (!_init)
                {
                    startingAngle = d;
                    _init = true;
                    Debug.Log("Starting angle: " + startingAngle);
                }

                OutputList.Add(string.Format("     {0}: {1}  ", datum.Description, d));
                data.Add(d);
            }
            OutputList.Add("Voltage: " + message.Voltage);
        }


        String s = "";
        foreach (string x in OutputList)
        {
            s += x + "\n";
        }

        _update = true;
        timer = 0;
        Debug.Log(s);
        double avgPitch = 0, avgRoll = 0;
        for (int i = 0; i < data.Count; i += 2)
        {
            avgPitch += data[i];
        }

        for (int i = 1; i < data.Count; i += 2)
        {
            avgRoll += data[i];
        }

        targetRotation = new Vector3((float)avgPitch / (data.Count / 2), (float)avgRoll / (data.Count / 2), 0);
    }

    void TurnOffUpdate()
    {
        _update = false;
    }*/
}
