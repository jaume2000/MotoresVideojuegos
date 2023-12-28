using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletPattern : MonoBehaviour, BulletGenerator {

    public GameObject[] bullet_prefabs;

    protected float awaiting_time = 1f;
    protected bool activated = true;

    protected RTDESKEngine engine;
    protected RTDESKEntity entity;
    public void activate(){activated = true;}
    public void deactivate(){activated = false;}

    private void Awake()
    {
        //Asignar el "listener" al componente normalizado que contienen todos los objetos que pueden recibir mensajes
        GetComponent<RTDESKEntity>().MailBox = MailBox;
    }

    void Start()
    {
        entity = GetComponent<RTDESKEntity>();
        engine = entity.RTDESKEngineScript;

        start();

        ObjectMsg Msg  = (ObjectMsg)engine.PopMsg((int)UserMsgTypes.Object);
        Msg.o = null;
        engine.SendMsg(Msg, gameObject, MailBox, engine.ms2Ticks(0));
        
    }

    protected void MailBox (MsgContent Msg)
    {
        switch (Msg.Type)
        {
            //Queremos generar N bullets.
            case (int)UserMsgTypes.Object:
                if(!player.gameover){
                    generator_loop();
                }
                break;

            //Faltan los casos Action (start) y Action (end) que nos permitan parar y reanudar el ciclo recursivo desde Generator Manager

            case (int)UserMsgTypes.Action:
                if(!player.gameover){
                    //Hacer cosas

                    //Switch case de Action start, end (etc.)

                }
                break;
                break;

            default:
                break;
        }

    }

    protected void generator_loop(){

        generate();

        ObjectMsg Msg  = (ObjectMsg)engine.PopMsg((int)UserMsgTypes.Object);
        Msg.o = null;
        engine.SendMsg(Msg, gameObject, MailBox, engine.ms2Ticks(awaiting_time*1000));
    }

    protected abstract void generate();
    protected abstract void start();

    
}


