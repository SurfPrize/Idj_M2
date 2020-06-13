
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "Biome")]
public class BiomeObj : ScriptableObject
{
    public List<GameObject> assets;
    public string nome;
    public Color Sky_color;
    public ParticleSystem particles;
}
