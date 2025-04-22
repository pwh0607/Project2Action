using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Props/AnswerKeyData")]
public class AnswerKeyData : ScriptableObject
{
    public List<GameObject> key;            //Key prefab
}
