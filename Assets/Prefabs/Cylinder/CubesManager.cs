using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesManager : MonoBehaviour
{
    List<MessageManager> Cubes = new List<MessageManager>();  //List of cubes to stores to manage

    RTDESKEngine engine;

    double deltaTime = 1450.0d;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<RTDESKEntity>().MailBox = MailBox;
    }

    void Start()
    {
        engine      = GetComponent<RTDESKEntity>().RTDESKEngineScript;
        Action Msg  = (Action)engine.PopMsg((int)UserMsgTypes.Action);
        Msg.action  = (int)UserActions.Move;

        engine.SendMsg(Msg, gameObject, MailBox, engine.ms2Ticks(deltaTime));
    }

    void MailBox (MsgContent Msg)
    {
        switch (Msg.Type)
        {
            case (int)UserMsgTypes.Object:
                Cubes.Add(RTDESKEntity.getMailBox(((ObjectMsg)Msg).o));
                engine.PushMsg(Msg);
                break;
            case ((int)UserMsgTypes.Action):
                foreach(MessageManager MM in Cubes)
                {
                    Action a = (Action)engine.PopMsg((int)UserMsgTypes.Action);
                    if ((int)UserActions.GetSteady == ((Action)Msg).action)
                        a.action = (int)UserActions.Move;
                    else
                        a.action = (int)UserActions.GetSteady;

                    engine.SendMsg(a, gameObject, MM, engine.ms2Ticks(deltaTime));
                }

                if ((int)UserActions.GetSteady == ((Action)Msg).action)
                    ((Action)Msg).action = (int)UserActions.Move;
                else((Action)Msg).action = (int)UserActions.GetSteady;

                engine.SendMsg(Msg, engine.ms2Ticks(deltaTime));
                break;
        }
    }
}
