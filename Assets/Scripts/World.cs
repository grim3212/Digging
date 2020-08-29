using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    private static World _instance;
	public static World Instance { get { return _instance; } }
    public string appPath;

    private void Awake () {
		if (_instance != null && _instance != this) {
			Destroy (this.gameObject);
		}
		else {
			_instance = this;
		}

		appPath = Application.persistentDataPath;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
