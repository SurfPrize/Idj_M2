using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class BiomeBehaviour : MonoBehaviour
{
    public BiomeObj currentbiome;
    public List<BiomeExtender> Extensions;
    public int decorations;

    public void ChangeBiome(BiomeObj novo)
    {
        decorations = BiomeManager.BManager.amount_decoration;
        currentbiome = novo;
    }


    public void DistributePoints(int amount, float biomesize)
    {
        List<BiomeExtender> result = new List<BiomeExtender>();
        float angle = Mathf.PI * 2 / amount;
        for (int i = 0; i < amount; i++)
        {
            GameObject NovoPonto = new GameObject();

            NovoPonto.AddComponent<BiomeExtender>().Move(new Vector2(transform.position.x + Mathf.Cos(angle * i) * UnityEngine.Random.Range(biomesize / 3, biomesize),
                transform.position.z + Mathf.Sin(angle * i) * UnityEngine.Random.Range(biomesize / 3, biomesize)));
            NovoPonto.name = "Ponto " + (i + 1);
            NovoPonto.GetComponent<BiomeExtender>().ChangeBiome(currentbiome);
            NovoPonto.transform.parent = this.gameObject.transform;
            result.Add(NovoPonto.GetComponent<BiomeExtender>());
            //result.Add(new BiomeExtender(new Vector2(transform.position.x + Mathf.Cos(angle) * Random.Range(biomesize / 3, biomesize),
            //    transform.position.x + Mathf.Sin(angle) * Random.Range(biomesize / 3, biomesize))));
        }
        Extensions = result;
        Decorate(decorations);
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

    public void Decorate(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject novo = Instantiate(currentbiome.assets[UnityEngine.Random.Range(0, currentbiome.assets.Count - 1)]);
            float angle = Random.Range(0, Mathf.PI * 2);
            float dist = Random.Range(0, BiomeManager.BManager.MaxBiomeSize);
            novo.gameObject.transform.position =
                new Vector3(transform.position.x + Mathf.Cos(angle) * dist,
                0,
                transform.position.z + Mathf.Sin(angle) * dist);
            novo.transform.parent = gameObject.transform;
        }
    }

}
