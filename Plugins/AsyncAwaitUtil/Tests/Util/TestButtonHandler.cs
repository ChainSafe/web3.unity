using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityAsyncAwaitUtil
{
    public class TestButtonHandler
    {
        readonly Settings _settings;

        int _buttonVCount;
        int _buttonHCount;

        public TestButtonHandler(Settings settings)
        {
            _settings = settings;
        }

        public void Restart()
        {
            _buttonVCount = 0;
            _buttonHCount = 0;
        }

        public bool Display(string text)
        {
            if (_buttonVCount > _settings.NumPerColumn)
            {
                _buttonHCount++;
                _buttonVCount = 0;
            }

            var result = GUI.Button(
                new Rect(
                    _settings.HorizontalMargin + _buttonHCount * (_settings.ButtonWidth + _settings.HorizontalSpacing),
                    _settings.VerticalMargin + _buttonVCount * (_settings.ButtonHeight + _settings.VerticalSpacing),
                    _settings.ButtonWidth, _settings.ButtonHeight), text);

            _buttonVCount++;

            return result;
        }

        [Serializable]
        public class Settings
        {
            public int NumPerColumn = 6;
            public float VerticalMargin = 50;
            public float VerticalSpacing = 50;
            public float HorizontalSpacing = 50;
            public float HorizontalMargin = 50;
            public float ButtonWidth = 50;
            public float ButtonHeight = 50;
        }
    }
}
