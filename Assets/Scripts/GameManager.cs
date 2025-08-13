using System.Collections.Generic;
using UnityEngine;
using LuongBaoToanTools;

interface IHealth
{
	void Dame ();
	void Destroy ();
}

[System.Serializable]
public class Level
{
	[Header("Enemy Per Level")]
	public int zombie1Amount;
	public int zombie2Amount;
	public int zombie3Amount;
	public int zombie4Amount;
	public int zombie5Amount;
	public int zombie6Amount;
	[Header("Bullet Per Load")]
	public int bullet;
	[Header("Enemy can escape")]
	public int allowEnemy;
}
[System.Serializable]
public class Gun
{
	public float[] dameLevel;
	public float[] bulletSpeedLevel;
}
public class GameManager : MonoBehaviour {
	public static GameManager Instance;
	private static bool upgardeArmor;
	public Gun[] gun;
	[Header("Game Parameter")]
	public int gunLevel = 0;
	public int startBulletValue;
	public int score;
	public int enemyGened;
	public int enemyEscape;
	public int enemyPerLevel;
	public int enemyCount;
	public int enemyKilled;
	public int dameLevel;
	public int armorLevel;
	public int speedLevel;
	public int ySorterOffset;
	public bool useBoom;
	public List<GameObject> enemyList;


	[Header("Gen Wave")]
	public Level[] level;
	[HideInInspector]public int currentLevel;
	private int zb1, zb2, zb3, zb4, zb5, zb6;
	public Vector3[] genPos = new Vector3[3];


	[Header("Object")]
	public GameObject completePanel;
	public GameObject pausePanel;
	public GameObject endPanel;
	public GameObject rechargeButton;
	public GameObject escapeWarning;

	public bool startCheck;

	private	void Awake ()
	{
		Instance = this;
		genPos [0] = new Vector3 (Camera.main.ScreenToWorldPoint (new Vector3( Screen.width* 1/ 4f, Screen.height, 0)).x + 1, Camera.main.ScreenToWorldPoint (new Vector3 (0, Screen.height, 0)).y, 0);
		genPos [1] = new Vector3 (Camera.main.ScreenToWorldPoint (new Vector3( Screen.width/2f, Screen.height, 0)).x, Camera.main.ScreenToWorldPoint (new Vector3 (0, Screen.height, 0)).y, 0);
		genPos [2] = new Vector3 (Camera.main.ScreenToWorldPoint (new Vector3( Screen.width * 3 / 4f, Screen.height, 0)).x - 1, Camera.main.ScreenToWorldPoint (new Vector3 (0, Screen.height, 0)).y, 0);
	}

	void Start ()
	{
		startBulletValue = 30;
		UIManager.Instance.SetBulletText ();
		enemyGened = 0;
		currentLevel = 0;
		UIManager.Instance.SetLevelText ();

		enemyEscape = level [currentLevel].allowEnemy;
		UIManager.Instance.SetEscapeText ();


		enemyPerLevel = level [currentLevel].zombie1Amount + level [currentLevel].zombie2Amount + level [currentLevel].zombie3Amount + level [currentLevel].zombie4Amount + level [currentLevel].zombie5Amount + level [currentLevel].zombie6Amount;
		enemyCount = enemyPerLevel;
		enemyKilled = 0;
		UIManager.Instance.FillLevelImage ();

		dameLevel = 1;
		armorLevel = 1;
		speedLevel = 1;
		UIManager.Instance.SetUpgradeText ();

		UIManager.Instance.SetMoneyText ();

		AudioSystem ();

//		GenerateEnemy ();	
		InvokeRepeating("GenerateEnemy1",0,3);
		InvokeRepeating("GenerateEnemy2",10,7);
		InvokeRepeating("GenerateEnemy3",15,10);
		InvokeRepeating("GenerateEnemy4",15,12);
		InvokeRepeating("GenerateEnemy5",20,15);
		InvokeRepeating("GenerateEnemy6",25,20);

		Invoke ("StartCheck", 2);
	}

	void StartCheck()
	{
		startCheck = true;
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
				activity.Call<bool>("moveTaskToBack", true);
			}
			else
			{
				Application.Quit();
			}
		}

		if (GameManager.Instance.dameLevel >= GameManager.Instance.gun [Menu.selectedGun].dameLevel.Length) {
			UIManager.Instance.FullUpgradeDame ();
		}

		if (GameManager.Instance.speedLevel >= GameManager.Instance.gun [Menu.selectedGun].bulletSpeedLevel.Length) {
			UIManager.Instance.FullUpgradeSpeed ();
		}
	}
		
	public void PauseGame(int index)
	{
		switch (index) {
		case 0:
			Time.timeScale = 0;
			pausePanel.SetActive (true);
			
			break;
		case 1:
			Time.timeScale = 1;
//			Application.LoadLevel (0);
			UIManager.Instance.startFill1 = true;
			break;
		case 2:
			print ("sound");
			break;
		case 3:
			Time.timeScale = 1;
			pausePanel.SetActive (false);
			break;
		}
	}

	public void GameOver (int index)
	{
		switch (index) {
		case 0:
			Time.timeScale = 1;
//			Application.LoadLevel (0);
			UIManager.Instance.startFill1 = true;
			break;
		case 1:
			if (PlayerPrefs.GetInt ("audio") == 0) {
				PlayerPrefs.SetInt ("audio", 1);
				if (Audio.Instance != null) {
					Audio.Instance.source.volume = 0;
				}
			}
			else if (PlayerPrefs.GetInt ("audio") == 1) {
				PlayerPrefs.SetInt ("audio", 0);
				if (Audio.Instance != null) {
					Audio.Instance.source.volume = 1;
				}
			}
			AudioSystem ();
			break;
		case 2:
			Time.timeScale = 1;
			Application.LoadLevel (Application.loadedLevel);
			break;
		}
	}
	public void AudioSystem()
	{
		if (PlayerPrefs.GetInt ("audio") == 0) {
			if (Audio.Instance != null) {
				Audio.Instance.source.volume = 1;
			}
			UIManager.Instance.AudioSpr.sprite =UIManager.Instance. AudioOn;
			UIManager.Instance.AudioSpr1.sprite =UIManager.Instance. AudioOn;
		} else {
			if (Audio.Instance != null) {
				Audio.Instance.source.volume = 0;
			}
			UIManager.Instance.AudioSpr.sprite =UIManager.Instance. AudioOff;
			UIManager.Instance.AudioSpr1.sprite =UIManager.Instance. AudioOff;
		}
	}

	public void Upgrade(int index)
	{
		switch (index) {
		case 0:
			if (GameManager.Instance.dameLevel < GameManager.Instance.gun [Menu.selectedGun].dameLevel.Length) {
				GameManager.Instance.dameLevel += 1;
				UIManager.Instance.SetUpgradeText ();
				NextLevel ();
				if (Audio.Instance != null) {
					Audio.Instance.SoundEffect (9);
					Audio.Instance.PlayBgMenu (1);
				}
			} 
			break;
		case 1:
			upgardeArmor = true;
			NextLevel ();
			if (Audio.Instance != null) {
				Audio.Instance.SoundEffect (9);
				Audio.Instance.PlayBgMenu (1);
			}
			break;
		case 2:
			if (GameManager.Instance.speedLevel < GameManager.Instance.gun [Menu.selectedGun].bulletSpeedLevel.Length) {
				GameManager.Instance.speedLevel += 1;
				UIManager.Instance.SetUpgradeText ();
				NextLevel ();
				if (Audio.Instance != null) {
					Audio.Instance.SoundEffect (9);
					Audio.Instance.PlayBgMenu (1);
				}
			}
			break;
		}
	}

	///<Summary>
	///Start generate enemy
	///</Summary>
	private void GenerateEnemy1()
	{
		ySorterOffset += 20;
		var enemy1 = ObjectPooling.INSTANCE.GetObjectFromPoolList(ObjectPooling.INSTANCE.pooledEnemy1,
			genPos[Random.Range(0,3)]);
		if (enemy1 != null) {
			enemyGened += 1;
			zb1 += 1;
			if (zb1 > level [currentLevel].zombie1Amount) {
				enemyGened -= 1;
				zb1 = 0;
				enemy1.SetActive (false);
				CancelInvoke ("GenerateEnemy1");
//				return;
			}
		}
		if (enemyGened == enemyPerLevel) {
			enemy1.GetComponent<EnemyContronller> ().lastEnemy = true;
		}
	}
	private void GenerateEnemy2()
	{
		ySorterOffset += 20;
		var enemy2 = ObjectPooling.INSTANCE.GetObjectFromPoolList(ObjectPooling.INSTANCE.pooledEnemy2,
			genPos[Random.Range(0,3)]);
		if (enemy2 != null) {
			enemyGened += 1;
			zb2 += 1;
			if (zb2 > level [currentLevel].zombie2Amount) {
				enemyGened -= 1;
				zb2 = 0;
				enemy2.SetActive (false);
				CancelInvoke ("GenerateEnemy2");
//				return;
			}
		}
		if (enemyGened == enemyPerLevel) {
			enemy2.GetComponent<EnemyContronller> ().lastEnemy = true;
		}
	}
	private void GenerateEnemy3()
	{
		ySorterOffset += 20;
		var enemy3 = ObjectPooling.INSTANCE.GetObjectFromPoolList(ObjectPooling.INSTANCE.pooledEnemy3,
			genPos[Random.Range(0,3)]);
		if (enemy3 != null) {
			enemyGened += 1;
			zb3 += 1;
			if (zb3 > level [currentLevel].zombie3Amount) {
				enemyGened -= 1;
				zb3 = 0;
				enemy3.SetActive (false);
				CancelInvoke ("GenerateEnemy3");
//				return;
			}
		}
		if (enemyGened == enemyPerLevel) {
			enemy3.GetComponent<EnemyContronller> ().lastEnemy = true;
		}
	}
	private void GenerateEnemy4()
	{
		ySorterOffset += 20;
		var enemy4 = ObjectPooling.INSTANCE.GetObjectFromPoolList(ObjectPooling.INSTANCE.pooledEnemy4,
			genPos[Random.Range(0,3)]);
		if (enemy4 != null) {
			enemyGened += 1;
			zb4 += 1;
			if (zb4 > level [currentLevel].zombie4Amount) {
				enemyGened -= 1;
				zb4 = 0;
				enemy4.SetActive (false);
				CancelInvoke ("GenerateEnemy4");
//				return;
			}
		}
		if (enemyGened == enemyPerLevel) {
			enemy4.GetComponent<EnemyContronller> ().lastEnemy = true;
		}
	}
	private void GenerateEnemy5()
	{
		ySorterOffset += 20;
		var enemy5 = ObjectPooling.INSTANCE.GetObjectFromPoolList(ObjectPooling.INSTANCE.pooledEnemy5,
			genPos[Random.Range(0,3)]);
		if (enemy5 != null) {
			enemyGened += 1;
			zb5 += 1;
			if (zb5 > level [currentLevel].zombie5Amount) {
				enemyGened -= 1;
				zb5 = 0;
				enemy5.SetActive (false);
				CancelInvoke ("GenerateEnemy5");
//				return;
			}
		}
		if (enemyGened == enemyPerLevel) {
			enemy5.GetComponent<EnemyContronller> ().lastEnemy = true;
		}
	}
	private void GenerateEnemy6()
	{
		ySorterOffset += 20;
		var enemy6 = ObjectPooling.INSTANCE.GetObjectFromPoolList(ObjectPooling.INSTANCE.pooledEnemy6,
			genPos[Random.Range(0,3)]);
		if (enemy6 != null) {
			enemyGened += 1;
			zb6 += 1;
			if (zb6 > level [currentLevel].zombie6Amount) {
				enemyGened -= 1;
				zb6 = 0;
				enemy6.SetActive (false);
				CancelInvoke ("GenerateEnemy6");
//				return;
			}
		}
		if (enemyGened == enemyPerLevel) {
			enemy6.GetComponent<EnemyContronller> ().lastEnemy = true;
		}
	}

	private void NextLevel()
	{
		enemyGened = 0;
		ySorterOffset = 0;
		currentLevel += 1;
		UIManager.Instance.SetLevelText ();

		if (upgardeArmor) {
			upgardeArmor= false;
			enemyEscape +=1;
		} 
		UIManager.Instance.SetEscapeText ();
		Time.timeScale = 1;
		//AdsControl.Instance.showAds ();
		Invoke ("SetPanelFalse",0.25f);
		enemyKilled = 0;
		enemyPerLevel = level [currentLevel].zombie1Amount + level [currentLevel].zombie2Amount + level [currentLevel].zombie3Amount + level [currentLevel].zombie4Amount + level [currentLevel].zombie5Amount + level [currentLevel].zombie6Amount;
		enemyCount = enemyPerLevel;
		UIManager.Instance.FillLevelImage ();
//		InvokeRepeating("GenerateEnemy1",0,3);
//		InvokeRepeating ("GenerateEnemy2", 5, 3);
//		InvokeRepeating ("GenerateEnemy3", 10, 4);
//		InvokeRepeating ("GenerateEnemy4", 15, 5);
//		InvokeRepeating ("GenerateEnemy5", 20, 8);
//		InvokeRepeating ("GenerateEnemy6", 25, 10);
		zb1 = 0;
		zb2 = 0;
		zb3 = 0;
		zb4 = 0;
		zb5 = 0;
		zb6 = 0;
		InvokeRepeating("GenerateEnemy1",0,3);
		InvokeRepeating("GenerateEnemy2",10,7);
		InvokeRepeating("GenerateEnemy3",15,10);
		InvokeRepeating("GenerateEnemy4",15,12);
		InvokeRepeating("GenerateEnemy5",20,15);
		InvokeRepeating("GenerateEnemy6",25,20);
	}

	public void DecreaseEnemy()
	{
		enemyCount -= 1;
		if (enemyCount == 0) {
			Time.timeScale = 0;
			//AdsControl.Instance.showAds ();

			GameManager.Instance.completePanel.SetActive (true);
		}
	}
	private void SetPanelFalse()
	{
		completePanel.SetActive (false);
	}

	public void Recharge()
	{
		startBulletValue = 30;UIManager.Instance.SetBulletText ();
		rechargeButton.SetActive (false);
	}

	public void Boom()
	{
		if (PlayerPrefs.GetInt ("boom") > 0) {
			PlayerPrefs.SetInt ("boom", PlayerPrefs.GetInt ("boom") - 1);
			UIManager.Instance.SetBomText ();
			useBoom = true;
		} else
			return;

	}

	public Animator camAnim;
	public void Shake()
	{
		camAnim.enabled = true;
	}

	public void Warning()
	{
		escapeWarning.SetActive (true);
		Invoke ("WarningDone", 0.75f);
	}
	private void WarningDone()
	{
		escapeWarning.SetActive (false);
	}
	public void KillEnemy()
	{
		for (int i = 0; i < ObjectPooling.INSTANCE.pooledEnemy1.Count; i++) {
			if (ObjectPooling.INSTANCE.pooledEnemy1 [i].activeInHierarchy) {
				ObjectPooling.INSTANCE.pooledEnemy1 [i].SetActive (false);
			}
		}
		for (int i = 0; i < ObjectPooling.INSTANCE.pooledEnemy2.Count; i++) {
			if (ObjectPooling.INSTANCE.pooledEnemy2 [i].activeInHierarchy) {
				ObjectPooling.INSTANCE.pooledEnemy2 [i].SetActive (false);
			}
		}
		for (int i = 0; i < ObjectPooling.INSTANCE.pooledEnemy3.Count; i++) {
			if (ObjectPooling.INSTANCE.pooledEnemy3 [i].activeInHierarchy) {
				ObjectPooling.INSTANCE.pooledEnemy3 [i].SetActive (false);
			}
		}
		for (int i = 0; i < ObjectPooling.INSTANCE.pooledEnemy4.Count; i++) {
			if (ObjectPooling.INSTANCE.pooledEnemy4 [i].activeInHierarchy) {
				ObjectPooling.INSTANCE.pooledEnemy4 [i].SetActive (false);
			}
		}
		for (int i = 0; i < ObjectPooling.INSTANCE.pooledEnemy5.Count; i++) {
			if (ObjectPooling.INSTANCE.pooledEnemy5 [i].activeInHierarchy) {
				ObjectPooling.INSTANCE.pooledEnemy5 [i].SetActive (false);
			}
		}
		for (int i = 0; i < ObjectPooling.INSTANCE.pooledEnemy6.Count; i++) {
			if (ObjectPooling.INSTANCE.pooledEnemy6 [i].activeInHierarchy) {
				ObjectPooling.INSTANCE.pooledEnemy6 [i].SetActive (false);
			}
		}
	}
}
