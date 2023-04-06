using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeys : MonoBehaviour
{
    private Dictionary<Key.KeyColor, bool> keys;

    private void Awake()
    {
        keys = new Dictionary<Key.KeyColor, bool>();
        foreach (Key.KeyColor color in System.Enum.GetValues(typeof(Key.KeyColor)))
        {
            keys[color] = false;
        }
    }

    public void AddKey(Key.KeyColor keyColor)
    {
        keys[keyColor] = true;
    }

    public bool HasKey(Key.KeyColor keyColor)
    {
        return keys[keyColor];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
