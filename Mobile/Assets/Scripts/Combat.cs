using UnityEngine;
using System;
using System.Collections;

public static class Combat {
    
    public static void Fight(Player p, Enemy e)
    {
        int rolls = p.weapon.Rolls;
        int[] AtkRolls = new int[rolls];
        int[] DefRolls = new int[rolls];

        for (int i= 0; i < rolls; i++)
        {
            AtkRolls[i] = UnityEngine.Random.Range(1, p.weapon.DSize+1);
            DefRolls[i] = UnityEngine.Random.Range(1, (p.weapon.DSize + 1) + (2 * (int)e.armor.ArmorClass));
        }
        Array.Sort(AtkRolls);
        Array.Sort(DefRolls);
        Array.Reverse(AtkRolls);
        Array.Reverse(DefRolls);

        Print(AtkRolls);
        Print(DefRolls);
    }

    public static void Print(int[] array)
    {
        Debug.Log("Combat Rolls:");
        for (int i= 0; i < array.Length; i++)
        {
            Debug.Log(array[i]);
        }
    }
}
