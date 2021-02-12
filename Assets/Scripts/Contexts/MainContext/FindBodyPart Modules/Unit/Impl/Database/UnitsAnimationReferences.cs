using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitsAnimationReferences", menuName = "ScriptableObjects/UnitsAnimationReferences", order = 1)]
public class UnitsAnimationReferences : ScriptableObject
{
    public UnitAnimationReference[] animationReferences;
}
