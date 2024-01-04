using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletPattern : MonoBehaviour {

    public GameObject[] bullet_prefabs;

    protected float awaiting_time = 1f;

    [SerializeField]
    protected bool active = true;

    [SerializeField]
    protected float mapBorderMultiplier = 1;
    protected bool started = false;

    protected RTDESKEngine engine;
    protected RTDESKEntity entity;
    public virtual void activate(){
        if(!active){generator_loop();}
        active = true;
        }
    public virtual void deactivate(){active = false;}
    public bool isActive(){return active;}

    private void Awake()
    {
        //Asignar el "listener" al componente normalizado que contienen todos los objetos que pueden recibir mensajes
        GetComponent<RTDESKEntity>().MailBox = MailBox;
    }

    protected void Start()
    {
        if(started){return;}

        started = true;
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
                if(!Player.gameover && active){

                    //Aqui se produce el BUCLE DE LLAMADAS
                    generator_loop();

                }
                break;

            //Faltan los casos Action (start) y Action (end) que nos permitan parar y reanudar el ciclo recursivo desde Generator Manager

            case (int)UserMsgTypes.Action:
                if(!Player.gameover){
                    Action a = (Action)Msg;
                    switch (a.action)
                    {
                        case (int)UserActions.Start:
                            activate();
                            break;

                        case (int)UserActions.End:
                            deactivate();
                            break;
                    }
                }
                break;

            default:
                break;
        }

    }

    protected void generator_loop(){

        if(engine == null){Start();}
        generate();

        ObjectMsg Msg  = (ObjectMsg)engine.PopMsg((int)UserMsgTypes.Object);
        Msg.o = null;
        engine.SendMsg(Msg, gameObject, MailBox, engine.ms2Ticks(awaiting_time*1000));
    }

    protected abstract void generate();
    protected abstract void start();

    
}


