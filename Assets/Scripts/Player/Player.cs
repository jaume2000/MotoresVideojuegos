using System;
using UnityEngine;

[RequireComponent(typeof(RTDESKEntity))]
public class Player : MonoBehaviour
{
    RTDESKEngine Engine;  
    
    [SerializeField]
    private float speed = 1f;
    private Vector2 dir = new Vector2(0, 0);
    public static Player i;
    public static bool gameover = false;
    public bool inmortal = false;
    public static float UPDATE_DELTA = 10f;

    private float deltaTime = 0f;
    private System.DateTime last_time_stamp;
    private System.DateTime time_stamp;

    public Collider2D my_collider;

    private RTDESKEngine engine;
    private RTDESKEntity entity;

    public Vector2[] mapBorder = new Vector2[2];
    
    private void Awake()
    {
        //Asignar el "listener" al componente normalizado que contienen todos los objetos que pueden recibir mensajes
        GetComponent<RTDESKEntity>().MailBox = MailBox;
        my_collider = GetComponent<Collider2D>();
        i = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<RTDESKEntity>();
        engine = entity.RTDESKEngineScript;

        Action Msg  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
        Msg.action = (int)UserActions.Move;
        engine.SendMsg(Msg, gameObject, MailBox, engine.ms2Ticks(Player.UPDATE_DELTA));
        last_time_stamp = System.DateTime.Now;
    }

    private void Update()
    {
        transform.Translate(dir.normalized * speed * Time.deltaTime);
    }

    void MailBox (MsgContent Msg)
    {
        switch (Msg.Type)
        {
            //Queremos generar N bullets.
            case (int)UserMsgTypes.Action:
                Action a = (Action)Msg;
                switch (a.action)
                {
                    case (int)UserActions.Move:
                        if(!Player.gameover){
                            move();
                        }
                        break;

                    case (int)UserActions.End:
                        break;
                    
                }
                break;

            default:
                break;
        }

    }   


    private void move(){
        time_stamp = System.DateTime.Now;

        deltaTime = (float)(engine.Ticks2ms((time_stamp - last_time_stamp).Ticks)/1000f);

        transform.Translate(playerMovementDirection() * speed * deltaTime);
        //Debug.Log(playerMovementDirection() + " "  +deltaTime);

        Action Msg  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
        Msg.action = (int)UserActions.Move;
        engine.SendMsg(Msg, gameObject, MailBox, engine.ms2Ticks(Player.UPDATE_DELTA));

        last_time_stamp = time_stamp;
    }
    
    private Vector2 playerMovementDirection()
    {
        Vector2 dir = Vector2.zero;

        if(Input.GetKey(KeyCode.W)){
            dir.y = 1;
        }
        else if(Input.GetKey(KeyCode.S)){
            dir.y = -1;
        }

        if(Input.GetKey(KeyCode.D)){
            dir.x = 1;
        }
        else if(Input.GetKey(KeyCode.A)){
            dir.x = -1;
        }

        return dir.normalized;
    }

    public void collided_with_bullet(){
        //Este método se llama desde BulletGameObject.cs, detecta la colisión y ejecuta este metodo.
        gameover = true && !inmortal;
    }
}
