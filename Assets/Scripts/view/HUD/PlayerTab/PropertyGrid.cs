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
    public void setValue(TK key, TV value){
        if(!Keys.Contains(key))
        {
            Debug.LogException(new System.Exception("No Key in Dictionary exception"));
            return;
        }
        Values[Keys.IndexOf(key)] = value;
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
    public Dictionary<TK,TV> toDict()
    {
        Dictionary<TK,TV> dict = new Dictionary<TK, TV>();
        for(int i = 0; i < Keys.Count; i++)
        {
            dict.Add(Keys[i],Values[i]);
        }
        return dict;
    }
}
}
