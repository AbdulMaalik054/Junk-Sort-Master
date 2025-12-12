using UnityEngine;

[CreateAssetMenu(menuName = "Game/Junk Type", fileName = "NewJunkType")]
public class JunkType : ScriptableObject
{

    public string typeName;                  // Identifier (ex: "PaperA")
    public GameObject[] prefabVariants;          // Prefab to pool

   

    [Header("Pool Size")]
    public int initialSize = 2;

    
    
}
