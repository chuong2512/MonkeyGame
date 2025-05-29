using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class CheckInReward
{
	public string rewardName;
	public int    rewardAmount;
	public Sprite rewardIcon;
}

[System.Serializable]
public class CheckInDay
{
	public bool            isClaimed;
	public System.DateTime claimDate;
	public CheckInReward   reward;
}

public class DailyCheckInSystem : MonoBehaviour
{
	[SerializeField] private CheckInReward[] dailyRewards;
	[SerializeField] private GameObject      checkInUIItemPrefab;
	[SerializeField] private Transform       checkInContainer;

	private       CheckInDay[] checkInDays;
	private const int          TOTAL_DAYS       =9;
	private const string       LAST_CHECK_IN_KEY="LastCheckInDate";
	private const string       CLAIMED_DAYS_KEY ="ClaimedDays"; // Key mới để lưu trạng thái các ngày

	private void Start()
	{
		InitializeCheckInSystem();
		UpdateUI();
	}

	private void InitializeCheckInSystem()
	{
		checkInDays=new CheckInDay[TOTAL_DAYS];
		for(int i=0;i<TOTAL_DAYS;i++)
		{
			checkInDays[i]=new CheckInDay
			{
				isClaimed=false,
				reward   =dailyRewards[i]
			};
		}

		LoadCheckInData();
	}

	private void LoadCheckInData()
	{
		string lastCheckInStr=PlayerPrefs.GetString(LAST_CHECK_IN_KEY,"");
		string claimedDaysStr=PlayerPrefs.GetString(CLAIMED_DAYS_KEY,"");

		if(!string.IsNullOrEmpty(lastCheckInStr))
		{
			DateTime lastCheckIn=DateTime.Parse(lastCheckInStr);
			DateTime today      =DateTime.Now.Date;
			TimeSpan difference =today-lastCheckIn.Date;

			// Reset nếu đã qua ngày mới
			if(difference.Days>=1)
			{
				ResetCheckIn();
			}
			// Load trạng thái đã claim của các ngày
			else if(!string.IsNullOrEmpty(claimedDaysStr))
			{
				string[] claimedDays=claimedDaysStr.Split(',');
				foreach(string day in claimedDays)
				{
					if(int.TryParse(day,out int index) && index>=0 && index<TOTAL_DAYS)
					{
						checkInDays[index].isClaimed=true;
						checkInDays[index].claimDate=lastCheckIn;
					}
				}
			}
		}
	}

	private void SaveClaimedDays()
	{
		List<string> claimedDays=new List<string>();
		for(int i=0;i<checkInDays.Length;i++)
		{
			if(checkInDays[i].isClaimed)
			{
				claimedDays.Add(i.ToString());
			}
		}
		PlayerPrefs.SetString(CLAIMED_DAYS_KEY,string.Join(",",claimedDays));
		PlayerPrefs.Save();
	}

	public void OnCheckInButtonClicked(int dayIndex)
	{
		if(dayIndex>=TOTAL_DAYS || checkInDays[dayIndex].isClaimed)
			return;

		// Kiểm tra xem đã nhận thưởng ngày hôm nay chưa
		if(HasClaimedToday())
		{
			Debug.Log("Already claimed today's reward!");
			return;
		}

		// Kiểm tra phải claim theo thứ tự
		if(dayIndex>0 && !checkInDays[dayIndex-1].isClaimed)
		{
			Debug.Log("Must claim previous day rewards first!");
			return;
		}

		// Claim phần thưởng
		checkInDays[dayIndex].isClaimed=true;
		checkInDays[dayIndex].claimDate=DateTime.Now;

		// Lưu ngày claim
		PlayerPrefs.SetString(LAST_CHECK_IN_KEY,DateTime.Now.ToString());
		SaveClaimedDays();

		// Phát thưởng
		GiveReward(checkInDays[dayIndex].reward);

		UpdateUI();
	}

	private bool HasClaimedToday()
	{
		string lastClaimStr=PlayerPrefs.GetString(LAST_CHECK_IN_KEY,"");
		if(string.IsNullOrEmpty(lastClaimStr)) return false;

		DateTime lastClaim=DateTime.Parse(lastClaimStr);
		return lastClaim.Date==DateTime.Now.Date;
	}

	private void ResetCheckIn()
	{
		foreach(var day in checkInDays)
		{
			day.isClaimed=false;
			day.claimDate=default;
		}
		PlayerPrefs.DeleteKey(CLAIMED_DAYS_KEY);
		PlayerPrefs.Save();
	}

	private void GiveReward(CheckInReward reward)
	{
		// Implement your reward distribution logic here
		Debug.Log($"Giving reward: {reward.rewardName} x{reward.rewardAmount}");

		GameDataManager.Instance.playerData.AddDiamond(reward.rewardAmount);

		DailyCheckInPopup.Instance.OnRewardClaimed();
	}

	private void UpdateUI()
	{
		// Clear existing UI
		foreach(Transform child in checkInContainer)
		{
			Destroy(child.gameObject);
		}

		// Create UI for each day
		for(int i=0;i<TOTAL_DAYS;i++)
		{
			GameObject   item =Instantiate(checkInUIItemPrefab,checkInContainer);
			CheckInDayUI dayUI=item.GetComponent<CheckInDayUI>();

			if(dayUI!=null)
			{
				dayUI.SetupDay(
				i+1,
				checkInDays[i].reward.rewardIcon,
				checkInDays[i].isClaimed,
				checkInDays[i].reward.rewardAmount);

				int index=i;
				dayUI.OnClaimButton.onClick.AddListener(() => OnCheckInButtonClicked(index));
			}
		}
	}
}
