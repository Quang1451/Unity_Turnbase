using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public bool IsDead { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }

    public virtual void CreateNewTurn()
    {

    }

    public virtual void UpdateTurn(int number)
    {

    }
}
