using System;
using Jackal;
using UnityEngine;

public class DailyCheckInPopup : Singleton<DailyCheckInPopup>
{
    [SerializeField] private GameObject popupPanel;
    
    private const string LAST_CLAIM_DATE_KEY = "LastClaimDate";
    private bool _isPopupShown = false;

    private void Start()
    {
        CheckAndShowPopup();
    }

    private void CheckAndShowPopup()
    {
        if (HasClaimedToday())
        {
            HidePopup();
        }
        else
        {
            ShowPopup();
        }
    }

    public bool HasClaimedToday()
    {
        string lastClaimDateStr = PlayerPrefs.GetString(LAST_CLAIM_DATE_KEY, "");
        
        if (string.IsNullOrEmpty(lastClaimDateStr))
            return false;

        DateTime lastClaimDate = DateTime.Parse(lastClaimDateStr);
        DateTime today = DateTime.Now.Date;
        
        return lastClaimDate.Date == today;
    }

    public void ShowPopup()
    {
        if (!_isPopupShown && !HasClaimedToday())
        {
            popupPanel.SetActive(true);
            _isPopupShown = true;
        }
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
        _isPopupShown = false;
    }

    // Gọi method này sau khi nhận thưởng thành công
    public void OnRewardClaimed()
    {
        PlayerPrefs.SetString(LAST_CLAIM_DATE_KEY, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }
}