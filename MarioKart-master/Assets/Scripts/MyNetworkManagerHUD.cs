#if ENABLE_UNET

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkManagerHUD")]
	[RequireComponent(typeof(NetworkManager))]
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class MyNetworkManagerHUD : MonoBehaviour
	{
		private NetworkManager manager;

		// Runtime variables
		private bool confirm = false;	//confirm that the user wants to quit

		void Awake()
		{
			manager = this.GetComponent<NetworkManager>();
		}

		void Update()
		{
			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				//S - start server, H - be server + client, C - start client, Q - quit
				if (Input.GetKeyDown(KeyCode.S))
				{
					manager.StartServer();
				}
				if (Input.GetKeyDown(KeyCode.H))
				{
					manager.StartHost();
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					manager.StartClient();
				}
				if (Input.GetKeyDown (KeyCode.Q)) 
				{
					Application.Quit();
				}
			}
			if (NetworkServer.active && NetworkClient.active)
			{
				//if user is a host
				if (!confirm)
				{
					if (Input.GetKeyDown (KeyCode.Q)) 
					{
						confirm = true;
					}
				} 
				else 
				{
					if (Input.GetKeyDown (KeyCode.Y)) 
					{
						confirm = false;
						manager.StopHost();
					}
					if (Input.GetKeyDown (KeyCode.N)) 
					{
						confirm = false;
					}
				}
			}
			if (NetworkServer.active && !NetworkClient.active)
			{
				//if user is a server
				if (!confirm) 
				{
					if (Input.GetKeyDown (KeyCode.Q)) 
					{
						confirm = true;
					}
				} 
				else 
				{
					if (Input.GetKeyDown (KeyCode.Y)) 
					{
						confirm = false;
						manager.StopServer();
					}
					if (Input.GetKeyDown (KeyCode.N)) 
					{
						confirm = false;
					}
				}
			}
			if (!NetworkServer.active && NetworkClient.active)
			{
				//if user is a client
				if (!confirm) 
				{
					if (Input.GetKeyDown (KeyCode.Q)) 
					{
						confirm = true;
					}
				} 
				else 
				{
					if (Input.GetKeyDown (KeyCode.Y)) 
					{
						confirm = false;
						manager.StopClient();
					}
					if (Input.GetKeyDown (KeyCode.N)) 
					{
						confirm = false;
					}
				}
			}
		}

		void OnGUI()
		{
			int xpos = 30;
			int ypos = 30;
			int spacing = 50;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (GUI.Button(new Rect(xpos, ypos, 150, 35), "Host (H)"))
				{
					manager.StartHost();
				}
				ypos += spacing;
				if (GUI.Button(new Rect(xpos, ypos, 150, 35), "Client (C)"))
				{
					manager.StartClient();
				}
				ypos += spacing;
				if (GUI.Button(new Rect(xpos, ypos, 150, 35), "Server (S)"))
				{
					manager.StartServer();
				}
				ypos += spacing;
				if (GUI.Button (new Rect (xpos, ypos, 150, 35), "Quit (Q)")) 
				{
					Application.Quit ();
				}
				ypos += spacing;
				GUI.Label(new Rect(xpos, ypos, 300, 35), "Network Address: " + manager.networkAddress);
				ypos += spacing;
			}
			else
			{
				if (NetworkServer.active)
				{
					GUI.Label(new Rect(xpos, ypos, 150, 35), "Server running on port: " + manager.networkPort);
					ypos += spacing;
				}
				if (NetworkClient.active)
				{
					GUI.Label(new Rect(xpos, ypos, 150, 35), "Client address: " + manager.networkAddress + " on port: " + manager.networkPort);
					ypos += spacing;
				}
			}

			if (NetworkServer.active || NetworkClient.active)
			{
				ypos += 100;
				if (!confirm) 
				{
					if (GUI.Button (new Rect (xpos, ypos, 150, 35), "Exit Level (Q)")) 
					{
						confirm = true;
					} 
				}
				else 
				{
					if (GUI.Button (new Rect (xpos + 200, ypos, 150, 35), "Confirm Quit (Y)")) 
					{
						confirm = false;
						manager.StopHost();
					} 
					if (GUI.Button (new Rect (xpos + 400, ypos, 150, 35), "Back to Game (N)")) 
					{
						confirm = false;
					} 
					ypos += spacing;
				}
				ypos += spacing;
			}
		}
	}
};
#endif //ENABLE_UNET
