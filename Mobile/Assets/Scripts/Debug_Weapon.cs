using UnityEngine;
using System.Collections;

public class Debug_Weapon : MonoBehaviour {

    public enum WeaponType
    {
        Pistol,
        Shotgun,
        SMG,
        Rifle,
        Burst,
        Sniper,
        HMG,
        None
    };

    public static int Range(WeaponType gun)
    {
            switch (gun)
            {
                case WeaponType.Pistol:
                    return 2;
                case WeaponType.SMG:
                    return 4;
                case WeaponType.Shotgun:
                    return 3;
                case WeaponType.Rifle:
                    return 5;
                case WeaponType.Burst:
                    return 5;
                case WeaponType.Sniper:
                    return 8;
                case WeaponType.HMG:
                    return 6;
                case WeaponType.None:
                    return 0;
                default:
                    return 1;
            }
    }
}
