using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace HotMountain
{
    public class NeutralityNpcController : NpcController
    {
        [SerializeField]
        string _dialogueFile = "NPC1";

        [SerializeField]
        private float _dialogueDistance = 5f;

        [SerializeField]
        private Text _dialogue;

        private TextAsset _data;
        private StringReader _sr;
        private int _textCnt;

        void Awake()
        {
            Init();

            _instance = this;
            _useDialogue = false;

           // _data = Resources.Load(_dialogueFile, typeof(TextAsset)) as TextAsset;
           // _sr = new StringReader(_data.text);
        }

        public void SetAvailableDailogue(bool useDialogue)
        {
            _useDialogue = useDialogue;
        }

        private void ReadText()
        {
            string line = _sr.ReadLine();

            if (line == "start")
            {
                line = _sr.ReadLine();

                while (line == "@end")
                {
                    _dialogue.text += line;
                    line = _sr.ReadLine();
                }
            }
        }

        public void CheckDistanceWithPlayer()
        {
            bool useDialogue;
            Vector3 playerVec = _playerContoller.GetComponent<Transform>().position;

            if (Vector3.Distance(this.transform.position, playerVec) < _dialogueDistance)
                useDialogue = true;
            else
                useDialogue = false;

            SetAvailableDailogue(useDialogue);
        }
    }
}