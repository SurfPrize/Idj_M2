using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeExtender : Biome
{
    private Vector2 pos;

    public BiomeExtender(Vector2 pos) : base()
    {
        this.pos = pos;
    }
    
    internal void Show_debug()
    {
        Debug.DrawRay(new Vector3(pos.x, 0, pos.y), transform.position, Color.red);
    }

}
