using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWorldStateText : MonoBehaviour
{
    public Text statesText;

    //if not assigned in the inspector assign it now for testing
    void Start()
    {
        if(statesText == null)
        {
            statesText = this.GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Dictionary<string, int> worldStates = GOAPGameWorld.WorldInstance.GetWorld().worldStates;
        statesText.text = "";
        foreach(KeyValuePair<string,int> s in worldStates)
        {
            statesText.text += s.Key + " , " + s.Value + "\n";
        }
    }
}
