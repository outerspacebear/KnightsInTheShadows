using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public void OnNextButtonPressed()
    {
        //Is final image
        if(currentImageIndex == tutorialImages.Count - 1)
        {
            SlideOut();
            currentImageIndex = 0;
        }
        else
        {
            tutorialImage.sprite = tutorialImages[++currentImageIndex];
        }

        UpdateButtons();
    }

    public void OnPrevButtonPressed()
    {
        if(currentImageIndex != 0)
        {
            tutorialImage.sprite = tutorialImages[--currentImageIndex];
            UpdateButtons();
        }
    }

    public void DisplayTutorialIfFirstTime()
    {
        if (PlayerPrefs.GetInt("isFirstTime", 1) == 1)
        {
            SlideIn();
            PlayerPrefs.SetInt("isFirstTime", 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentImageIndex = 0;
        tutorialImage.sprite = tutorialImages[currentImageIndex];
        UpdateButtons();
    }

    void SlideIn()
    {
        GetComponent<UIScroller>().MoveToTarget();
    }

    void SlideOut()
    {
        GetComponent<UIScroller>().MoveToOriginalPosition();
    }

    void UpdateButtons()
    {
        bool isFirstImage = currentImageIndex == 0;
        prevButton.interactable = isFirstImage ? false : true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    Image tutorialImage;
    [SerializeField]
    Button prevButton;
    [SerializeField]
    Button nextButton;
    [SerializeField]
    List<Sprite> tutorialImages;

    int currentImageIndex = 0;
}
