using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomGenerator
{
    public static List<int> RandomIntGenerate(this List<int> list, int count){
        if(count > list.Count) return null;
        List<int> res = new();

        List<int> shuffledList = new(list);
        for (int i = 0; i < shuffledList.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledList.Count);
            (shuffledList[i], shuffledList[randomIndex]) = (shuffledList[randomIndex], shuffledList[i]);
        }

        return shuffledList.Take(count).ToList();
    }
}
