using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(RTDESKEntity))]
public class CircularPattern : BulletPattern
{

    public float awaiting_time= 3;
    public float phase_shift = 0.1f;
    private float total_phase = 0;
    public float bullet_velocity = 5;
    public int bullet_count = 5;
    public int survival_time = 5;

    protected float time_transcured = 0;

    private RTDESKEngine engine;
    private RTDESKEntity entity;

    private void Awake()
    {
        //Asignar el "listener" al componente normalizado que contienen todos los objetos que pueden recibir mensajes
        GetComponent<RTDESKEntity>().MailBox = MailBox;
    }
    
    void Start()
    {
        entity = GetComponent<RTDESKEntity>();
        engine = entity.RTDESKEngineScript;


        ObjectMsg Msg  = (ObjectMsg)engine.PopMsg((int)UserMsgTypes.Object);
        Msg.o = null;
        engine.SendMsg(Msg, gameObject, MailBox, engine.ms2Ticks(awaiting_time*500));
        
    }

    public void genertate(){


        for(int i=0; i< bullet_count; i++){
            GameObject lb = Instantiate(this.bullet_prefabs[0], transform.position, Quaternion.identity);
            BulletGameObject bgo = lb.GetComponent<BulletGameObject>();
            bgo.setSurvivalTIme(survival_time);
            float angle = 2*Mathf.PI/bullet_count * i + total_phase/180*Mathf.PI;
            bgo.setBehaviour(new LinearBullet(lb, Mathf.Cos(angle), Mathf.Sin(angle), bullet_velocity));
        }

        total_phase+=phase_shift;
        total_phase = total_phase % 360;

    }

    void MailBox (MsgContent Msg)
    {
        switch (Msg.Type)
        {
            //Queremos generar N bullets.
            case (int)UserMsgTypes.Object:
                if(!player.gameover){
                    loop();
                }
                break;

            default:
                break;
        }

    }

    void loop(){

        genertate();

        ObjectMsg Msg  = (ObjectMsg)engine.PopMsg((int)UserMsgTypes.Object);
        Msg.o = null;
        engine.SendMsg(Msg, gameObject, MailBox, engine.ms2Ticks(awaiting_time*1000));
    }
}
