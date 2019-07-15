﻿using UnityEngine;
using UnityEngine.UI;

namespace Gruel.Console.Obelisk {
	public class ObeliskLog : MonoBehaviour {
		
#region Properties
		public ConsoleLog ConsoleLog { get; private set; }

		public RectTransform RectTransform => _rectTransform;
#endregion Properties

#region Fields
		[SerializeField] private RectTransform _rectTransform;
		[SerializeField] private Image _background;
		[SerializeField] private Button _button;
		[SerializeField] private Text _text;
		[SerializeField] private GameObject _stackTraceGameObject;
		[SerializeField] private Image _stackTraceImage;

		private const float HeightIncrement = 20.0f;

		private ObeliskConsole _obeliskConsole;
#endregion Fields

#region Public Methods
		public void Init(ObeliskConsole obeliskConsole) {
			_obeliskConsole = obeliskConsole;
		}
		
		public void SetLog(ref ConsoleLog log) {
			ConsoleLog = log;

			if (log.CustomColor) {
				SetColors(log.TextColor, log.BgColor);
			} else {
				SetColors(log.LogType);
			}

			_text.text = log.LogString;

			if (string.IsNullOrEmpty(ConsoleLog.StackTrace) == false) {
				_stackTraceGameObject.SetActive(true);
			}
		}
#endregion Public Methods

#region Private Methods
		private void Awake() {
			_stackTraceImage.color = ObeliskConsole.ColorSet.IconColor;
			_button.onClick.AddListener(OnButtonClicked);
		}
		
		private void OnButtonClicked() {
			if (ConsoleLog.StackTrace == "") {
				return;
			}

			_obeliskConsole.OpenStackTraceForLog(ConsoleLog);
		}

		private void OnRectTransformDimensionsChange() {
			var heightMultiple = Mathf.Ceil(_text.preferredHeight / HeightIncrement);
			var newHeight = heightMultiple * HeightIncrement;

			RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, newHeight);
		}

		private void SetColors(LogType logType) {
			var backgroundColor = ObeliskConsole.ColorSet.LogBackgroundColor(logType);

			if ((transform.parent.childCount % 2) == 0) {
				backgroundColor += new Color(0.015f, 0.015f, 0.015f, 1.0f);
			}

			_background.color = backgroundColor;
			_text.color = ObeliskConsole.ColorSet.LogTextColor(logType);
		}

		private void SetColors(Color textColor, Color backgroundColor) {
			_background.color = backgroundColor;
			_text.color = textColor;
		}


#endregion Private Methods
		
	}
}