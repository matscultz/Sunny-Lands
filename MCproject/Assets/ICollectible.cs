using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface ICollectible
{
    public void Collect()
    {
        Debug.Log("Coin collected!");
    }
}
