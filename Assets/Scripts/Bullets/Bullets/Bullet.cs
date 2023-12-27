using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet
{
    protected GameObject bullet;

    public Bullet(GameObject bullet){
        this.bullet = bullet;
    }

    public abstract void update(float deltaTime);
    public abstract void start();
}
