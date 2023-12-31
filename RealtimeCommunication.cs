using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

enum Player
{
    Player1,
    Player2
}

[Serializable]
public class RequestForm
{
    public string from;
    public string opponent;
    public CommunicationData data;

}

[Serializable]
public class ResponseForm
{
    public string status;
    public CommunicationData data;
}

[Serializable]
public class CommunicationData
{
    public Vector3 headPosition;
    public Quaternion headRotation;
    public Vector3 parentPosition;
    public Quaternion parentRotation;
    public Vector3 pelvisPosition;
    public Vector3 leftHandPosition;

    public Vector3 rightHandPosition;
    
}

    public class RealtimeCommunication : MonoBehaviour
    {
        [SerializeField] private string hostIPAddress = "118.27.8.251";
        [SerializeField] private int hostPort = 50000;
        [SerializeField] private int timeOut =10;
        [SerializeField] private Player playerName = Player.Player1;
        [SerializeField] private int interval = 30;

        [SerializeField] private Transform parent;
        [SerializeField] private Transform head;
        [SerializeField] private Transform pelvis;
        [SerializeField] private Transform rightHand;
        [SerializeField] private Transform leftHand;
        
        
        IPEndPoint _remoteIpEndPoint;
        private UdpClient _udpClient;
        private int _frameCount = 0;
        [NonSerialized] public CommunicationData OpponentData;
        void Start()
        {
            
            _udpClient = new UdpClient();
            try{

               
            }
            catch (Exception e ) {
                Debug.Log(e.ToString());
            }

        }

        void Connect()
        {
            _remoteIpEndPoint = new IPEndPoint(IPAddress.Parse(hostIPAddress), hostPort);
            _udpClient.Connect(_remoteIpEndPoint);
            _udpClient.Client.ReceiveTimeout = timeOut;
        }

        void Update()
        {
            _frameCount++;
            if (_frameCount % interval == 0)
            {
                try
                {
                    Sync();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    if (e is SocketException && _udpClient.Client.RemoteEndPoint == null)
                    {
                        Connect();
                    }
                }
            }
            
        }

        private void OnDestroy()
        {
            _udpClient.Close();
        }

        void Sync()
        {
            // Sends a message to the host to which you have connected.
            RequestForm requestForm = new RequestForm()
            {
                from = playerName == Player.Player1 ? "player1" : "player2",
                opponent = playerName == Player.Player1 ? "player2" : "player1",
                data = new CommunicationData()
                {
                    headPosition = head.position,
                    headRotation = head.rotation,
                    parentPosition = parent.position,
                    parentRotation = parent.rotation,
                    pelvisPosition = pelvis.position,
                    leftHandPosition = leftHand.position,
                    rightHandPosition = rightHand.position

                }
            };
            Byte[] sendBytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(requestForm));
            _udpClient.SendAsync(sendBytes, sendBytes.Length);
                

            //IPEndPoint object will allow us to read datagrams sent from any source.
                

            // Blocks until a message returns on this socket from a remote host.
            Byte[] receiveBytes = _udpClient.Receive(ref _remoteIpEndPoint);
            string returnData = Encoding.UTF8.GetString(receiveBytes);

            // Uses the IPEndPoint object to determine which of these two hosts responded.
            Debug.Log("This is the message you received " +
                      returnData.ToString());
            ResponseForm res = JsonUtility.FromJson<ResponseForm>(returnData);
            if (res.status.Equals("SUCCESS"))
            {
                OpponentData = res.data;
            }
            else
            {
                Debug.LogError((res.status));
            }
            
        }
    }
