using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class textCoin : MonoBehaviour
{
	public TextMeshProUGUI diamondTmp;

	void OnValidate() { diamondTmp=GetComponent<TextMeshProUGUI>(); }
	// Start is called before the first frame update
	void Start() { }

	// Update is called once per frame
	void Update() { diamondTmp.SetText(PlayerPrefs.GetInt("money").ToString()); }
}
