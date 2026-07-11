using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(FightUnit))]
public class FightUnitClickHandler : MonoBehaviour, IPointerClickHandler
{
    private FightUnit fightUnit;

    public event Action<FightUnit> Clicked;

    private void Awake()
    {
        fightUnit = GetComponent<FightUnit>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (fightUnit == null || !fightUnit.IsAlive)
        {
            return;
        }

        Clicked?.Invoke(fightUnit);
    }
}