﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour {

    [Header("Loading Visuals")]
    public Image loadingIcon;
    public Image loadingDoneIcon;
    public Text loadingText;
    public Image progressBar;
    public Image fadeOverlay;

    [Header("Timing Settings")]
    public float waitOnLoadEnd = 0.25f;
    public float fadeDuration = 0.25f;

    [Header("Loading Settings")]
    public LoadSceneMode loadSceneMode = LoadSceneMode.Single;
    public ThreadPriority loadThreadPriority;

    [Header("Other")]
    // If loading additive, link to the cameras audio listener, to avoid multiple active audio listeners
    public AudioListener audioListener;

    AsyncOperation operation;
    Scene currentScene;

    public static string sceneToLoad = "";
    // IMPORTANT! This is the build index of your loading scene. You need to change this to match your actual scene index
    static string loadingSceneName = "Loading";

    public static void LoadScene(string levelName)
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        sceneToLoad = levelName;
        SceneManager.LoadScene(loadingSceneName);
    }

    void Start()
    {
        if (sceneToLoad != "")
            return;

        fadeOverlay.gameObject.SetActive(true); // Making sure it's on so that we can crossfade Alpha
        currentScene = SceneManager.GetActiveScene();
        StartCoroutine(LoadAsync(sceneToLoad));
    }

    private IEnumerator LoadAsync(string levelName)
    {
        ShowLoadingVisuals();

        yield return null;

        FadeIn();
        StartOperation(levelName);

        float lastProgress = 0f;

        // operation does not auto-activate scene, so it's stuck at 0.9
        while (DoneLoading() == false)
        {
            yield return null;

            if (Mathf.Approximately(operation.progress, lastProgress) == false)
            {
                //progressBar.fillAmount = operation.progress;
                lastProgress = operation.progress;
            }
        }

        if (loadSceneMode == LoadSceneMode.Additive)
            audioListener.enabled = false;

        ShowCompletionVisuals();

        yield return new WaitForSeconds(waitOnLoadEnd);

        FadeOut();

        yield return new WaitForSeconds(fadeDuration);

        if (loadSceneMode == LoadSceneMode.Additive)
            SceneManager.UnloadScene(currentScene.name);
        else
            operation.allowSceneActivation = true;
    }

    private void StartOperation(string leveName)
    {
        Application.backgroundLoadingPriority = loadThreadPriority;
        operation = SceneManager.LoadSceneAsync(leveName, loadSceneMode);


        if (loadSceneMode == LoadSceneMode.Single)
            operation.allowSceneActivation = false;
    }

    private bool DoneLoading()
    {
        return (loadSceneMode == LoadSceneMode.Additive && operation.isDone) || (loadSceneMode == LoadSceneMode.Single && operation.progress >= 0.9f);
    }

    void FadeIn()
    {
        fadeOverlay.CrossFadeAlpha(0, fadeDuration, true);
    }

    void FadeOut()
    {
        fadeOverlay.CrossFadeAlpha(1, fadeDuration, true);
    }

    void ShowLoadingVisuals()
    {
        loadingIcon.gameObject.SetActive(true);
        loadingDoneIcon.gameObject.SetActive(false);

        //progressBar.fillAmount = 0f;
        loadingText.text = "LOADING...";
    }

    void ShowCompletionVisuals()
    {
        loadingIcon.gameObject.SetActive(false);
        loadingDoneIcon.gameObject.SetActive(true);

       // progressBar.fillAmount = 1f;
        loadingText.text = "LOADING DONE";
    }
}
