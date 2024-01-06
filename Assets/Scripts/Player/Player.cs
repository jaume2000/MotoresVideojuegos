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

    public UnityEngine.Transform canvas;
    public UnityEngine.Transform bulletStore;
    public BulletPattern main_generator;

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
        canvas.gameObject.SetActive(false);
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

        if(Input.GetKey(KeyCode.W) && transform.position.y < mapBorder[0].y){
            dir.y = 1;
        }
        else if(Input.GetKey(KeyCode.S) && transform.position.y > mapBorder[1].y){
            dir.y = -1;
        }

        if(Input.GetKey(KeyCode.D) && transform.position.x < mapBorder[0].x){
            dir.x = 1;
        }
        else if(Input.GetKey(KeyCode.A)  && transform.position.x > mapBorder[1].x){
            dir.x = -1;
        }

        return dir.normalized;
    }

    public void collided_with_bullet(){
        //Este método se llama desde BulletGameObject.cs, detecta la colisión y ejecuta este metodo.
        if(inmortal){
            return;
        }
        gameover = true;
        canvas.gameObject.SetActive(true);
    }

    public void restartGame(){
        if(gameover){
            gameover=false;

            var main_mailbox = main_generator.GetComponent<RTDESKEntity>().MailBox;

            Action Msg  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
            Msg.action = (int)UserActions.End;
            engine.SendMsg(Msg, gameObject, main_mailbox, HRTimer.HRT_ALMOST_INMEDIATELY);

            for(int i = 0; i < bulletStore.childCount; i++){
                UnityEngine.Transform t = bulletStore.GetChild(i);
                Destroy(t.gameObject);
            }

            Action Msg2  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
            Msg2.action = (int)UserActions.Start;
            engine.SendMsg(Msg2, gameObject, main_mailbox, engine.ms2Ticks(3000));

            Start();

            canvas.gameObject.SetActive(false);

            transform.position = Vector3.zero;
            

        }
    }
}
