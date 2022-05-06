using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerSystem : MonoBehaviour
{
    List<SpriteRenderer> renderers;

    private static SpriteLayerSystem _instance;
    public static SpriteLayerSystem Instance { get { return _instance; } }


    void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        renderers = new List<SpriteRenderer>();
        renderers.AddRange(FindObjectsOfType<SpriteRenderer>());
    }

    // Update is called once per frame
    void Update()
    {
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.sortingOrder = (int)(renderer.transform.position.y * -200);
        }
    }

    public void AddSpriteToArray(SpriteRenderer spriteRenderer)
    {
        renderers.Add(spriteRenderer);
    }

    public void AddSpritesToArray(SpriteRenderer[] spriteRenderers)
    {
        renderers.AddRange(spriteRenderers);
    }

    public void RemoveSpriteFromArray(SpriteRenderer spriteRenderer)
    {
        renderers.Remove(spriteRenderer);
    }
}
