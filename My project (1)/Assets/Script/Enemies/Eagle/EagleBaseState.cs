using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EagleBaseState
{
    public abstract void EnterState(Eagle eagle);
    public abstract void UpdateState(Eagle eagle);
    public abstract void ExitState(Eagle eagle);
}
