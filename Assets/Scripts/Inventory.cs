using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	City city;
	UIController uIController;

	public int cash = 0;
	public float food = 0;

	public bool work = true;
	
	private void Start()
	{
		city = FindObjectOfType<City>();
		uIController = FindObjectOfType<UIController>();
	}

	public void AddFood(float amount)
	{
		if (city.Cash < 0)
		{
			amount = amount - (amount * 70 / 100);
			city.Food += amount;
		}
		else
		{
			city.Food += amount;

		}
		
		uIController.UpdateCityDataGOAP();
	}

	public void DecreaseFood(float amount)
	{
		city.Food -= amount;
		uIController.UpdateCityDataGOAP();
	}

	public void AddCash(float amount)
	{
		if (city.Food < 0f)
		{
			amount = amount - (amount * 70 / 100);
			city.Cash += amount;
		}
		else
		{
			city.Cash += amount;

		}
		
		uIController.UpdateCityDataGOAP();
	}

	public void DecreaseCash(float amount)
	{
		city.Cash -= amount;
		uIController.UpdateCityDataGOAP();
	}
}
