using UnityEngine;
using Gley.DailyRewards;

public class DailyRewardsManager : MonoBehaviour
{
	
	private void Start()
	{
		Gley.DailyRewards.API.Calendar.AddClickListener(CalendarButtonClicked);
		//CCDS_SaveGameManager.Delete();
	}
	
	public void ShowCalendar()
	{
		Gley.DailyRewards.API.Calendar.Show();
	}
	
	private void CalendarButtonClicked(int dayNumber, int rewardValue, Sprite rewardSprite)
	{		
		CCDS.ChangeMoney(rewardValue);	
	}
}
