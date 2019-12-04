using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentMeleeManager : MonoBehaviour
{
    public enum MeleeType { Sword, Spear, Daggers }

    public MeleeType equipedMelee;

    public void ChangeCurrentMelee(MeleeType meleeType)
    {
        // Drop old Melee
        equipedMelee = meleeType;
    }
    
}
