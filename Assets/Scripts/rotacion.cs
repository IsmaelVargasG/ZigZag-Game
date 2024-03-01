using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class rotacion : MonoBehaviour
{
    public float speed = 75.0f;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime, Space.World);

        if(Vector3.Distance(player.transform.position, transform.position) > 25){
            Destroy(gameObject);
        }
    }
}
