using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStatus : MonoBehaviour
{

    public int x;
    public int y;

    int[,] blocks;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject map = GameObject.FindGameObjectWithTag("Map");
        blocks = map.GetComponent<MapStatus>().getBlocks();
        if (blocks[x, y] == 0)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }
    }
}
