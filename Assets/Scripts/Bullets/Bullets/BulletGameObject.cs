using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGameObject: MonoBehaviour
{
    public Bullet behaviour;

    public static float UPDATE_DELTA = 10f;

    private RTDESKEngine engine;
    private RTDESKEntity entity;
    
    private bool destroy = false;
    private Collider2D collider;
    private float survival_time = 1;
    

    public void setBehaviour(Bullet behaviour){
        this.behaviour = behaviour;
    }

    public void setSurvivalTIme(float t){
        survival_time = t;
    }

    private float survivalTime = 4f;

    void Awake(){
        //Asignar el "listener" al componente normalizado que contienen todos los objetos que pueden recibir mensajes
        GetComponent<RTDESKEntity>().MailBox = MailBox;
    }

    void Start(){
        entity = GetComponent<RTDESKEntity>();
        engine = entity.RTDESKEngineScript;
        collider = gameObject.GetComponent<Collider2D>();

        Action Msg  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
        Msg.action = (int)UserActions.Move;
        engine.SendMsg(Msg, gameObject, MailBox, engine.ms2Ticks(player.UPDATE_DELTA)/*HRTimer.HRT_INMEDIATELY*/);

        Action Msg2  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
        Msg2.action = (int)UserActions.End;
        engine.SendMsg(Msg2, gameObject, MailBox, engine.ms2Ticks(survival_time*1000));
    }

    private void move()
    {
        if(destroy){
            Object.Destroy(gameObject);
            return;
        }
        behaviour.update(player.UPDATE_DELTA/1000);

        if(player.i.collider.IsTouching(collider)){
            player.i.collided_with_bullet();
        }

        Action Msg  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
        Msg.action = (int)UserActions.Move;
        engine.SendMsg(Msg, gameObject, MailBox, engine.ms2Ticks(player.UPDATE_DELTA)/*HRTimer.HRT_INMEDIATELY*/);
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
                        if(!player.gameover){
                            move();
                        }
                        break;

                    case (int)UserActions.End:
                        destroy=true;
                        break;
                    
                }
                break;

            default:
                break;
        }

    }    

}
