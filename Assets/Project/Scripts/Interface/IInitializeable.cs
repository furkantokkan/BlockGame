using UnityEngine;

public interface IInitializeable 
{
    public int Priority();
    public void Initialize(int piceAmount, int gridSize, Vector3 newSpawnPos);
}
