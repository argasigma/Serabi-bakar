using UnityEngine;

public class CrystalManager : MonoBehaviour
{
    public static CrystalManager Instance;

    public int crystalCount = 0;
    public int requiredCrystals = 3;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddCrystal()
    {
        crystalCount++;
        Debug.Log("Crystal didapat! Total: " + crystalCount);
    }

    public bool HasEnoughCrystals()
    {
        return crystalCount >= requiredCrystals;
    }
}