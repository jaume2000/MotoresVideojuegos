using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGenerators : BulletPattern
{
    [SerializeField]
    private (BulletPattern gen, MessageManager m)[] generators;

    [SerializeField]
    public float delay = 2f;

    private int active_child = 0;

    protected override void start(){
        GetComponent<RTDESKEntity>().MailBox = MailBox;
        awaiting_time = delay;

        UnityEngine.Transform[] ts = gameObject.GetComponentsInChildren<UnityEngine.Transform>();
        LinkedList<(BulletPattern gen, MessageManager m)> aux_generators = new LinkedList<(BulletPattern gen, MessageManager m)>();
        if (ts != null){
            for(int i = 0; i < transform.childCount; i++){
                UnityEngine.Transform t = transform.GetChild(i);
                if (t != null && t != transform){
                    aux_generators.AddLast((t.GetComponent<BulletPattern>(),t.GetComponent<RTDESKEntity>().MailBox));
                }
            }
        }

        generators = new (BulletPattern gen, MessageManager m)[aux_generators.Count];
        aux_generators.CopyTo(generators, 0);

        for(int i = 1; i < generators.Length; i++){
            Action Msg  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
            Msg.action = (int)UserActions.End;
            engine.SendMsg(Msg, gameObject, generators[i].m, HRTimer.HRT_INMEDIATELY);
        }

    }

    protected override void generate(){

        if(engine == null){Start();}
        
        Action Msg  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
        Msg.action = (int)UserActions.End;
        engine.SendMsg(Msg, gameObject, generators[(active_child-1 + transform.childCount) % transform.childCount].m, HRTimer.HRT_INMEDIATELY);

        

        Action Msg2  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
        Msg2.action = (int)UserActions.Start;
        engine.SendMsg(Msg2, gameObject, generators[active_child].m, HRTimer.HRT_INMEDIATELY);

        active_child = (active_child+1) % transform.childCount;
    }

    public override void deactivate(){
        base.deactivate();
        if(engine == null){Start();}

        foreach((BulletPattern gen, MessageManager m) g in generators){
            Action Msg  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
            Msg.action = (int)UserActions.End;
            engine.SendMsg(Msg, gameObject, g.m, HRTimer.HRT_INMEDIATELY);
        }
    }

    public override void activate(){
        base.activate();
        active_child = 0;
        if(engine == null){Start();}

        Action Msg2  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
        Msg2.action = (int)UserActions.Start;
        engine.SendMsg(Msg2, gameObject, generators[active_child].m, HRTimer.HRT_INMEDIATELY);
    }

}
