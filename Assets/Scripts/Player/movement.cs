using UnityEngine;

[RequireComponent(typeof(RTDESKEntity))]
public class movement : MonoBehaviour
{
    RTDESKEngine Engine;  
    
    public Vector2 dir = new Vector2(0, 0);
    public float speed;
    
    private void Awake()
    {
        //Asignar el "listener" al componente normalizado que contienen todos los objetos que pueden recibir mensajes
        GetComponent<RTDESKEntity>().MailBox = ReceiveMessage;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject      engine = GameObject.Find(RTDESKEngine.Name);

        Engine = engine.GetComponent<RTDESKEngine>();
        RTDESKInputManager IM = engine.GetComponent<RTDESKInputManager>();

        //Register keys that we want to be signaled in case the user press them
        IM.RegisterKeyCode(ReceiveMessage, KeyCode.W);
        IM.RegisterKeyCode(ReceiveMessage, KeyCode.A);
        IM.RegisterKeyCode(ReceiveMessage, KeyCode.S);
        IM.RegisterKeyCode(ReceiveMessage, KeyCode.D);
    }

    // Update is called once per frame
    void Update()
    {
        // control player with wasd keys
        dir.x = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
        dir.y = Input.GetKey(KeyCode.S) ? -1 : Input.GetKey(KeyCode.W) ? 1 : 0;
        
        // move player
        transform.Translate(dir.normalized * speed * Time.deltaTime);
    }
    
    void ReceiveMessage(MsgContent Msg)
    {
        Debug.Log("patata");
    }
}
