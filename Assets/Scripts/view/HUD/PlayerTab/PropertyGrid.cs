using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
public class PropertyGrid : MonoBehaviour
{
    [SerializeField] public DictionaryWrapper<int,PropertyToggle> propertyToggles;
}
[System.Serializable]
public class DictionaryWrapper<TK,TV>
{
    public List<TK> Keys;
    public List<TV> Values;

    public void add(TK key, TV value)
    {
        if(Keys.Contains(key))
        {
            Debug.LogException(new System.Exception("Key duplication exception"));
            return;
        }
        Keys.Add(key);
        Values.Add(value);
    }
    public TV getValue(TK key){
        if(!Keys.Contains(key))
        {
            Debug.LogException(new System.Exception("No Key in Dictionary exception"));
        }
        return Values[Keys.IndexOf(key)];
    }

    public void remove(TK key)
    {
        if(!Keys.Contains(key))
        {
            Debug.LogException(new System.Exception("No Key in Dictionary exception"));
            return;
        }
    int ind = Keys.IndexOf(key);
    Keys.Remove(key);
    Values.RemoveAt(ind);
    }
}
}
