﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class UIEvents
{
    public class ActionButtonClickedEvent : UnityEvent<ECharacterActions> { }

    public static ActionButtonClickedEvent actionButtonClickedEvent { get; } = new ActionButtonClickedEvent();
}