using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectHolder : MonoBehaviour
{
    private static ParticleEffectHolder _instance;
    public static ParticleEffectHolder Instance { get { return _instance; } }

    public GameObject coldParticleEffect;
    public GameObject frozenParticleEffect;
    public GameObject hotParticleEffect;
    public GameObject evaporationParticleEffect;
    public GameObject fireParticleEffect;


    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void StartEffect(GameObject effect, GameObject parent)
    {
        var particleObj = GameObject.Instantiate(effect, parent.transform);
        particleObj.transform.SetAsLastSibling();
    }

    public void EndEffect( GameObject parent)
    {
        ParticleSystem particleSystem = parent.GetComponentInChildren<ParticleSystem>();
        if(particleSystem != null)
        {
            GameObject.Destroy(particleSystem.gameObject);
        }
        
    }

    public void EndAllEffects(GameObject parent)
    {
        ParticleSystem[] particleSystems = parent.GetComponentsInChildren<ParticleSystem>();
        if(particleSystems.Length > 0)
        {
            foreach(ParticleSystem system in particleSystems)
            {
                GameObject.Destroy(system.gameObject);
            }

        }
    }
}
