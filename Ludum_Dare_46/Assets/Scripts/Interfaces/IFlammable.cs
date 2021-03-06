﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlammable
{
    bool isOnFire { get; }

    void RegisterFlammable();
    void UnRegisterFlammable();
}
