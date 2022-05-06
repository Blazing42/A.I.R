using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Core : InteractiveObject
{
    [SerializeField] Slider coreUIHealthSlider;
    public int creaturesAttacking = 0;
    bool beingAttacked = false;
    bool dead = false;

    void Start()
    {
        coreUIHealthSlider.maxValue = maxHp;
        coreUIHealthSlider.value = currentHp;
        levelSystem.coreHP = currentHp;
    }

    public override void TakeDamage(int damageAmount)
    {
        if (beingAttacked == false && dead == false)
        {
            StartCoroutine(BeingAttacked());
        }
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.C))
        {
            TakeDamage(20);
        }
    }

    IEnumerator BeingAttacked()
    {
        beingAttacked = true;
        while (creaturesAttacking > 0)
        {
            base.TakeDamage(creaturesAttacking);
            coreUIHealthSlider.value = currentHp;
            levelSystem.coreHP = currentHp;
            if (currentHp <= 0)
            {
                dead = true;
                levelSystem.OpenGameOverScreen();
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
            
        }
        beingAttacked = false; // out of range
        yield return null;
    }
}
