using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeExtender : Biome
{
    private Vector2 pos;

    
    public void Move(Vector2 pos)
    {
        this.pos = pos;
        this.transform.position = new Vector3(pos.x, 0, pos.y);
    }

    internal void Show_debug()
    {
        Debug.DrawLine(transform.position, transform.parent.position, Color.blue);
    }

}
