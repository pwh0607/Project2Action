using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Props/ButtonKeyData")]
public class ButtonKeyData : ScriptableObject
{
    public List<GameObject> key;
    public GameObject button;
}
