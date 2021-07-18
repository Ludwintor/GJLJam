using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GJLJam
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject exit;

        [SerializeField]
        private float fadingTime;
        [SerializeField]
        private Image blackImage;

        private Coroutine starting;

        private void Start()
        {
#if UNITY_WEBGL
            exit.SetActive(false);
#endif
        }

        public void Play()
        {
            if (starting == null)
                starting = StartCoroutine(HandleStart());
        }

        public void Exit()
        {
            Application.Quit();
        }

        private IEnumerator HandleStart()
        {
            float currentTime = 0;
            while (currentTime < fadingTime)
            {
                currentTime += Time.deltaTime;
                blackImage.color = new Color(0, 0, 0, currentTime / fadingTime);
                yield return null;
            }

            SceneManager.LoadScene(1);
        }
    }
}