using UnityEngine;

public class CrystalManager : MonoBehaviour
{
    public static CrystalManager instance;

    public int currentCrystal = 0;

    private void Awake()
    {
        instance = this;
    }

    public void AddCrystal(int amount)
    {
        currentCrystal += amount;
    }
}