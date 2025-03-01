using Gley.DailyRewards.Internal;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Gley.DailyRewards
{
	public class DayButtonScript : MonoBehaviour
	{
		public Text dayText;
		public Image rewardImage;
		public Text rewardValue;

		public Image dayBg;
		public Sprite claimedSprite;
		public Sprite currentSprite;
		public Sprite availableSprite;
		public Sprite lockedSprite;

		private Sprite rewardSprite;
		private int dayNumber;
		private int reward;
        
		public Color claimedColor;
		public Color currentColor;
		public Color availableColor;
		public Color lockedColor;

		public GameObject locked;
		public Image titleImage;
		public Image buttonImage;
	    
		public Text titleText;
		public TextMeshProUGUI rewardText;

		/// <summary>
		/// Setup each day button
		/// </summary>
		/// <param name="dayNumber">current day number</param>
		/// <param name="rewardSprite">button sprite</param>
		/// <param name="rewardValue">reward value</param>
		/// <param name="currentDay">current active day</param>
		/// <param name="timeExpired">true if timer expired</param>
		public void Initialize(int dayNumber, Sprite rewardSprite, int rewardValue, int currentDay, bool timeExpired, ValueFormatterFunction valueFormatterFunction)
		{
			dayText.text = $"DAY {dayNumber}";
			rewardImage.sprite = rewardSprite;
			bool formattedUsingFormatterFunction = false;
			if (valueFormatterFunction != null)
			{
				try
				{
					this.rewardValue.text = valueFormatterFunction(rewardValue);
					formattedUsingFormatterFunction = true;
				}
					catch (System.Exception)
					{
					}
			}
			if (!formattedUsingFormatterFunction)
			{
				this.rewardValue.text = $"x{rewardValue}";
			}
			this.dayNumber = dayNumber;
			this.rewardSprite = rewardSprite;
			reward = rewardValue;

			Refresh(currentDay, timeExpired);
			
		}


		/// <summary>
		/// Refresh button if required
		/// </summary>
		/// <param name="currentDay"></param>
		/// <param name="timeExpired"></param>
		public void Refresh(int currentDay, bool timeExpired)
		{
			//Debug.LogError("Day number " + (dayNumber -1) + " currentdDay " + currentDay);
		    
			if (dayNumber - 1 < currentDay) 
			{
				dayBg.sprite = claimedSprite;
			}

			if (dayNumber - 1 == currentDay)
			{
				if (timeExpired == true)
				{
					dayBg.sprite = availableSprite;
				}
				else
				{
					dayBg.sprite = currentSprite;					
				}
				dayText.gameObject.SetActive(true);
				
			}

			if (dayNumber - 1 > currentDay)
			{
				dayBg.sprite = lockedSprite;
				dayText.gameObject.SetActive(true);
			}
			
			if(dayBg.sprite == claimedSprite)
			{
				rewardImage.color = Color.white;
				buttonImage.color = claimedColor;
				titleImage.color = claimedColor;
				GetComponent<Image>().color = claimedColor;
				
				dayText.color = Color.black;
				titleText.color = Color.black;
				rewardText.color = Color.black;
	            
				rewardText.text = "Claimed";
				locked.SetActive(false);

			}
			else if(dayBg.sprite == availableSprite)
			{
				dayText.color = Color.black;
				titleText.color = Color.black;
				rewardText.color = Color.black;
				
				rewardText.text = "Claim";

				buttonImage.color = availableColor;
				titleImage.color = availableColor;
				GetComponent<Image>().color = availableColor;
				locked.SetActive(false);

			}
			else if(dayBg.sprite == currentSprite)
			{
				dayText.color = Color.black;
				titleText.color = Color.black;
				rewardText.color = Color.black;
				
				buttonImage.color = currentColor;
				titleImage.color = currentColor;
				
				rewardText.text = "In Process";

				GetComponent<Image>().color = currentColor;
				locked.SetActive(false);
			}
			else if(dayBg.sprite == lockedSprite)
			{
				locked.SetActive(true);
				rewardImage.color = lockedColor;
				buttonImage.color = lockedColor;
				titleImage.color = lockedColor;
				GetComponent<Image>().color = lockedColor;
				
				dayText.color = Color.white;
				titleText.color = Color.white;
				rewardText.color = Color.white;
	            
				rewardText.text = "Locked";
			}
		}


		/// <summary>
		/// Called when a day button is clicked
		/// </summary>
		public void ButtonClicked()
		{
			if (dayBg.sprite == availableSprite)
			{
			    CalendarManager.Instance.ButtonClick(dayNumber, reward, rewardSprite);
			}
		}
	}
}
