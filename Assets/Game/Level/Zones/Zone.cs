using UnityEngine;
using System.Collections;
using System;

public class Zone : MonoBehaviour
{
    public PlayerInput PlayerInput;
    public Transform CommandsBase;
    public ZoneTrigger ZoneTrigger;

    bool triggered = false;

    ZoneCommand[] commands;

    void Start()
    {
        commands = CommandsBase.GetComponentsInChildren<ZoneCommand>();
        //for (int i = 0; i < CommandsBase.childCount; i++)
        //{
        //    var child = CommandsBase.GetChild(i);

        //}

        this.ZoneTrigger.Trigger += OnTrigger;
    }

    void OnDestroy()
    {
        this.ZoneTrigger.Trigger -= OnTrigger;
    }

    private void OnTrigger()
    {
        if (!triggered)
            StartCoroutine(Execute());

        triggered = true;
    }

    IEnumerator Execute()
    {
        this.PlayerInput.enabled = false;

        for (int i = 0; i < commands.Length; i++)
        {
            var cmd = commands[i];
            yield return StartCoroutine(cmd.Execute());
        }

        this.PlayerInput.enabled = true;
    }
}
