using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EmoLensManager : MonoBehaviour {
    private CognitiveCamera _CognitiveCamera;

	// Use this for initialization
	void Start () {
        _CognitiveCamera = GameObject.Find("_Emolens").GetComponent<CognitiveCamera>();
        Assert.IsNotNull(_CognitiveCamera);

        _CognitiveCamera.onDataReceived.AddListener(OnDataReceived);
    }

    private void OnDataReceived(string response)
    {
        // Json request is got when it arrives.
        Debug.Log(_CognitiveCamera.response);
    }
}
