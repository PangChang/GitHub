using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class playSound : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioMixer masterMixer;

    // Start is called before the first frame update
    private void Start()
    {
        setVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
    }

    public void setVolume(float numValue)
    {
        if (numValue < 1)
        {
            numValue = .001f;
        }
        refreshSlider(numValue);
        PlayerPrefs.SetFloat("SavedMasterVolume", numValue);
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(numValue / 100)  * 20f);

    }

    public void sliderVolume()
    {
        setVolume(soundSlider.value);
    }

    public void refreshSlider(float value)
    {
        soundSlider.value = value;
    }
}
