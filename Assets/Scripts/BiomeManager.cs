using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BiomeManager : MonoBehaviour
{
    [SerializeField]
    public List<BiomeObj> All_Biomes = new List<BiomeObj>();
    public List<BiomeBehaviour> BiomeCenters = new List<BiomeBehaviour>();
    [Range(1, 50)]
    public int amount_decoration = 5;
    public GameObject prefab;

    public static BiomeManager BManager;
    [Range(200f, 500f)]
    public float MaxBiomeSize = 200;
    [SerializeField]
    [Range(100f, 800f)]
    private float MaxBiomeDistance = 400;

    private float _minBiomeDistance;
    [SerializeField]
    private float MinBiomeDistance
    {
        get => _minBiomeDistance;
        set
        {

            if (value > MaxBiomeDistance)
            {
                _minBiomeDistance = MaxBiomeSize * 0.9f;
            }
            else
            {
                _minBiomeDistance = value;
            }
        }
    }
    private float _MinDistanceFromOthers;
    public float MinDistanceFromOthers
    {
        get
        {
            if (_MinDistanceFromOthers == 0)
            {
                _MinDistanceFromOthers = MaxBiomeSize * 1.2f;
                return _MinDistanceFromOthers;
            }
            else
            {
                return _MinDistanceFromOthers;
            }
        }
        set
        {
            _MinDistanceFromOthers = value;
        }
    }


    //private void LoadBiomesOld()
    //{
    //    List<GameObject> all = Resources.LoadAll("", typeof(GameObject)).Cast<GameObject>().ToList();
    //    foreach (GameObject obj in all)
    //    {

    //        string biome_name = obj.name.Split('_').Last();
    //        if (All_Biomes.Count == 0 || All_Biomes.Find(x => x.name == biome_name) == null)
    //        {
    //            GameObject tipo = new GameObject(biome_name);
    //            List<GameObject> todos = Resources.LoadAll(biome_name, typeof(GameObject))
    //                .Cast<GameObject>().ToList();
    //            tipo.AddComponent<BiomeBehaviour>();
    //            tipo.GetComponent<BiomeBehaviour>().ChangeBiome(biome_name, todos);
    //            // Debug.Log(obj.name + " " + todos.Count);
    //            All_Biomes.Add(tipo.GetComponent<BiomeObj>());
    //            tipo.SetActive(false);
    //        }
    //    }
    //    Resources.UnloadUnusedAssets();
    //}

    private void LoadBiomes()
    {
        List<BiomeObj> all = Resources.LoadAll("", typeof(BiomeObj)).Cast<BiomeObj>().ToList();
        All_Biomes = all;
    }
    private void Start()
    {
        LoadBiomes();
        SpawnFirstBiomes(3);
        SpreadAllCenters(20);
        MinBiomeDistance = 100f;
        MinDistanceFromOthers = MaxBiomeSize * 1.5f;

        if (MaxBiomeDistance < MaxBiomeSize * 1.5f)
        {
            Debug.LogWarning("A distancia maxima dos biomas ate ao jogador e menor que a distancia entre biomas");
            MinDistanceFromOthers = MaxBiomeDistance * 0.5f;
        }
    }
    private void Awake()
    {
        if (BManager == null)
        {
            BManager = this;
        }
        else
        {
            Destroy(this);
        }
        StartCoroutine(Check_Distance());
    }
    private void SpawnFirstBiomes(int quantity)
    {

        for (int i = 0; i < quantity; i++)
        {
            GameObject Next = Instantiate(prefab);
            Next.GetComponent<BiomeBehaviour>().ChangeBiome(All_Biomes[Random.Range(0, All_Biomes.Count)]);
            BiomeCenters.Add(Next.GetComponent<BiomeBehaviour>());
        }
    }

    public void SpreadAllCenters(float max_distance)
    {
        foreach (BiomeBehaviour este in BiomeCenters)
        {
            MoveCenter(este);
            Seperate_centers(este);

        }
    }

    public void Seperate_centers(BiomeBehaviour este)
    {
        foreach (BiomeBehaviour outro in BiomeCenters)
        {
            if (este != outro && Vector3.Distance(este.transform.position, outro.transform.position) < MinDistanceFromOthers)
            {
                // Debug.Log("Too close!");
                MoveCenter(este);
                Seperate_centers(este);
                break;
            }
            else if (este != outro)
            {
                // Debug.Log(Vector3.Distance(este.transform.position, outro.transform.position) + " " + MinDistanceFromOthers);
            }
        }
    }

    public void MoveCenter(BiomeBehaviour este)
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        float dist = Random.Range(MinBiomeDistance, MaxBiomeDistance);
        este.gameObject.transform.position =
            new Vector3(PlayerMovement.current.transform.position.x + Mathf.Cos(angle) * dist,
            0,
            PlayerMovement.current.transform.position.z + Mathf.Sin(angle) * dist);

        este.DistributePoints(8, Random.Range(MaxBiomeSize / 2, MaxBiomeSize));
    }

    public IEnumerator Check_Distance()
    {
        yield return new WaitForSeconds(1);
        foreach (BiomeBehaviour este in BiomeCenters)
        {
            if (Vector3.Distance(este.transform.position, BiomePlayer.BPlayer.transform.position) > MaxBiomeDistance)
            {
                MoveCenter(este);
                Seperate_centers(este);
                este.ChangeBiome(All_Biomes[Random.Range(0, All_Biomes.Count - 1)]);
            }
        }
        StartCoroutine(Check_Distance());
    }
    private void Update()
    {
        foreach (BiomeBehaviour este in BiomeCenters)
        {
            este.Show_extensions();
        }

    }

    
}
