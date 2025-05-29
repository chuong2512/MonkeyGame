using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckInDayUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI dayText;
	[SerializeField] private TextMeshProUGUI amountText;
	[SerializeField] private Image           rewardIcon;
	[SerializeField] private GameObject      claimedOverlay;
	[SerializeField] private Button          claimButton;

	public Button OnClaimButton => claimButton;

	public void SetupDay(int day,Sprite reward,bool isClaimed,int amount)
	{
		dayText.text     =$"Day {day}";
		amountText.text  =$"{amount}";
		rewardIcon.sprite=reward;
		rewardIcon.SetNativeSize();
		claimedOverlay.SetActive(isClaimed);
		claimButton.interactable=!isClaimed;
	}
}
