using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGameObject: MonoBehaviour
{
    public Bullet behaviour;

    public static float UPDATE_DELTA = 50f;

    private RTDESKEngine engine;
    private RTDESKEntity entity;
    
    private bool destroy = false;
    private Collider2D my_collider;

    private float mapBorderMultiplier = 1;

    public void setBehaviour(Bullet behaviour){
        this.behaviour = behaviour;
    }

    /*
    public void setSurvivalTIme(float t){
        survival_time = t;
    }
    */

    public void setMapBorderMultiplier(float m){
        mapBorderMultiplier = m;
    }
    void Awake(){
        //Asignar el "listener" al componente normalizado que contienen todos los objetos que pueden recibir mensajes
        GetComponent<RTDESKEntity>().MailBox = MailBox;
    }

    void Start(){
        entity = GetComponent<RTDESKEntity>();
        engine = entity.RTDESKEngineScript;
        my_collider = gameObject.GetComponent<Collider2D>();

        Action Msg  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
        Msg.action = (int)UserActions.Move;
        engine.SendMsg(Msg, gameObject, MailBox, engine.ms2Ticks(Player.UPDATE_DELTA)/*HRTimer.HRT_INMEDIATELY*/);

        /*
        Action Msg2  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
        Msg2.action = (int)UserActions.End;
        engine.SendMsg(Msg2, gameObject, MailBox, engine.ms2Ticks(survival_time*1000));
        */
    }

    private bool isOutsideOfMap(){
        return (
            transform.position.x > Player.i.mapBorder[0].x * mapBorderMultiplier ||
            transform.position.x < Player.i.mapBorder[1].x * mapBorderMultiplier ||
            transform.position.y > Player.i.mapBorder[0].y * mapBorderMultiplier ||
            transform.position.y < Player.i.mapBorder[1].y * mapBorderMultiplier
        );
    }

    private void move()
    {
        
        

        if(isOutsideOfMap()){
            destroy=true;
        }
        if(destroy){
            Object.Destroy(gameObject);
            return;
        }
        behaviour.update(Player.UPDATE_DELTA/1000);

        if(Player.i.my_collider.IsTouching(my_collider)){
            Player.i.collided_with_bullet();
        }

        Action Msg  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
        Msg.action = (int)UserActions.Move;
        engine.SendMsg(Msg, gameObject, MailBox, engine.ms2Ticks(Player.UPDATE_DELTA)/*HRTimer.HRT_INMEDIATELY*/);
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
                        destroy=true;
                        break;
                    
                }
                break;

            default:
                break;
        }

    }    

}
