using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Pooling/Pool Database", fileName = "JunkPoolDatabase")]
public class JunkDatabase : ScriptableObject
{
   
    public List<JunkType> items;
}
