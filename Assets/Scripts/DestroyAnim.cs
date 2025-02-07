using UnityEngine;

public class DestroyAnim : MonoBehaviour {

	public Animator anim;
	public SpriteRenderer spr;

	public void ShakeCompete()
	{
		anim.enabled = false;
	}

	public void DestroyAni()
	{
		gameObject.SetActive (false);
	}

	void OnEnable()
	{
//		spr.color = new Color32 (255, 255, 255, 255);
	}
}
