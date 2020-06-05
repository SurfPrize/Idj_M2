using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BiomeManager : MonoBehaviour
{
    [SerializeField]
    public List<Biome> All_Biomes = new List<Biome>();
    public List<Biome> BiomeCenters = new List<Biome>();
    public GameObject prefab;
    [SerializeField]
    [Range(20f, 50f)]
    private float MaxBiomeSize = 20;
    [SerializeField]
    [Range(10f, 30f)]
    private float MaxBiomeDistance = 20;

    private float _minBiomeDistance;
    [SerializeField]
    private float MinBiomeDistance
    {
        get => _minBiomeDistance; set
        {

            if (value > MaxBiomeDistance)
            {
                _minBiomeDistance = MaxBiomeSize*0.9f;
            }
            else
            {
                _minBiomeDistance = value;
            }
        }
    }
    private float MinDistanceFromOthers;


    private void LoadBiomes()
    {
        List<GameObject> all = Resources.LoadAll("", typeof(GameObject)).Cast<GameObject>().ToList();
        foreach (GameObject obj in all)
        {

            string biome_name = obj.name.Split('_').Last();
            if (All_Biomes.Count == 0 || All_Biomes.Find(x => x.name == biome_name) == null)
            {
                GameObject tipo = new GameObject(biome_name);
                List<GameObject> todos = Resources.LoadAll(biome_name, typeof(GameObject))
                    .Cast<GameObject>().ToList();
                tipo.AddComponent<Biome>();
                tipo.GetComponent<Biome>().ChangeBiome(biome_name, todos);
                Debug.Log(obj.name + " " + todos.Count);
                All_Biomes.Add(tipo.GetComponent<Biome>());
                tipo.SetActive(false);
            }
        }
        Resources.UnloadUnusedAssets();
    }

    private void Start()
    {
        LoadBiomes();
        SpawnFirstBiomes(3);
        SpreadAllCenters(20);
        MinDistanceFromOthers = MaxBiomeSize * 1.5f;
        MinBiomeDistance = 10f;
    }

    private void SpawnFirstBiomes(int quantity)
    {

        for (int i = 0; i < quantity; i++)
        {
            GameObject Next = Instantiate(prefab);
            Next.GetComponent<Biome>().ChangeBiome(All_Biomes[Random.Range(0, All_Biomes.Count)]);
            BiomeCenters.Add(Next.GetComponent<Biome>());
        }
    }

    public void SpreadAllCenters(float max_distance)
    {
        foreach (Biome este in BiomeCenters)
        {
            MoveCenter(este);
            while (BiomeCenters.Find(x => Vector3.Distance(este.gameObject.transform.position, x.gameObject.transform.position) < MinDistanceFromOthers) != null)
            {
                Debug.Log("ola");
                MoveCenter(este);
            }


        }
    }

    public void MoveCenter(Biome este)
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        float dist = Random.Range(MinBiomeDistance, MaxBiomeDistance);
        este.gameObject.transform.position =
            new Vector3(PlayerMovement.current.transform.position.x + Mathf.Cos(angle) * dist,
            0,
            PlayerMovement.current.transform.position.z + Mathf.Sin(angle) * dist);
    }
}
