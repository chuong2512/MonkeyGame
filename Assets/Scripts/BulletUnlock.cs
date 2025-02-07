using UnityEngine;
using UnityEngine.UI;
public class BulletUnlock : MonoBehaviour {
public enum BulletType
	{
		normal,
		water,
		tomato,
		enegy,
		arrow,
		tennis,
		killerfock,
		cannonball,
		shuriken,
		bublebullet,
		laser,
		buyBoom
	}

	public BulletType _BulletType;
	public bool isUnlocked;
	[SerializeField] Button thisBtn;
	[SerializeField] Text condition;
	private	void Start ()
	{
		CheckCondition ();
	}
		

	void CheckCondition()
	{
		switch (_BulletType) {
		case BulletType.normal:
			isUnlocked = true;
			thisBtn.interactable = true;
			break;
		case BulletType.water:
			if (PlayerPrefs.GetInt ("totalkill") > 300) {
				isUnlocked = true;
				thisBtn.interactable = true;
			} else {
				condition.text = "Kill:"+PlayerPrefs.GetInt("totalkill").ToString()+"/300";
				isUnlocked = false;
				thisBtn.interactable = false;
			}
			break;
		case BulletType.tomato:
			if (PlayerPrefs.GetInt ("totalkill") > 500) {
				isUnlocked = true;
				thisBtn.interactable = true;
			} else {
				condition.text = "Kill:"+PlayerPrefs.GetInt("totalkill").ToString()+"/500";
				isUnlocked = false;
				thisBtn.interactable = false;
			}
			break;
		case BulletType.enegy:
			if (PlayerPrefs.GetInt ("bestscore") > 500) {
				isUnlocked = true;
				thisBtn.interactable = true;
			} else {
				condition.text = "Best:"+PlayerPrefs.GetInt("bestscore").ToString()+"/500";
				isUnlocked = false;
				thisBtn.interactable = false;
			}
			break;
		case BulletType.arrow:
			if (PlayerPrefs.GetInt ("bestscore") > 700) {
				isUnlocked = true;
				thisBtn.interactable = true;
			} else {
				condition.text = "Best:"+PlayerPrefs.GetInt("bestscore").ToString()+"/700";
				isUnlocked = false;
				thisBtn.interactable = false;
			}
			break;
		case BulletType.tennis:
			if (PlayerPrefs.GetInt ("totalkill") > 600) {
				isUnlocked = true;
				thisBtn.interactable = true;
			} else {
				condition.text = "Kill:"+PlayerPrefs.GetInt("totalkill").ToString()+"/600";
				isUnlocked = false;
				thisBtn.interactable = false;
			}
			break;
		case BulletType.killerfock:
			if (PlayerPrefs.GetInt ("totalkill") > 800) {
				isUnlocked = true;
				thisBtn.interactable = true;
			} else {
				condition.text = "Kill:"+PlayerPrefs.GetInt("totalkill").ToString()+"/800";
				isUnlocked = false;
				thisBtn.interactable = false;
			}
			break;
		case BulletType.cannonball:
			if (PlayerPrefs.GetInt ("totalkill") > 900) {
				isUnlocked = true;
				thisBtn.interactable = true;
			} else {
				condition.text = "Kill:"+PlayerPrefs.GetInt("totalkill").ToString()+"/900";
				isUnlocked = false;
				thisBtn.interactable = false;
			}
			break;
		case BulletType.shuriken:
			if (PlayerPrefs.GetInt ("totalkill") > 950) {
				isUnlocked = true;
				thisBtn.interactable = true;
			} else {
				condition.text = "Kill:"+PlayerPrefs.GetInt("totalkill").ToString()+"/950";
				isUnlocked = false;
				thisBtn.interactable = false;
			}
			break;
		case BulletType.bublebullet:
			if (PlayerPrefs.GetInt ("bestscore") > 900) {
				isUnlocked = true;
				thisBtn.interactable = true;
			} else {
				condition.text = "Best:"+PlayerPrefs.GetInt("bestscore").ToString()+"/900";
				isUnlocked = false;
				thisBtn.interactable = false;
			}
			break;
		case BulletType.laser:
			if (PlayerPrefs.GetInt ("survivalMode") == 1) {
				isUnlocked = true;
				thisBtn.interactable = true;
			} else {
				isUnlocked = false;
				thisBtn.interactable = false;
			}
			break;
		}
		Menu.Instance.GunCheck ();
	}
}
