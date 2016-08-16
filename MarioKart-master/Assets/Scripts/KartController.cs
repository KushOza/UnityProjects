using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class KartController : MonoBehaviour {

	public GameObject banana;
	public bool showItem = false;
	//SerialPort sPort = PortMan.sPort;
	SerialPort sPort = new SerialPort("COM8", 9600);
	public static int speed = 100;
	private int bananas = 0;
	private float slowDownTime;
	private bool hitBanana = false;
	private bool speedBoost = false;
	private float speedUpTime;
	private string itemName = "Nothing";

	void Update()
	{
		if (bananas > 0) 
		{
			if (Input.GetKeyDown (KeyCode.T)) 
			{
				bananas--;
				Vector3 spawnPosition;
				if (bananas == 2) 
				{
					itemName = "2 Bananas";
					spawnPosition = this.transform.parent.FindChild ("Items").FindChild ("HamburgerTres").transform.position;
					this.transform.parent.FindChild ("Items").FindChild ("HamburgerTres").gameObject.SetActive(false);
				} 
				else if (bananas == 1) 
				{
					itemName = "1 Banana";
					spawnPosition = this.transform.parent.FindChild ("Items").FindChild ("HamburgerDos").transform.position;
					this.transform.parent.FindChild ("Items").FindChild ("HamburgerDos").gameObject.SetActive(false);
				}
				else
				{
					itemName = "Nothing";
					spawnPosition = this.transform.parent.FindChild ("Items").FindChild ("HamburgerUno").transform.position;
					this.transform.parent.FindChild ("Items").FindChild ("HamburgerUno").gameObject.SetActive(false);
					bananas = 0;
				}
				Instantiate (banana, spawnPosition, this.transform.rotation);
			}
		}
		if (Time.time - slowDownTime > 2.0 && hitBanana) 
		{
			this.transform.parent.GetComponent<OVRPlayerControllerVanilla> ().Acceleration *= 5.0f;
			slowDownTime = Time.time;
			hitBanana = false;
		}
		if (speedBoost) 
		{
			if (Input.GetKeyDown (KeyCode.T)) 
			{
				itemName = "Nothing";
				this.transform.parent.FindChild ("Items").FindChild ("MushroomUno").gameObject.SetActive(false);
				speedBoost = false;
				speedUpTime = Time.time;
				this.transform.parent.GetComponent<OVRPlayerControllerVanilla> ().Acceleration *= 3.0f;
			}
		}
		if (speedUpTime > 0) 
		{
			if (Time.time - speedUpTime > 3.0) 
			{
				speedUpTime = 0;
				this.transform.parent.GetComponent<OVRPlayerControllerVanilla> ().Acceleration /= 3.0f;
			}
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Off Road")) 
		{
			speed = 25;
			if (sPort.IsOpen) 
			{
				sPort.WriteLine (speed + ",");
			}
		} 
		else if (other.gameObject.CompareTag ("Item Box")) 
		{
			showItem = true;
			StartCoroutine (respawn (other, 7));

			//determine whether to spawn 1, 2, or 3 bananas
			int randomItem = Random.Range (1, 5);
			if (randomItem < 4)
				spawnBananas(randomItem);
			else
				spawnMushrooms();
		} 
		else if (other.gameObject.CompareTag ("Banana")) 
		{
			DestroyObject (other.gameObject);
			this.transform.parent.GetComponent<OVRPlayerControllerVanilla> ().Acceleration *= 0.2f;
			slowDownTime = Time.time;
			hitBanana = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Off Road")){
			speed = 100;
			if (sPort.IsOpen) {
				sPort.WriteLine (speed + ",");
			}
		}
	}

	void spawnBananas(int numBananas)
	{
		speedBoost = false;
		this.transform.parent.FindChild ("Items").FindChild ("HamburgerUno").gameObject.SetActive(false);
		this.transform.parent.FindChild ("Items").FindChild ("HamburgerDos").gameObject.SetActive(false);
		this.transform.parent.FindChild ("Items").FindChild ("HamburgerTres").gameObject.SetActive(false);

		if (numBananas >= 1) 
		{
			itemName = "1 Banana";
			this.transform.parent.FindChild ("Items").FindChild ("HamburgerUno").gameObject.SetActive(true);
		}
		if (numBananas >= 2) 
		{
			itemName = "2 Bananas";
			this.transform.parent.FindChild ("Items").FindChild ("HamburgerDos").gameObject.SetActive(true);
		}
		if (numBananas >= 3) 
		{
			itemName = "3 Bananas";
			this.transform.parent.FindChild ("Items").FindChild ("HamburgerTres").gameObject.SetActive(true);
		}
		bananas = numBananas;
	}

	void spawnMushrooms ()
	{
		itemName = "Mushroom";
		bananas = 0;
		this.transform.parent.FindChild ("Items").FindChild ("HamburgerUno").gameObject.SetActive(false);
		this.transform.parent.FindChild ("Items").FindChild ("HamburgerDos").gameObject.SetActive(false);
		this.transform.parent.FindChild ("Items").FindChild ("HamburgerTres").gameObject.SetActive(false);
		this.transform.parent.FindChild ("Items").FindChild ("MushroomUno").gameObject.SetActive(true);
		speedBoost = true;
	}

	IEnumerator respawn(Collider obj,float respawnTime)
	{
		obj.gameObject.SetActive (false);
		yield return new WaitForSeconds(respawnTime);
		obj.gameObject.SetActive (true);
	}

	void OnGUI()
	{
		GUI.Label (new Rect (30, 150, 150, 35), "Current Item: " + itemName);
	}

	void OnApplicationQuit()
	{
		sPort.Close();
	}
}