using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TransformExtension
{
    public static Transform FindSlot(this Transform root, string slotname){
        List<Transform> children = root.GetComponentsInChildren<Transform>().ToList();
        foreach(Transform t in children)
            if(t.name.ToLower().Contains(slotname.ToLower())) return t;

        Debug.Log($"못 찾음 : {slotname}");
        return null;
    }

    public static Transform FindSlot(this Transform root, params string[] slotnames){
        List<Transform> children = root.GetComponentsInChildren<Transform>().ToList();
        
        foreach(var slot in slotnames)
        {
            foreach(Transform t in children){
                if(t.name.ToLower().Contains(slot.ToLower())) return t;
            }
        }
        
        Debug.Log($"못 찾음 : {slotnames.ToList()}");
        return null;
    }
}