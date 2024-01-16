using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenManager : MonoSingleton<LoadingScreenManager>
{
    [SerializeField] GameObject loadingScreenGroup;
    [SerializeField] Image loadingScreenImage;
    [SerializeField] Image fader;
    [SerializeField] TextMeshProUGUI loadingPercentText;
    [SerializeField] Sprite[] loadingSprites = null; // array of loading screen sprites
    [SerializeField] float loadTime = 2f;
    [SerializeField] TMP_InputField nameField;

    private AsyncOperation _asyncOperation;
    private bool _isLoading = false;

    private void Awake()
    {
        HideLoadingScreenElements();
    }

    public void LoadScene(string sceneName)
    {
        if (!_isLoading)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        _isLoading = true;

        // Show loading screen elements
        ShowLoadingScreenElements();

        // randomly select a sprite from the array
        if (loadingSprites != null && loadingSprites.Length > 0)
        {
            int index = Random.Range(0, loadingSprites.Length);
            if (loadingScreenImage) loadingScreenImage.sprite = loadingSprites[index];
        }

        _asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        _asyncOperation.allowSceneActivation = false;

        // Wait for a short delay to show the loading screen elements
        float delayTime = 0.5f;
        yield return new WaitForSeconds(delayTime);

        // Start the scene loading process
        float elapsedTime = 0f;
        while (!_asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(_asyncOperation.progress / 0.9f);
            if (fader) fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, progress);
            if (loadingPercentText) loadingPercentText.text = Mathf.RoundToInt(progress * 100f).ToString() + "%";

            if (_asyncOperation.progress >= 0.9f)
            {
                // If the scene is almost loaded, wait for the fader to fade out before allowing scene activation
                float fadeOutTime = 0.5f;
                while (elapsedTime < fadeOutTime)
                {
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                _asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        // Wait until the scene is fully loaded
        yield return new WaitUntil(() => _asyncOperation.isDone);

        // Hide loading screen elements
        HideLoadingScreenElements();
        _isLoading = false;
    }


    void ShowLoadingScreenElements()
    {
        // Show loading screen elements
        if (loadingScreenGroup) loadingScreenGroup.SetActive(true);
        if (fader)
        {
            fader.gameObject.SetActive(true);
            fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, 1f); // Set the fader alpha to 1
        }
        if (loadingPercentText)
        {
            loadingPercentText.gameObject.SetActive(true);
            loadingPercentText.text = "0%"; // Set the initial percent to 0
        }
    }

    void HideLoadingScreenElements()
    {
        if (loadingScreenGroup) loadingScreenGroup.SetActive(false);
        if (fader)fader.gameObject.SetActive(false);
        if (loadingPercentText) loadingPercentText.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }    
}
