using System;
using System.Text;
using HappyUnity.Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HappyUnity.UI
{
    public abstract class ScreenLogger : ScriptableObject
    {
        private const int Lines = 18;

        private static readonly string[] LogCache = new string[Lines];
        private static string _outPut = string.Empty;

        private Rect border;
        private GUIStyle _logArea;

        public virtual void PushLogMessage(string message)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < LogCache.Length - 1; i++)
            {
                LogCache[i] = LogCache[i + 1];
                builder.AppendLine(LogCache[i]);
            }

            LogCache[LogCache.Length - 1] = message;
            builder.AppendLine(message);
            _outPut = builder.ToString();
        }

        public virtual void PushTextMessage(in TextMeshProUGUI text, in string message)
        {
            if (text)
                text.text = message;
        }

        public virtual void OnGUI()
        {
            _logArea.wordWrap = true;
            _logArea.clipping = TextClipping.Overflow;
            _logArea.normal.textColor = new Color(0.5f, .3f, 1, 0.9f);
            _logArea.fontSize = 16;

            GUI.Label(border, _outPut, _logArea);
        }

        private void Start()
        {
            border = new Rect(1, 1, Screen.width + 10, Screen.height + 10);
            _logArea = new GUIStyle();
        }
    }
}