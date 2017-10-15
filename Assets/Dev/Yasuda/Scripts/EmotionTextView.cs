using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class EmotionTextView : MonoBehaviour {
    //[SerializeField] float anger = 1.0f;

    public Text angerText;
    public Text contemptText;
    public Text disgustText;
    public Text fearText;
    public Text happinessText;
    public Text neutralText;
    public Text sadnessText;
    public Text supriseText;

    private Vector3 localScale;
    private float maxHeight = 50.0f;
    private float minHeight =  0.0f;

    // Use this for initialization
    void Start () {
        localScale = angerText.transform.localScale;
    }

    private Text GetTextComponent(string name)
    {
        return transform.Find(name).gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        //updateEmotion(anger, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
	}

    public void updateEmotion(float anger, float contempt, float disgust, float fear,
                              float happiness, float neutral, float sadness, float suprise)
    {
        anger = Mathf.Clamp(anger, 0.1f, 1.0f) + 0.01f ;
        contempt = Mathf.Clamp(contempt, 0.1f, 1.0f) + 0.01f;
        disgust = Mathf.Clamp(disgust, 0.1f, 1.0f) + 0.01f;
        fear = Mathf.Clamp(fear, 0.1f, 1.0f) + 0.01f;
        happiness = Mathf.Clamp(happiness, 0.1f, 1.0f) + 0.01f;
        neutral = Mathf.Clamp(neutral, 0.1f, 1.0f) + 0.01f;
        sadness = Mathf.Clamp(sadness, 0.1f, 1.0f) + 0.01f;
        suprise = Mathf.Clamp(suprise, 0.1f, 1.0f) + 0.01f;
        

        angerText.transform.localScale = localScale * anger;
        contemptText.transform.localScale = localScale * contempt;
        disgustText.transform.localScale = localScale * disgust;
        fearText.transform.localScale = localScale * fear;
        happinessText.transform.localScale = localScale * happiness;
        neutralText.transform.localScale = localScale * neutral;
        sadnessText.transform.localScale = localScale * sadness;
        supriseText.transform.localScale = localScale * suprise;
    }
}
