using UnityEngine;

public class Audio : MonoBehaviour {

	public static Audio Instance;
	public AudioClip menuClip,gameClip;
	public AudioSource source;
	public AudioClip[] effectClip;
	private	void Awake ()
	{
		if (Instance == null) {
			DontDestroyOnLoad (this.gameObject);
			Instance = this;
		} else {
			Destroy (gameObject);
		}
	}


	void Start ()
	{
		PlayBgMenu (0);	
	}

	public void PlayBgMenu(int index)
	{
		switch (index) {
		case 0:
			source.clip = menuClip;
			source.Play ();
			source.loop = true;
			break;
		case 1:
			source.clip = gameClip;
			source.Play ();
			source.loop = true;
			break;
		}


	}
	public void SoundEffect(int index)
	{
		source.PlayOneShot (effectClip[index]);
	}

}
