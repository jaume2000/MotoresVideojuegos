using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class movement : MonoBehaviour
{
    public Vector2 dir = new Vector2(0, 0);
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // control player with wasd keys
        dir.x = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
        dir.y = Input.GetKey(KeyCode.S) ? -1 : Input.GetKey(KeyCode.W) ? 1 : 0;
        
        // move player
        transform.Translate(dir.normalized * speed * Time.deltaTime);
    }
}
