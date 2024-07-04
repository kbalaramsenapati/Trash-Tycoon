using System;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
	public string SAVE_KEY = "Player progress";

	public Progress playerProgress;

	private void Awake()
	{
        LoadProgress();
        //SaveProgress(playerProgress);
    }

	public void LoadProgress()
	{
		playerProgress = JsonUtility.FromJson<Progress>(PlayerPrefs.GetString(SAVE_KEY));
		if (playerProgress == null)
		{
			playerProgress = new Progress();
		}
	}

	public void SaveProgress(Progress tempProgress)
	{
        //playerProgress.money = MoneyController.Instance.Money;
        playerProgress.IsDontWantTutorial = tempProgress.IsDontWantTutorial;
        playerProgress.money = tempProgress.money;
        playerProgress.carCount = tempProgress.carCount;
        playerProgress.TrashCollect = tempProgress.TrashCollect;
        string progress = JsonUtility.ToJson(playerProgress);
		PlayerPrefs.SetString(SAVE_KEY, progress);
	}

}

[System.Serializable]
public class Progress
{
	public bool IsDontWantTutorial;
	public int money;
	public int carCount;
	public int TrashCollect;
}
