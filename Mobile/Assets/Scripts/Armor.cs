using System;
using UnityEngine;

public class Armor : MonoBehaviour
{
    public enum ArmorRate
    {
        Light = 0,
        Medium = 1,
        Heavy = 2
    };
    public ArmorRate ArmorClass;
}
