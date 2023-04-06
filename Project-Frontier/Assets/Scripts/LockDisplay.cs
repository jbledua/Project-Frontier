using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockDisplay : MonoBehaviour
{
    public PlayerKeys playerKeys;
    public Key.KeyColor keyColor;
    public Sprite keyCollectedSprite;
    public Sprite keyNotCollectedSprite;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerKeys.HasKey(keyColor))
        {
            spriteRenderer.sprite = keyCollectedSprite;
        }
        else
        {
            spriteRenderer.sprite = keyNotCollectedSprite;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
