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
