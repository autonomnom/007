using System;
using System.ComponentModel;

#if ENABLE_UNET

namespace UnityEngine.Networking {

    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class YoogooohUD : MonoBehaviour {

        [HideInInspector] public Yoohooho yoghurt;

        [SerializeField] public bool showGUI = true;
        [SerializeField] public int offsetX;
        [SerializeField] public int offsetY;

        // Runtime variable
        bool m_ShowServer;

        // Container for specific images (has to be defined as texture)
        public Texture lanhost;
        public Texture lanclient;
        public Texture lanserveronly;
        public Texture cancelconnectionattempt;
        public Texture clientready;
        public Texture stopx;
        public Texture enablematchmaker;
        public Texture createinternetmatch;
        public Texture findinternetmatch;
        public Texture joinmatch;
        public Texture backtomatchmenu;
        public Texture changemmserver;
        public Texture mmlocal;
        public Texture mminternet;
        public Texture mmstaging;
        public Texture disablematchmaker;

        void Awake() {

            yoghurt = GetComponent<Yoohooho>();
        }

        void Update() {

            if (!showGUI)
                return;

            if (!yoghurt.IsClientConnected() && !NetworkServer.active && yoghurt.matchMaker == null) {

                if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer) {
                    if (Input.GetKeyDown(KeyCode.S)) {
                        yoghurt.StartServer();
                    }
                    if (Input.GetKeyDown(KeyCode.H)) {
                        yoghurt.StartHost();
                    }
                }
                if (Input.GetKeyDown(KeyCode.C)) {
                    yoghurt.StartClient();
                }
            }
            if (NetworkServer.active && yoghurt.IsClientConnected()) {

                if (Input.GetKeyDown(KeyCode.X)) {
                    yoghurt.StopHost();
                }
            }
        }

        void OnGUI() {

            if (!showGUI)
                return;

            int xpos = 10 + offsetX;
            int ypos = 40 + offsetY;
            const int spacing = 24;

            bool noConnection = (yoghurt.client == null || yoghurt.client.connection == null ||
                                 yoghurt.client.connection.connectionId == -1);

            if (!yoghurt.IsClientConnected() && !NetworkServer.active && yoghurt.matchMaker == null) {

                if (noConnection) {
                    if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer) {
                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)")) {
                            yoghurt.StartHost();
                        }
                        ypos += spacing;
                    }

                    if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(C)")) {
                        yoghurt.StartClient();
                    }

                    yoghurt.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), yoghurt.networkAddress);
                    ypos += spacing;

                    if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer) {
                        // cant be a server in webgl build
                        GUI.Box(new Rect(xpos, ypos, 200, 25), "(  WebGL cannot be server  )");
                        ypos += spacing;
                    }
                    else {
                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)")) {
                            yoghurt.StartServer();
                        }
                        ypos += spacing;
                    }
                }
                else {

                    GUI.Label(new Rect(xpos, ypos, 200, 20), "Connecting to " + yoghurt.networkAddress + ":" + yoghurt.networkPort + "..");
                    ypos += spacing;


                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Cancel Connection Attempt")) {
                        yoghurt.StopClient();
                    }
                }
            }
            else {

                if (NetworkServer.active) {
                    string serverMsg = "Server: port=" + yoghurt.networkPort;
                    if (yoghurt.useWebSockets) {
                        serverMsg += " (Using WebSockets)";
                    }
                    GUI.Label(new Rect(xpos, ypos, 300, 20), serverMsg);
                    ypos += spacing;
                }
                if (yoghurt.IsClientConnected()) {
                    GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + yoghurt.networkAddress + " port=" + yoghurt.networkPort);
                    ypos += spacing;
                }
            }

            if (yoghurt.IsClientConnected() && !ClientScene.ready) {

                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready")) {
                    ClientScene.Ready(yoghurt.client.connection);

                    if (ClientScene.localPlayers.Count == 0) {
                        ClientScene.AddPlayer(0);
                    }
                }
                ypos += spacing;
            }

            if (NetworkServer.active || yoghurt.IsClientConnected()) {

                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)")) {
                    yoghurt.StopHost();
                }
                ypos += spacing;
            }

            if (!NetworkServer.active && !yoghurt.IsClientConnected() && noConnection) {

                ypos += 10;

                if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer) {
                    GUI.Box(new Rect(xpos - 5, ypos, 220, 25), "(WebGL cannot use Match Maker)");
                    return;
                }

                if (yoghurt.matchMaker == null) {
                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Enable Match Maker (M)")) {
                        yoghurt.StartMatchMaker();
                    }
                    ypos += spacing;
                }
                else {

                    if (yoghurt.matchInfo == null) {

                        if (yoghurt.matches == null) {

                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Create Internet Match")) {

                                yoghurt.matchMaker.CreateMatch(yoghurt.matchName, yoghurt.matchSize, true, "", "", "", 0, 0, yoghurt.OnMatchCreate);
                            }
                            ypos += spacing;

                            GUI.Label(new Rect(xpos, ypos, 100, 20), "Room Name:");
                            yoghurt.matchName = GUI.TextField(new Rect(xpos + 100, ypos, 100, 20), yoghurt.matchName);
                            ypos += spacing;

                            ypos += 10;

                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match")) {
                                yoghurt.matchMaker.ListMatches(0, 20, "", false, 0, 0, yoghurt.OnMatchList);
                            }
                            ypos += spacing;
                        }
                        else {

                            for (int i = 0; i < yoghurt.matches.Count; i++) {

                                var match = yoghurt.matches[i];
                                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name)) {
                                    yoghurt.matchName = match.name;
                                    yoghurt.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, yoghurt.OnMatchJoined);
                                }

                                ypos += spacing;
                            }

                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Back to Match Menu")) {
                                yoghurt.matches = null;
                            }
                            ypos += spacing;
                        }
                    }

                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Change MM server")) {

                        m_ShowServer = !m_ShowServer;
                    }
                    if (m_ShowServer) {

                        ypos += spacing;

                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Local")) {
                            yoghurt.SetMatchHost("localhost", 1337, false);
                            m_ShowServer = false;
                        }
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Internet")) {
                            yoghurt.SetMatchHost("mm.unet.unity3d.com", 443, true);
                            m_ShowServer = false;
                        }
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Staging")) {
                            yoghurt.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
                            m_ShowServer = false;
                        }
                    }

                    ypos += spacing;

                    GUI.Label(new Rect(xpos, ypos, 300, 20), "MM Uri: " + yoghurt.matchMaker.baseUri);
                    ypos += spacing;

                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Disable Match Maker")) {
                        yoghurt.StopMatchMaker();
                    }
                    ypos += spacing;
                }
            }
        }
    }
}
#endif //ENABLE_UNET