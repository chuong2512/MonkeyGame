using UnityEngine;

public class Bullet : MonoBehaviour {
	public enum BulletType
	{
		bullet,
		boom
	}

	public BulletType _BulletType;
	public Vector3 target;
	public SpriteRenderer spr;
	public Sprite[] bulletSpr;
	public Sprite boomSpr;
	public Animator anim;
	void OnEnable ()
	{
		if (_BulletType == BulletType.bullet) {
			SetBulletType ();
		} 
	}

	void Update ()
	{
		if (_BulletType == BulletType.bullet) {
			if (target != Vector3.zero) {
				transform.position = Vector3.MoveTowards (transform.position, target, GameManager.Instance.gun [Menu.selectedGun].bulletSpeedLevel [GameManager.Instance.speedLevel - 1] * Time.deltaTime);
			}
			if (transform.position == target) {
				gameObject.SetActive (false);
			}
		} else if (_BulletType == BulletType.boom) {
			if (target != Vector3.zero) {
				transform.position = Vector3.MoveTowards (transform.position, target, 3*Time.deltaTime);
				transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
			}
			if (transform.position == target) {
				anim.Play ("boom");
			}
		}
	}


	void OnTriggerEnter2D (Collider2D col)
	{
		if (_BulletType == BulletType.bullet) {
			var hp = col.GetComponent<IHealth> ();
			if (hp == null)
				return;
			hp.Dame ();

			gameObject.SetActive (false);
		} else if (_BulletType == BulletType.boom) {
			var hp = col.GetComponent<IHealth> ();
			if (hp == null)
				return;
			hp.Destroy ();
		}
	}


	void SetBulletType()
	{
		spr.sprite = bulletSpr [Menu.selectedGun];
	}

	void FinishExplosion()
	{
		gameObject.SetActive (false);
	}

	void SoundExplosion()
	{
		if (Audio.Instance != null) {
			Audio.Instance.SoundEffect (12); 
		}

	}

	void OnDisable()
	{
		if (_BulletType == BulletType.boom) {
			spr.sprite = boomSpr;
			transform.localScale = new Vector3 (0.3f, 0.3f, 0);
		}
	}
}
