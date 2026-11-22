using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonArrangement : MonoBehaviour
{
	public CanonArrangementData[] canonArrangementDatas;
    public static Action<List<ShipMovementHandler.CanonType>> OnCanonArrangmentChanged;
    [SerializeField]
    public Dictionary<ShipMovementHandler.CanonType, CanonBallHandler> canonBalls;
	IEnumerator Start()
	{
        for (int i = 0; i < canonArrangementDatas.Length; i++)
        {
            canonArrangementDatas[i].canonHandler.index = i;
        }
		yield return new WaitForSeconds(0.05f);
        OnCanonArrangmentChanged?.Invoke(GetListOfCanonTypes());
		yield return null;
    }
    public void SetCanonArrangement(int index, ShipMovementHandler.CanonType newCanonType)
    {
		try
		{
			canonArrangementDatas[index].canontype = newCanonType;
			if (newCanonType != ShipMovementHandler.CanonType.None)
			{
				canonArrangementDatas[index].canonHandler.gameObject.SetActive(true);
				canonArrangementDatas[index].canonHandler.CanonType = newCanonType;
            }
			OnCanonArrangmentChanged?.Invoke(GetListOfCanonTypes());
        }
		catch (System.IndexOutOfRangeException)
		{
			Debug.LogError($"Index {index} is out of range for canontypes array. Array length: {canonArrangementDatas.Length}");
            throw;
		}
    }

	private List<ShipMovementHandler.CanonType> GetListOfCanonTypes()
	{
        List<ShipMovementHandler.CanonType> tempList = new List<ShipMovementHandler.CanonType>(canonArrangementDatas.Length);
        for (int i = 0; i < canonArrangementDatas.Length; i++)
        {
            tempList.Add(canonArrangementDatas[i].canontype);
        }
		return tempList;
    }

	public float CalculateReloadTime(ShipCanonHandler canonHandler)
    {
        int startingIndex = (int)Mathf.Floor(((float)canonHandler.index / 3)) * 3;
        float reloadTime = 1;
        int activeCanons = 1;

        for (int i = startingIndex; i < startingIndex + 3; i++)
        {
            if (canonArrangementDatas[i].canonHandler.gameObject.activeInHierarchy)
            {
                reloadTime += canonBalls[canonArrangementDatas[i].canonHandler.CanonType].reloadTime;
                activeCanons++;
            }
        }
        reloadTime /= activeCanons; // Average reload time of the three canons
        return reloadTime;
    }

    public bool AreCanonsReloading(ShipCanonHandler canonHandler)
    {
        int startingIndex = (int)Mathf.Floor(((float)canonHandler.index / 3)) * 3;
        
        for (int i = startingIndex; i < startingIndex + 3; i++)
        {
            if (canonArrangementDatas[i].canonHandler.gameObject.activeInHierarchy)
            {
                if (canonArrangementDatas[i].canonHandler.isReloading)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
[Serializable]
public struct CanonArrangementData
{
	public string label;
    public ShipMovementHandler.CanonType canontype;
	public ShipCanonHandler canonHandler;
}