using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// TODO: Get rid of pages and use scenes with ui already setup

namespace BackToLife
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageWindow;
        private List<Message> _messages;
        private bool _msgActive;

        public void ShowMessage(string msg, MsgType type)
        {
            _messages.Add(new Message{text = msg,type = type});
        }

        private void DisplayMsg(Message msg)
        {
            messageWindow.text = msg.text;
            messageWindow.gameObject.SetActive(true);
            _msgActive = true;
            StartCoroutine(DisplayMsgFor(2f));
            _messages.Remove(msg);
        }
        
        private IEnumerator DisplayMsgFor(float time)
        {
            yield return new WaitForSeconds(time);
            _msgActive = false;
            messageWindow.gameObject.SetActive(false);
        }

        private void Awake()
        {
            _msgActive = false;
            _messages = new List<Message>();
        }

        private void Update()
        {
            if (_msgActive) return;
            if (_messages.Count < 1) return;
            DisplayMsg(_messages[0]);
        }

        [Serializable]
        private struct Message
        {
            public string text;
            public MsgType type;
        }
        
        public enum MsgType
        {
            Info,
            Warning,
            Error
        }
    }
}