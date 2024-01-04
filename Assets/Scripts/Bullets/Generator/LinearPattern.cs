using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearPattern : BulletPattern
{

    public float generate_time= 3;
    public float phase_shift = 0.1f;
    private float total_phase = 0;
    public float bullet_velocity = 5;
    public int bullet_count = 5;
    public int survival_time = 5;

    private Vector3 endpoint_rel_pos;
    private float endpoint_distance;
    private Vector3 bullet_direction;
    
    protected override void start(){
        awaiting_time = generate_time;
        
        if(transform.GetChild(0) == null){
            GameObject child = new GameObject("Endpoint");
            child.transform.SetParent(this.transform);

            endpoint_distance = 3;
            endpoint_rel_pos = Vector3.up*endpoint_distance;
            bullet_direction = Vector3.right;
            child.transform.position = this.transform.position + endpoint_rel_pos;
        }
        else {
            UnityEngine.Transform endpoint = transform.GetChild(0);
            endpoint_rel_pos = endpoint.position - transform.position;
            endpoint_distance = endpoint_rel_pos.magnitude;
            bullet_direction = (new Vector3(endpoint_rel_pos.y, -endpoint_rel_pos.x,0)).normalized;
        }

    }

    protected override void generate(){

        for(int i=0; i<= bullet_count; i++){

            Vector3 rel_pos = i * endpoint_rel_pos/bullet_count + endpoint_rel_pos.normalized * total_phase;
            while(rel_pos.magnitude > endpoint_distance){
                rel_pos -= endpoint_rel_pos;
            }

            GameObject lb = Instantiate(this.bullet_prefabs[0], transform.position + rel_pos, Quaternion.identity);
            BulletGameObject bgo = lb.GetComponent<BulletGameObject>();
            //bgo.setSurvivalTIme(survival_time);

            bgo.setBehaviour(new LinearBullet(lb, bullet_direction, bullet_velocity));
            bgo.setMapBorderMultiplier(mapBorderMultiplier);
        }

        total_phase+=phase_shift;
        total_phase = (total_phase + endpoint_distance) % endpoint_distance;


    }
}
