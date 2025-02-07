using UnityEngine;
using System.Collections;
using LuongBaoToanTools;
//using DemoObserver;


public class Marine : MonoBehaviour
{
	#region Init, config

	[Header("Config marine")]
	/// Will rotate gun follow mouse position on screen
	[SerializeField] Transform gunTransform = null;
	/// The barrel position. Bullet will spawn at this position
	[SerializeField] Transform barrelPosition = null;
	/// Bullet prefab
	[SerializeField] GameObject bulletPrefab = null;

	Vector3 targetPos;

	void OnValidate()
	{
//		Common.Warning(gunTransform != null, "Marine is missing gunTransform !!");
//		Common.Warning(barrelPosition != null, "Marine is missing barrelPosition !!");
//		Common.Warning(barrelPosition != null, "Marine is missing bulletPrefab !!");
	}

	void Awake()
	{
		InvokeRepeating ("Shoot",0, 0.5f);
	}

	#endregion



	#region Working

	void Update ()
	{
		// rotate gun
		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = gunTransform.position.z;
		var direction = mousePos - gunTransform.position;
		gunTransform.up = direction;

		if (Input.GetMouseButtonDown (0) && GameManager.Instance.useBoom) {
			GameManager.Instance.useBoom = false;
			var dir = new Vector3( Camera.main.ScreenToWorldPoint (Input.mousePosition).x,Camera.main.ScreenToWorldPoint (Input.mousePosition).y,0);
			var boom = ObjectPooling.INSTANCE.GetObjectFromPoolList (ObjectPooling.INSTANCE.pooledBoom, gunTransform.position);
			boom.GetComponent<Bullet> ().target = dir;
		}

	}

	#endregion

	void Shoot()
	{
		targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 _dir = new Vector2 (targetPos.x - gunTransform.position.x, targetPos.y - gunTransform.position.y);
		RaycastHit2D hit = Physics2D.Raycast (new Vector2 (gunTransform.position.x, gunTransform.position.y), _dir, 200f, LayerMask.GetMask ("Enemy"));
		if (hit) {
			if (GameManager.Instance.startBulletValue > 0) {
				var bullet = ObjectPooling.INSTANCE.GetObjectFromPoolList (ObjectPooling.INSTANCE.pooledBullet, gunTransform.position);
				if (bullet != null) {
					//set bullet param
					bullet.GetComponent<Bullet> ().target = hit.point;
					float hitAngle;
					if (Input.mousePosition.x < Screen.width / 2f) {
						hitAngle = Vector2.Angle (Vector2.up, _dir);
					} else {
						hitAngle = -Vector2.Angle (Vector2.up, _dir);
					}
					bullet.transform.eulerAngles = new Vector3 (bullet.transform.eulerAngles.x, bullet.transform.eulerAngles.y, hitAngle);

					//set bullet text
					GameManager.Instance.startBulletValue -= 1;
					UIManager.Instance.SetBulletText ();
					if (GameManager.Instance.startBulletValue == 0) {
						GameManager.Instance.rechargeButton.SetActive (true);
					}

					GameManager.Instance.Shake ();
					if (Audio.Instance != null) {
						Audio.Instance.SoundEffect (10);
					}
				}
			}
		} else {
//			print ("no hit");
		}
	}
}