using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public static UIManager Instance;
	[SerializeField] Text bulletText;
	[SerializeField] Text score;
	[SerializeField] Text enemyEscape;
	[SerializeField] Text levelText;
	[SerializeField] Image fillLevelImage;
	[SerializeField] Text dameLevel, armorLevel, speedLevel;
	[SerializeField] Text scoreText, bestScoreText;
	[SerializeField] RectTransform left,right;
	bool startFill;
	public bool startFill1;
	[SerializeField] float fillSpeed;
	[SerializeField] Text boomText;
	public Image AudioSpr,AudioSpr1;
	public Sprite AudioOn, AudioOff;

	public Text moneyText;

	public Button upgradeDameBtn, upgradeBulletSpeedBtn;
	private	void Awake ()
	{
		Instance = this;	
		SetAnchored ();
		SetBomText ();
	}
	IEnumerator Start()
	{
		if (Audio.Instance != null) {
			Audio.Instance.PlayBgMenu (1);
			Audio.Instance.SoundEffect (11);
		}
		yield return new WaitForEndOfFrame ();

		startFill = true;
	}
	public void SetBulletText()
	{
		bulletText.text = GameManager.Instance.startBulletValue.ToString ();
	}
	public void SetScore()
	{
		score.text = GameManager.Instance.score.ToString ();
	}

	public void SetEscapeText()
	{
		enemyEscape.text = GameManager.Instance.enemyEscape.ToString ();
	}
	public void SetLevelText()
	{
		levelText.text = "Level: " + (GameManager.Instance.currentLevel + 1).ToString ();
	}

	public void FillLevelImage()
	{
		fillLevelImage.fillAmount = (float)GameManager.Instance.enemyKilled / GameManager.Instance.enemyPerLevel;
	}

	public void SetUpgradeText()
	{
		dameLevel.text = GameManager.Instance.dameLevel.ToString ();
		armorLevel.text = GameManager.Instance.armorLevel.ToString ();
		speedLevel.text = GameManager.Instance.speedLevel.ToString ();
	}

	public void SetResaultText()
	{
		scoreText.text = GameManager.Instance.score.ToString ();
		bestScoreText.text = PlayerPrefs.GetInt ("bestscore").ToString ();
	}
	private void SetAnchored()
	{
		left.anchoredPosition = new Vector2 (-600, 0);
		right.anchoredPosition = new Vector2 (-200, 0);
	}
	public void SetBomText()
	{
		boomText.text = PlayerPrefs.GetInt ("boom").ToString ();
	}

	public void SetMoneyText()
	{
		moneyText.text = PlayerPrefs.GetInt ("money").ToString ();
	}
	void Update ()
	{
		if (startFill) {
			left.anchoredPosition = new Vector2 (left.anchoredPosition.x-fillSpeed, 0);
			right.anchoredPosition = new Vector2 (right.anchoredPosition.x+fillSpeed, 0);
			if (right.anchoredPosition.x == 600) {
				startFill = false;
			}
		}
		if (startFill1) {
			left.anchoredPosition = new Vector2 (left.anchoredPosition.x+fillSpeed, 0);
			right.anchoredPosition = new Vector2 (right.anchoredPosition.x-fillSpeed, 0);
			if (right.anchoredPosition.x == -200) {
				startFill1 = false;
				Application.LoadLevel (0);
			}
		}
	}

	public void FullUpgradeDame()
	{
		upgradeDameBtn.interactable = false;
	}

	public void FullUpgradeSpeed()
	{
		upgradeBulletSpeedBtn.interactable = false;
	}
}
