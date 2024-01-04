using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearBullet : Bullet
{

    private Vector2 dir;
    private float vel;
    
    public LinearBullet(GameObject obj, Vector2 dir, float vel) : base(obj){
        
        this.dir = dir;
        this.vel = vel;
    }

    public override void update(float deltaTime){
        this.bullet.transform.Translate(dir * vel * deltaTime);
    }

    public override void start(){
        
    }
}
