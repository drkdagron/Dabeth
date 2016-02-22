using UnityEngine;

public static class Spawner
{
    public static void SpawnSmoke(Vector3 position, float Range)
    {
        float xRange = Random.Range(0, Range);
        float yRange = Random.Range(0, Range);
        GameObject obj = GameObject.Instantiate(Resources.Load("Assets/Prefabs/Smoke"), position + new Vector3(xRange, yRange, -0.25f), Quaternion.identity) as GameObject;
    }
    public static void SpawnExplosion(Vector3 position)
    {
        GameObject obj = GameObject.Instantiate(Resources.Load("Assets/Prefabs/Explosion"), position, Quaternion.identity) as GameObject;
    }
}
