using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : GOAPAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        GOAPGameWorld.WorldInstance.GetWorld().ModifyStateValue("doorOpen", -1);
        GOAPGameWorld.WorldInstance.GetWorld().ModifyStateValue("doorClosed", 1);
        return true;
    }
}
