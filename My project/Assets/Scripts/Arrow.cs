using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject arrow;
    public Transform arrowPos;
    private GameObject player;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 5)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
            timer = 0;
            shoot();
            }
        }

        
    }
    void shoot()
    {
        Instantiate(arrow, arrowPos.position, Quaternion.identity);
    } 
}
