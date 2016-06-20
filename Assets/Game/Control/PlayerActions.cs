using UnityEngine;
using System.Collections;
using InControl;

public class PlayerActions : PlayerActionSet
{
    public PlayerAction Skill1;

    public PlayerAction MoveUp;
    public PlayerAction MoveDown;
    public PlayerAction MoveLeft;
    public PlayerAction MoveRight;

    public PlayerTwoAxisAction Move;

//    public PlayerAction AimLeft;
//    public PlayerAction AimRight;
//    public PlayerAction AimUp;
//    public PlayerAction AimDown;
//    public PlayerTwoAxisAction Aim;

    public PlayerActions()
    {
        Skill1 = CreatePlayerAction("Skill1");

        MoveUp = CreatePlayerAction("Move Up");
        MoveDown = CreatePlayerAction("Move Down");
        MoveLeft = CreatePlayerAction("Move Left");
        MoveRight = CreatePlayerAction("Move Right");

        Move =  CreateTwoAxisPlayerAction(MoveLeft, MoveRight, MoveDown, MoveUp);

//        AimLeft = CreatePlayerAction("Aim Left");
//        AimRight = CreatePlayerAction("Aim Right");
//        AimUp = CreatePlayerAction("Aim Up");
//        AimDown = CreatePlayerAction("Aim Down");
//        Aim = CreateTwoAxisPlayerAction(AimLeft, AimRight, AimDown, AimUp);
    }


    public static PlayerActions CreateWithDefaultBindings()
    {
        var playerActions = new PlayerActions();

        playerActions.Skill1.AddDefaultBinding(InputControlType.Action1);
        //playerActions.Fire.AddDefaultBinding(Mouse.LeftButton);

        //playerActions.Jump.AddDefaultBinding(Key.W);
        //playerActions.Jump.AddDefaultBinding(Key.Space);

        //move
        //playerActions.MoveLeft.AddDefaultBinding(Key.A);
        //playerActions.MoveRight.AddDefaultBinding(Key.D);
        //playerActions.MoveLeft.AddDefaultBinding(Key.LeftArrow);
        //playerActions.MoveRight.AddDefaultBinding(Key.RightArrow);

        playerActions.MoveUp.AddDefaultBinding(InputControlType.LeftStickUp);
        playerActions.MoveDown.AddDefaultBinding(InputControlType.LeftStickDown);
        playerActions.MoveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
        playerActions.MoveRight.AddDefaultBinding(InputControlType.LeftStickRight);

        //aim
//        playerActions.AimLeft.AddDefaultBinding(InputControlType.RightStickLeft);
//        playerActions.AimRight.AddDefaultBinding(InputControlType.RightStickRight);
//        playerActions.AimUp.AddDefaultBinding(InputControlType.RightStickUp);
//        playerActions.AimDown.AddDefaultBinding(InputControlType.RightStickDown);

        //playerActions.AimUp.AddDefaultBinding(Mouse.PositiveY);
        //playerActions.AimDown.AddDefaultBinding(Mouse.NegativeY);
        //playerActions.AimLeft.AddDefaultBinding(Mouse.NegativeX);
        //playerActions.AimRight.AddDefaultBinding(Mouse.PositiveX);

        playerActions.ListenOptions.IncludeUnknownControllers = true;
        playerActions.ListenOptions.MaxAllowedBindings = 4;
        //          playerActions.ListenOptions.MaxAllowedBindingsPerType = 1;
        //          playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;
        //          playerActions.ListenOptions.IncludeMouseButtons = true;

        playerActions.ListenOptions.OnBindingFound = (action, binding) =>
        {
            if (binding == new KeyBindingSource(Key.Escape))
            {
                action.StopListeningForBinding();
                return false;
            }
            return true;
        };

        playerActions.ListenOptions.OnBindingAdded += (action, binding) =>
        {
            Debug.Log("Binding added... " + binding.DeviceName + ": " + binding.Name);
        };

        playerActions.ListenOptions.OnBindingRejected += (action, binding, reason) =>
        {
            Debug.Log("Binding rejected... " + reason);
        };

        return playerActions;
    }
}