using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {

	private static UIManager _instance;
	public static UIManager Instance { get { return _instance; } }

    public TextMeshProUGUI lives;
	public TextMeshProUGUI score;
	public TextMeshProUGUI time;

	private void Awake () {
		if (_instance != null && _instance != this) {
			Destroy (this.gameObject);
		}
		else {
			_instance = this;
		}
	}

	// Update is called once per frame
	void Update () {
		this.lives.SetText (World.Instance.gameManager.LivesForDisplay ());
		this.score.SetText (World.Instance.gameManager.ScoreForDisplay ());
		this.time.SetText (World.Instance.gameManager.TimeForDisplay ());
	}
}
