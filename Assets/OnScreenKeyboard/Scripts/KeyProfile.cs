using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Keyboard Profile", menuName = "Keyboard Profile", order = 51)]
public class KeyProfile : ScriptableObject
{
    [SerializeField]
    private List<key> keys;

    public Dictionary<int, key> GetDict()
    {
        var i = 0;
        return keys.ToDictionary(x => i++, x => x);
    }

    [System.Serializable]
    public class key
    {
        public string uppercase;
        public string lowercase;
    }
}
