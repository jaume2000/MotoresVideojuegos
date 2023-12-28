using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(RTDESKEntity))]
public class CircularPattern : BulletPattern
{

    public float generate_time= 3;
    public float phase_shift = 0.1f;
    private float total_phase = 0;
    public float bullet_velocity = 5;
    public int bullet_count = 5;
    public int survival_time = 5;
    
    protected override void start(){
        awaiting_time = generate_time;
    }

    protected override void generate(){


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
}
