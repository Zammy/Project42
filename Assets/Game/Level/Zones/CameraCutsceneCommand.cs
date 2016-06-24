using System.Collections;
using System;

public class CameraCutsceneCommand : ZoneCommand
{
    public bool Activate;

    public override IEnumerator Execute()
    {
        if (Activate)
            InGameUI.Instance.ActivateCutsceneView();
        else
            InGameUI.Instance.DeactivateCutsceneView();

        yield return null;
    }
}
