using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	public static Menu             Instance;
	public static int              selectedGun;
	int                            currentBullet;
	[SerializeField] Image[]       selectbutton;
	[SerializeField] Sprite        selected,unselect;
	[SerializeField] GameObject[]  unlocked;
	[SerializeField] GameObject[]  locked;
	[SerializeField] GameObject    gunselector;
	[SerializeField] GameObject    mainMenu;
	[SerializeField] RectTransform leftSide, rightSide;
	bool                           startFill,startFill1,startFill2,toGunSelector,startGame;
	[SerializeField] float         fillSpeed;
	[SerializeField] Image         AudioSpr;
	[SerializeField] Sprite        AudioOn,  AudioOff;
	[SerializeField] Text          moneyText,boomText;
	[SerializeField] Text          moneyText1,boomText1;
	public           GameObject    PopUpRewardVideo;

	private void Awake()
	{
		Instance=this;
		SetAnchor();
//		PlayerPrefs.SetInt ("money", 1000);
	}

	IEnumerator Start()
	{
		SelectorImage();
		Audio.Instance.SoundEffect(8);
		AudioSystem();
		SetText();
		yield return new WaitForEndOfFrame();
		GunCheck();
		Invoke("StartFill1",0.5f);
	}

	private void SelectorImage()
	{
		selectedGun=PlayerPrefs.GetInt("gun");
		for(int i=0;i<selectbutton.Length;i++)
		{
			if(selectedGun==i)
			{
				selectbutton[i].sprite=selected;
			}
			else
			{
				selectbutton[i].sprite=unselect;
			}
		}
	}

	public void GunCheck()
	{
		for(int i=0;i<unlocked.Length;i++)
		{
			if(unlocked[i].transform.parent.gameObject.GetComponent<BulletUnlock>().isUnlocked)
			{
				unlocked[i].SetActive(true);
				locked[i].SetActive(false);
			}
			else
			{
				unlocked[i].SetActive(false);
				locked[i].SetActive(true);
			}
		}
	}

	public void MenuOption(int index)
	{
		switch (index)
		{
			case 0:
//			Application.LoadLevel (1);
				startFill=true;
				if(Audio.Instance!=null)
				{
					Audio.Instance.SoundEffect(11);
				}
				break;
			case 1:
//			mainMenu.SetActive (false);
//			gunselector.SetActive (true);
				startFill2   =true;
				toGunSelector=true;
				if(Audio.Instance!=null)
				{
					Audio.Instance.SoundEffect(11);
				}
				break;
			case 2:
//			mainMenu.SetActive (true);
//			gunselector.SetActive (false);
				startFill2=true;
				if(Audio.Instance!=null)
				{
					Audio.Instance.SoundEffect(11);
				}
				break;
		}
	}

	public void Selectgun(int type)
	{
		PlayerPrefs.SetInt("gun",type);
		SelectorImage();
	}

	public void BuyBoom()
	{
		if(PlayerPrefs.GetInt("money")>=50)
		{
			PlayerPrefs.SetInt("boom",PlayerPrefs.GetInt("boom")+1);
			PlayerPrefs.SetInt("money",PlayerPrefs.GetInt("money")-50);
			SetText();
		}
		else
			PopUpRewardVideo.SetActive(true);
	}
	private void SetAnchor()
	{
		leftSide.anchoredPosition =new Vector2(500,0);
		rightSide.anchoredPosition=new Vector2(-500,0);
	}
	private void StartFill1() { startFill1=true; }

	void Update()
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			if(Application.platform==RuntimePlatform.Android)
			{
				AndroidJavaObject activity=new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
				activity.Call<bool>("moveTaskToBack",true);
			}
			else
			{
				Application.Quit();
			}
		}
		if(startFill)
		{
			leftSide.anchoredPosition=new Vector2(leftSide.anchoredPosition.x+fillSpeed,0);
			if(leftSide.anchoredPosition.x==500f)
			{
				startFill=false;
				startGame=true;
			}
			rightSide.anchoredPosition=new Vector2(rightSide.anchoredPosition.x-fillSpeed,0);
		}

		if(startFill1)
		{
			leftSide.anchoredPosition=new Vector2(leftSide.anchoredPosition.x-fillSpeed,0);
			if(leftSide.anchoredPosition.x==-800)
			{
				startFill1=false;
			}
			rightSide.anchoredPosition=new Vector2(rightSide.anchoredPosition.x+fillSpeed,0);
		}

		if(startFill2)
		{
			leftSide.anchoredPosition=new Vector2(leftSide.anchoredPosition.x+fillSpeed,0);
			if(leftSide.anchoredPosition.x==500f)
			{
				startFill2=false;
				Invoke("StartFill1",0.25f);
				if(toGunSelector)
				{
					toGunSelector=false;
					mainMenu.SetActive(false);
					gunselector.SetActive(true);
				}
				else
				{
					mainMenu.SetActive(true);
					gunselector.SetActive(false);
				}
			}
			rightSide.anchoredPosition=new Vector2(rightSide.anchoredPosition.x-fillSpeed,0);
		}

		if(startGame)
		{
			startGame=false;
			if(Audio.Instance!=null)
			{
				Audio.Instance.source.Stop();
			}
			Invoke("Startgame",0.25f);
		}
	}

	private void Startgame() { Application.LoadLevel(1); }

	private void AudioSystem()
	{

		if(PlayerPrefs.GetInt("audio")==0)
		{
			if(Audio.Instance!=null)
			{
				Audio.Instance.source.volume=1;
			}
			AudioSpr.sprite=AudioOn;
		}
		else
		{
			if(Audio.Instance!=null)
			{
				Audio.Instance.source.volume=0;
			}
			AudioSpr.sprite=AudioOff;
		}
	}

	public void SetAudio()
	{
		switch (PlayerPrefs.GetInt("audio"))
		{
			case 0:
				PlayerPrefs.SetInt("audio",1);
				AudioSystem();
				break;
			case 1:
				PlayerPrefs.SetInt("audio",0);
				AudioSystem();
				break;
		}
	}
	public void MoreGames()  { }
	public void ClosePopup() { PopUpRewardVideo.SetActive(false); }
	public void GetReward()
	{
//		PlayerPrefs.SetInt ("money", PlayerPrefs.GetInt ("money") + 50);
		PopUpRewardVideo.SetActive(false);
		//SetText ();

	}
	public void SetText()
	{
//		PlayerPrefs.SetInt ("money", 999);
		moneyText.text=PlayerPrefs.GetInt("money").ToString();
		moneyText1.text=PlayerPrefs.GetInt("money").ToString();
		boomText1.text =PlayerPrefs.GetInt("boom").ToString();
		boomText.text =PlayerPrefs.GetInt("boom").ToString();
	}
}
