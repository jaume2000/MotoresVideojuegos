using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGameObject: MonoBehaviour
{
    public Bullet behaviour;

    public void setBehaviour(Bullet behaviour){
        this.behaviour = behaviour;
    }

    void Update()
    {
        behaviour.update(Time.deltaTime);
    }

}
