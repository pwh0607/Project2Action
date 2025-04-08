using System.Collections.Generic;
using UnityEngine;

public static class RandomGenerator
{
    public static List<int> RandomIntGenerator(this List<int> list, int count){
        List<int> res = new();
        for(int i = 0; i < count; i++){
            int rnd = Random.Range(0, list.Count);
            int k;
            do{
                k = list[rnd];
            }while(res.Contains(k));
            res.Add(k);
        }
        return res;
    }
}
