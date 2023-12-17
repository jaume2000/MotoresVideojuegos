using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircularPattern : BulletPattern
{

    public float awaiting_time= 3;
    public float bullet_velocity = 5;
    public int bullet_count = 5;

    protected float time_transcured = 0;

    // Update is called once per frame


    public void genertate(){

        for(int i=0; i< bullet_count; i++){
            GameObject lb = Instantiate(this.bullet_prefabs[0], transform.position, Quaternion.identity);
            BulletGameObject bgo = lb.GetComponent<BulletGameObject>();
            float angle = 2*Mathf.PI/bullet_count * i;
            bgo.setBehaviour(new LinearBullet(lb, Mathf.Cos(angle), Mathf.Sin(angle), bullet_velocity));
        }

    }

    void Update()
    {
        if (time_transcured < awaiting_time){
            time_transcured += Time.deltaTime;
        }
        else {
            genertate();
            time_transcured = 0;
        }
    }
}
