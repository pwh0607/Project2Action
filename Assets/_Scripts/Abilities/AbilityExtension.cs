using UnityEngine.Events;

public static class AbilityExtension
{
    // a Ability 보유 여부
    public static bool Has(this ref AbilityFlag abilities, AbilityFlag a)
    {
        return (abilities & a) == a;
    }

    // a Ability 추가
    public static void Add(this ref AbilityFlag abilities, AbilityFlag a, UnityAction onComplete = null){
        abilities |= a;
        onComplete?.Invoke();
    }

    // a Ability 제거
    public static void Remove(this ref AbilityFlag abilities, AbilityFlag a, UnityAction onComplete = null){
        abilities &= ~a;
        onComplete?.Invoke();
    }

    // a Ability 사용 -> 액션 발동.
    public static void Use(this ref AbilityFlag abilities, AbilityFlag a, UnityAction onComplete = null){
        if(abilities.Has(a)){
            onComplete?.Invoke();
        } 
    }
}