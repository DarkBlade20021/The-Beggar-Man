using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuControl : MonoBehaviour
{
	public TMP_Dropdown resolutionDropdown;
	public GameObject PostProcessingObj;
	public int isBloomedInt;

	Resolution[] resolutions;
	
	void Start()
	{
		resolutions = Screen.resolutions;
		
		resolutionDropdown.ClearOptions();
		
		List<string> options = new List<string>();
		
		int currentResolutionIndex = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " x " + resolutions[i].height;
			options.Add(option);
			
			if (resolutions[i].width == Screen.currentResolution.width &&
				resolutions[i].height == Screen.currentResolution.height)
			{
				currentResolutionIndex = i;
			}
		}
		
		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue();
		
		isBloomedInt = PlayerPrefs.GetInt("PostProcessing");

		if (isBloomedInt == 1)
			PostProcessingObj.SetActive(true);
		else if (isBloomedInt == 0)
			PostProcessingObj.SetActive(false);
	}

	public void SetResolution (int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

	public void SetQuality (int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
	}
	
	public void SetFullscreen (bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}

	public void SetPostProcessing(bool isBloomed)
    {
		if (isBloomed)
			isBloomedInt = 1;
		else
			isBloomedInt = 0;
		PlayerPrefs.SetInt("PostProcessing", isBloomedInt);
		if (PlayerPrefs.GetInt("PostProcessing") == 1)
			PostProcessingObj.SetActive(true);
		else if (PlayerPrefs.GetInt("PostProcessing") == 0)
			PostProcessingObj.SetActive(false);
	}

}
