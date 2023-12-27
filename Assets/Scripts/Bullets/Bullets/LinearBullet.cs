using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearBullet : Bullet
{

    private float dir_x, dir_y, vel;
    
    public LinearBullet(GameObject obj, float dir_x, float dir_y, float vel) : base(obj){
        
        this.dir_x = dir_x;
        this.dir_y = dir_y;
        this.vel = vel;
    }

    public override void update(float deltaTime){
        this.bullet.transform.Translate(new Vector2(dir_x,dir_y) * vel * deltaTime);
    }

    public override void start(){
        
    }
}
