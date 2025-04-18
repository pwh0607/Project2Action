using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomGenerator
{
    /*
        public static T Random<T>(this List<T> list){
            int rnd = UnityEngine.Random.Range(0, list.Count);
            return list[rnd];
        }
    */
    public static List<int> RandomIntGenerate(this int max, int count){
        if(count > max) return null;
        List<int> list = new();

        for(int i=0;i<max;i++){
            list.Add(i);
        }

        List<int> shuffledList = new(list);
        for (int i = 0; i < shuffledList.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledList.Count);
            (shuffledList[i], shuffledList[randomIndex]) = (shuffledList[randomIndex], shuffledList[i]);
        }

        return shuffledList.Take(count).ToList();
    }
}
