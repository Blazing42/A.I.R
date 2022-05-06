using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AtmosphereUtilities {

    public enum TempValue { COMFORTABLE, HOT, COLD, BURNING, FREEZING}
    
    //methods used to check temperature
    public static void ChangeBodyTemp(Actor creature, float roomTemp)
    {
        float bodyTemp = creature.BodyTemp;
        float tempDifference = bodyTemp - roomTemp;
        if(bodyTemp < roomTemp)
        {
            for (int i = 0; i < tempDifference; i++)
            {
                bodyTemp++;
                CheckTemperature(creature.BodyTemp, creature.MaxComfTemp, creature.MinComfTemp);
            }
        }
        else if (bodyTemp > roomTemp)
        {
            for (int i = 0; i < tempDifference; i++)
            {
                bodyTemp--;
                CheckTemperature(creature.BodyTemp, creature.MaxComfTemp, creature.MinComfTemp);
            }
        }
    }

    public static TempValue CheckTemperature(float roomTemp, float maxComfTemp, float minComfTemp)
    {
        if (roomTemp > maxComfTemp && roomTemp <= maxComfTemp + 15)
        {
            //trigger event/animation when creature is to hot
            //reduce movement speed to half
            //Debug.Log("Too Hot");
            return TempValue.HOT;
        }
        else if (roomTemp > maxComfTemp + 15)
        {
            //trigger dieing of max heat event/ animation
            //Debug.Log("Burning");
            return TempValue.BURNING;
        }
        else if (roomTemp < minComfTemp && roomTemp >= minComfTemp - 15)
        {
            //trigger event/animation when creature is to cold
            //reduce movement speed to half
            //Debug.Log("Too Cold");
            return TempValue.COLD;
        }
        else if (roomTemp < minComfTemp - 15)
        {
            //trigger dieing of min temp event/ animation
            //reduce movement speed to 0
            //Debug.Log("Freezing");
            return TempValue.FREEZING;
        }
        else
        {
            //Debug.Log("comfortable");
            return TempValue.COMFORTABLE;
        }
    }

    /*public static void CheckPressure()
    {
        if (bodyPressure > maxComfPressure && bodyPressure <= maxComfTemp + 1)
        {
            //trigger event/animation when creature is to hot
            Debug.Log("Too high");
        }
        else if (bodyPressure > maxComfPressure + 1)
        {
            //trigger dieing of max heat event/ animation
            Debug.Log("Implosion");
        }
        else if (bodyPressure < minComfPressure && bodyPressure >= minComfPressure - 1)
        {
            //trigger event/animation when creature is to cold
            Debug.Log("Too Low");
        }
        else if (bodyPressure < minComfPressure - 1)
        {
            //trigger dieing of min temp event/ animation
            Debug.Log("Explosion");
        }
        else
        {
            Debug.Log("Comfortable");
        }
    }*/

}
