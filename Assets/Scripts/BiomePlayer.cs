using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BiomePlayer : MonoBehaviour
{
    public static BiomePlayer BPlayer;
    public List<BiomeExtender> closest;
    public List<BiomeObj> current_Biome;
    public bool debug = false;

    private void Awake()
    {
        if (BPlayer != null)
        {
            Destroy(this);
        }
        else
        {
            BPlayer = this;
        }
        StartCoroutine(Update_Closest());
    }

    public List<BiomeExtender> Get_Closest(int size)
    {
        List<BiomeExtender> result = new List<BiomeExtender>();
        foreach (BiomeBehaviour este in BiomeManager.BManager.BiomeCenters)
        {
            result.AddRange(este.Extensions);
        }
        result.Sort(delegate (BiomeExtender b1, BiomeExtender b2)
        {

            return Vector3.Distance(BPlayer.gameObject.transform.position, b1.gameObject.transform.position).CompareTo(Vector3.Distance(BPlayer.gameObject.transform.position, b2.gameObject.transform.position));

        });
        result = result.Take(size).ToList();
        return result;
    }

    public IEnumerator Update_Closest()
    {
        yield return new WaitForSeconds(0.2f);
        closest = Get_Closest(3);
        foreach (BiomeExtender este in closest)
        {
            if (!current_Biome.Contains(este.currentbiome))
                current_Biome.Add(este.currentbiome);
        }
        StartCoroutine(Update_Closest());
    }

    private void Update()
    {
        if (debug)
        {
            foreach (BiomeExtender este in closest)
            {
                Debug.DrawLine(este.transform.position, transform.position, Color.yellow);
            }
        }
    }
}
