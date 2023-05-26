using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TurnBasedSystem : MonoBehaviour
{
    [field: Header("References:")]
    [field: SerializeField] public List<Unit> PlayerUnits { get; private set; }
    [field: SerializeField] public List<Unit> EmenyUnits { get; private set; }
    [field: SerializeField] public Transform UIList { get; private set; }
    [field: SerializeField] public Image ImagePrefab { get; private set; }
    
    [field: Header("Settings:")]
    [field: SerializeField] [field: Range(150,250)] public int FirstCircle { get; private set; }
    [field: SerializeField] [field: Range(200, 300)] public int Circle { get; private set; }

    public PlayerInputUltility Input { get; private set; }

    private PriorityQueue<Unit> Queue = new PriorityQueue<Unit>();
    private List<Unit> AllUnits;
    private Dictionary<Unit, Image> DictionaryUnit;

    private void Awake()
    {
        Input = GetComponent<PlayerInputUltility>();
    }

    private void Start()
    {
        Init();
    }

    #region Main Methods
    private void Init()
    {
        AllUnits = new List<Unit>();
        DictionaryUnit = new Dictionary<Unit, Image>();

        //Add Character Units
        if (PlayerUnits.Count > 0) AllUnits.AddRange(PlayerUnits);
        //Add Enemey Units
        if (EmenyUnits.Count > 0) AllUnits.AddRange(EmenyUnits);
        
        //Create First turn for all Units
        foreach (Unit unit in AllUnits)
        {
            CreateNewTurn(unit, FirstCircle);
            CreateImage(unit, FirstCircle - unit.Speed);
        }
       
        UpdateTurnAll(Queue.Peek().Item2);
        
        ShowQueue();

        StartBattle();
    }

    private void StartBattle()
    {
        var Unit = Queue.Peek();

        if(Unit.Item1 is ICharacter)
        {
            AddCharacterInput();
        }
        else
        {
            FinishTurn();
        }
    }

    private void FinishTurn()
    {
        RemoveCharacterInput();

        Unit Unit = Queue.Dequeue().Item1;

        CreateNewTurn(Unit, Circle);

        UpdateTurnAll(Queue.Peek().Item2);

        ShowQueue();
        
        StartBattle();
    }

    //Add turn in queue
    private void CreateNewTurn(Unit unit, int circle)
    {
        Queue.Enqueue(unit, circle - unit.Speed);
    }

    //Update turn for all units
    private void UpdateTurnAll(int number)
    {
        Queue.UpdatePriorityAll(number);
    }


    private void CreateImage(Unit unit, int priorityValue)
    {
        Image image = Instantiate(ImagePrefab, UIList);
        image.sprite = unit.Image;
        image.GetComponentInChildren<TMP_Text>().text = priorityValue.ToString();
        image.GetComponent<TurnUnit>().Unit = unit;

        DictionaryUnit[unit] = image;
    }

    
    //Show Queue on UI
    private void ShowQueue()
    {
        for (int i = 0; i < Queue.Count; i++)
        {
            DictionaryUnit[Queue.ListQueue[i].Item1].transform.SetSiblingIndex(FindUnitIndex(Queue.ListQueue[i].Item1));
            DictionaryUnit[Queue.ListQueue[i].Item1].GetComponentInChildren<TMP_Text>().text = Queue.ListQueue[i].Item2.ToString();
        }
    }

    private int FindUnitIndex(Unit unit)
    {
        Queue.SoftQueue();
        return Queue.ListQueue.Find((x) => x.Item1 == unit).Item2;
    }
    #endregion

    #region Character Methods
    private void AddCharacterInput()
    {
        Input.PlayerActions.Skill.performed += PlayerSkill;
        Input.PlayerActions.Ultimate.performed += PlayerUltimate;
    }

    private void RemoveCharacterInput()
    {
        Input.PlayerActions.Skill.performed -= PlayerSkill;
        Input.PlayerActions.Ultimate.performed -= PlayerUltimate;
    }

    #endregion

    #region Enemy Methods

    #endregion

    #region Callback Methods
    private void PlayerSkill(InputAction.CallbackContext context)
    {
        ICharacter Character = (ICharacter)Queue.Peek().Item1;
        Character.Skill();
        
        //Test
        FinishTurn();
    }

    private void PlayerUltimate(InputAction.CallbackContext context)
    {
        ICharacter Character = (ICharacter)Queue.Peek().Item1;
        Character.Ultimate();

        //Test
        FinishTurn();
    }
    #endregion
}
