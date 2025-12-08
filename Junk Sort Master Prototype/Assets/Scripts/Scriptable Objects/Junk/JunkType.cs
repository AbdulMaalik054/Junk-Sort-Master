using UnityEngine;

[CreateAssetMenu(menuName = "Junk/JunkType")]
public class JunkType : ScriptableObject
{
    public string typeName;
    public float speedValue = 2.0f;
    public Color highlightColor = Color.white;
}
