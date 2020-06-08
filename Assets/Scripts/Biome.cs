using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome : MonoBehaviour
{
    public List<GameObject> assets;
    public string Biomename;
    public List<BiomeExtender> Extensions;

    public void ChangeBiome(string name, List<GameObject> assets)
    {
        Biomename = name;
        this.assets = assets;
    }

    public void ChangeBiome(Biome novo)
    {
        assets = novo.assets;
        Biomename = novo.Biomename;
    }

    public void DistributePoints(int amount, float biomesize)
    {
        List<BiomeExtender> result = new List<BiomeExtender>();
        float angle = Mathf.PI * 2 / amount;
        for (int i = 0; i < amount; i++)
        {
            GameObject NovoPonto = new GameObject();

            NovoPonto.AddComponent<BiomeExtender>().Move(new Vector2(transform.position.x + Mathf.Cos(angle * i) * Random.Range(biomesize / 3, biomesize),
                transform.position.z + Mathf.Sin(angle * i) * Random.Range(biomesize / 3, biomesize)));
            NovoPonto.name = "Ponto " + (i + 1);
            NovoPonto.GetComponent<BiomeExtender>().ChangeBiome(this.GetComponent<Biome>());
            NovoPonto.transform.parent = this.gameObject.transform;
            result.Add(NovoPonto.GetComponent<BiomeExtender>());
            //result.Add(new BiomeExtender(new Vector2(transform.position.x + Mathf.Cos(angle) * Random.Range(biomesize / 3, biomesize),
            //    transform.position.x + Mathf.Sin(angle) * Random.Range(biomesize / 3, biomesize))));
        }
        Extensions = result;
    }

    public void Show_extensions()
    {
        foreach (BiomeExtender este in Extensions)
        {
            este.Show_debug();
        }

        Debug.DrawLine(Extensions[0].transform.position, Extensions[Extensions.Count - 1].transform.position);
        for (int i = 0; i < Extensions.Count - 1; i++)
        {

            Debug.DrawLine(Extensions[i].transform.position, Extensions[i + 1].transform.position);

        }
    }
}
