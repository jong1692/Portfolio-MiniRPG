using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotMountain
{
    public class GameOption : MonoBehaviour
    {
        [SerializeField]
        private GameObject _gameOption;

        private bool _paused = false;

        void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                _paused = !_paused;
            }

            if (_paused)
            {
                _gameOption.SetActive(true);
                Time.timeScale = 0;
            }

            if (!_paused)
            {
                _gameOption.SetActive(false);
                Time.timeScale = 1f;
            }
        }

        public void Return()
        {
            _paused = !_paused;
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}