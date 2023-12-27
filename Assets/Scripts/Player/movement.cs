using System;
using UnityEngine;

[RequireComponent(typeof(RTDESKEntity))]
public class movement : MonoBehaviour
{
    RTDESKEngine Engine;  
    
    public Vector2 dir = new Vector2(0, 0);
    public float speed;
    public static movement player;

    public Collider2D collider;
    
    private void Awake()
    {
        //Asignar el "listener" al componente normalizado que contienen todos los objetos que pueden recibir mensajes
        GetComponent<RTDESKEntity>().MailBox = ReceiveMessage;
        collider = GetComponent<Collider2D>();
        player = this;
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

    private void Update()
    {
        transform.Translate(dir.normalized * speed * Time.deltaTime);
    }

    void ReceiveMessage(MsgContent Msg)
    {
        Debug.Log(((RTDESKInputMsg)Msg).c + " " + ((RTDESKInputMsg)Msg).s);
    }
    
    void MovementManager(KeyCode key, int action = 0)
    {
        dir.x = 0;
        dir.y = 0;
        switch (key)
        {
            case KeyCode.W:
                dir.y = 1;
                break;
            case KeyCode.A:
                dir.x = -1;
                break;
            case KeyCode.S:
                dir.y = -1;
                break;
            case KeyCode.D:
                dir.x = 1;
                break;
        }
    }

    public void collided_with_bullet(){
        //Este método se llama desde BulletGameObject.cs, detecta la colisión y ejecuta este metodo.
        Debug.Log("Collisión con bullet.");
    }
}
