using UnityEngine;
using LuongBaoToanTools;
public class EnemyContronller : MonoBehaviour,IHealth {

public enum EnemyType
	{
		enemy1,
		enemy2,
		enemy3,
		enemy4,
		enemy5,
		enemy6
	}

	public EnemyType _EnemyType;

	[SerializeField]  float health;
	[SerializeField] float currentHealth;
	[SerializeField]  GameObject healthBar;
	[SerializeField] GameObject canhQuat;
	[SerializeField] SpriteRenderer[] spr;
	public int enemyLevelIndex;
	public bool getBoomHit;
	public bool lastEnemy;
	public int[] fistValue;

	private	void Awake ()
	{
		fistValue = new int[spr.Length];
		for (int i = 0; i < spr.Length; i++) {
			fistValue [i] = spr [i].sortingOrder;
		}	
	}


	void Start ()
	{
//		GameManager.Instance.enemyList.Add (this.gameObject);	
	}

	void OnEnable ()
	{
		for (int i = 0; i < spr.Length; i++) {
			spr [i].sortingOrder -= GameManager.Instance.ySorterOffset;
		}
		currentHealth = health;

		lastEnemy = false;
	}

	void Update ()
	{
		if (_EnemyType == EnemyType.enemy4) {
			canhQuat.transform.eulerAngles += new Vector3 (0, 0, transform.eulerAngles.z + 15f);
		}
	}


	///<Summary>
	///call when get hit from abullet
	///</Summary>
	int count;
	public void Dame()
	{

		currentHealth -= GameManager.Instance.gun [Menu.selectedGun].dameLevel [GameManager.Instance.dameLevel-1];
		if (currentHealth > 0) {
			healthBar.transform.localScale = new Vector3 (currentHealth / health, healthBar.transform.localScale.y, 0);
		} else {
			healthBar.transform.localScale = new Vector3 (0, healthBar.transform.localScale.y, 0);
			FillLevel ();
			ObjectPooling.INSTANCE.GetObjectFromPoolList (ObjectPooling.INSTANCE.pooledBlood, transform.position);
			if (Audio.Instance != null) {
				Audio.Instance.SoundEffect (Random.Range (2, 8));
			}

			if (lastEnemy) {
				lastEnemy = false;
				for (int i = 0; i < GameManager.Instance.enemyList.Count; i++) {
					if (GameManager.Instance.enemyList [i].activeInHierarchy) {
						GameManager.Instance.enemyList [i].GetComponent<EnemyContronller> ().lastEnemy = true;
						break;
					}
				}

//				for (int i = 0; i < GameManager.Instance.enemyList.Count; i++) {
//
//					if (!GameManager.Instance.enemyList [i].activeInHierarchy) {
//						count += 1;
//						if (count == GameManager.Instance.enemyList.Count) {
//							count = 0;
//							Time.timeScale = 0;
//							GameManager.Instance.completePanel.SetActive (true);
//						}
//					}
//				}
			}

			GameManager.Instance.DecreaseEnemy ();

			gameObject.SetActive (false);
		}
	}

	public void Destroy()
	{
		FillLevel ();
		gameObject.SetActive (false);
	}

	void FillLevel()
	{
		AddScore ();
		//fill level status
		if (GameManager.Instance.enemyKilled < GameManager.Instance.enemyPerLevel) {
			GameManager.Instance.enemyKilled += 1;
			UIManager.Instance.FillLevelImage ();

			if (GameManager.Instance.enemyKilled == GameManager.Instance.enemyPerLevel) {
				Time.timeScale = 0;
				//AdsControl.Instance.showAds ();
				
				GameManager.Instance.completePanel.SetActive (true);
				if (Audio.Instance != null) {
					Audio.Instance.source.Stop ();
					Audio.Instance.SoundEffect (0);
				}
			}
		}
		PlayerPrefs.SetInt ("totalkill", PlayerPrefs.GetInt ("totalkill") + 1);
	}

	///<Summary>
	///Reset chracter paramerter when it die
	///</Summary>

	void OnDisable()
	{
		Health ();
		healthBar.transform.localScale = new Vector3 (1, 1, 1);
		for (int i = 0; i < spr.Length; i++) {
			spr [i].sortingOrder = fistValue[i];
		}
	}

	void Health()
	{
		switch (_EnemyType) {
		case EnemyType.enemy1:
			health = 1f;
			break;
		case EnemyType.enemy2:
			health = 1.5f;
			break;
		case EnemyType.enemy3:
			health = 1.75f;
			break;
		case EnemyType.enemy4:
			health = 3.5f;
			break;
		case EnemyType.enemy5:
			health = 2.5f;
			break;
		case EnemyType.enemy6:
			health = 3f;
			break;
		default:
			break;
		}
	}
	void AddScore()
	{
		switch (_EnemyType) {
		case EnemyType.enemy1:
			GameManager.Instance.score += 5;
			PlayerPrefs.SetInt ("money", PlayerPrefs.GetInt ("money") + 1);
			break;
		case EnemyType.enemy2:
			GameManager.Instance.score += 10;
			PlayerPrefs.SetInt ("money", PlayerPrefs.GetInt ("money") + 2);
			break;
		case EnemyType.enemy3:
			GameManager.Instance.score += 15;
			PlayerPrefs.SetInt ("money", PlayerPrefs.GetInt ("money") + 3);
			break;
		case EnemyType.enemy4:
			GameManager.Instance.score += 20;
			PlayerPrefs.SetInt ("money", PlayerPrefs.GetInt ("money") + 4);
			break;
		case EnemyType.enemy5:
			GameManager.Instance.score += 25;
			PlayerPrefs.SetInt ("money", PlayerPrefs.GetInt ("money") + 5);
			break;
		case EnemyType.enemy6:
			GameManager.Instance.score += 30;
			PlayerPrefs.SetInt ("money", PlayerPrefs.GetInt ("money") + 6);
			break;
		default:
			break;
		}
		if (GameManager.Instance.score > PlayerPrefs.GetInt ("bestscore")) {
			PlayerPrefs.SetInt ("bestscore", GameManager.Instance.score);
		}
		UIManager.Instance.SetScore ();
		UIManager.Instance.SetMoneyText ();
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.tag == "escape1") {

			GameManager.Instance.DecreaseEnemy ();
			gameObject.SetActive (false);

		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		if (col.tag == "escapte") {
			if (GameManager.Instance.enemyEscape > 0) {
				
				GameManager.Instance.enemyEscape -= 1;
				UIManager.Instance.SetEscapeText ();
				if (GameManager.Instance.enemyEscape == 0) {
					Time.timeScale = 0;
					UIManager.Instance.SetResaultText ();
					GameManager.Instance.endPanel.SetActive (true);
				
					if (Audio.Instance != null) {
						Audio.Instance.SoundEffect (1);
					}
				}

//				if (GameManager.Instance.enemyGened == GameManager.Instance.enemyPerLevel && GameManager.Instance.enemyEscape > 0) {
//					if (lastEnemy) {
//						Time.timeScale = 0;
//						//AdsControl.Instance.showAds ();
//						GameManager.Instance.KillEnemy();
//						GameManager.Instance.completePanel.SetActive (true);
//					}
//				}

				if (Audio.Instance != null) {
					Audio.Instance.SoundEffect (13);
				}

				if (!lastEnemy) {
					GameManager.Instance.Warning ();
				} else {
					if (GameManager.Instance.enemyGened == GameManager.Instance.enemyPerLevel && GameManager.Instance.enemyEscape > 0) {
						Time.timeScale = 0;
						//AdsControl.Instance.showAds ();
						GameManager.Instance.KillEnemy();
						GameManager.Instance.completePanel.SetActive (true);
					}
				}
			}
		} else if (col.tag == "attacker") {
			getBoomHit = true;
		}
	}
}
