using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BiomeLoader : MonoBehaviour
{
    [SerializeField]
    public List<Biome> All_Biomes=new List<Biome>();

    private void LoadBiomes()
    {
        foreach (Object obj in Resources.LoadAll(""))
        {
            
            string biome_name = obj.name.Split('_').Last();
            if (All_Biomes.Find(x => x.name == biome_name) == null&& obj.GetType()== typeof(GameObject))
            {
                List<GameObject> todos = Resources.LoadAll(biome_name, typeof(GameObject))
                    .Cast<GameObject>().ToList();
                Biome este = new Biome(biome_name, todos);
                Debug.Log(obj.name + " " + todos.Count);
                All_Biomes.Add(este);
            }
        }
        Resources.UnloadUnusedAssets();
    }

    private void Start()
    {
        LoadBiomes();
    }
}
