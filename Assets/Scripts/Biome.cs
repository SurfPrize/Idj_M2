using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome
{
    public List<GameObject> assets;
    public string name;

    public Biome(string name,List<GameObject> assets)
    {
        this.name = name;
        this.assets = assets;
    }


}
