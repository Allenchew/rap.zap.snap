using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DS4Input
{
	public class DS4InputTest : MonoBehaviour
	{

		private float vibrationL = 0;
		private float vibrationR = 0;
		private DS4Color ds4Color;
		enum PushType
		{
			Down,
			Up
		}

		private List<DS4ButtonType> buttonList = new List<DS4ButtonType>();
		private List<DS4AxisType> axisList = new List<DS4AxisType>();

		private void Start()
		{
			Array array = Enum.GetValues(typeof(DS4ButtonType));
			for (int i = 0, max = array.Length; i < max; i++)
			{
				buttonList.Add((DS4ButtonType) array.GetValue(i));
			}

			array = Enum.GetValues(typeof(DS4AxisType));
			for (int i = 0, max = array.Length; i < max; i++)
			{
				axisList.Add((DS4AxisType) array.GetValue(i));
			}
		}

		private void OnGUI()
		{
			Rect rect = new Rect(0,0,200,30);
			foreach (var button in buttonList)
			{
				GUI.Label(rect,string.Format("{0}:{1}",button,DS4Input.IsButton(DS4ControllerType.P1, button) ? PushType.Down : PushType.Up));
				rect.Set(0,rect.position.y + rect.size.y + 5,200,30);
			}
			rect.Set(rect.position.x + rect.width + 5,0,200,30);
			foreach (var axis in axisList)
			{
				GUI.Label(rect,string.Format("{0}:{1}",axis,DS4Input.IsAxis(DS4ControllerType.P1, axis)));
				rect.Set(rect.position.x,rect.position.y + rect.size.y + 5,200,30);
			}
			rect.Set(Screen.width - 200,0,200,30);
			GUI.Label(rect,"左モーター");
			rect.Set(rect.position.x,rect.position.y + rect.size.y + 5,200,30);
			vibrationL = GUI.Slider(rect, vibrationL, 1, 0, 255, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 0);
			rect.Set(rect.position.x,rect.position.y + rect.size.y + 5,200,30);
			GUI.Label(rect,"右モーター");
			rect.Set(rect.position.x,rect.position.y + rect.size.y + 5,200,30);
			vibrationR = GUI.Slider(rect, vibrationR, 1, 0, 255, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 1);
			rect.Set(rect.position.x,rect.position.y + rect.size.y + 5,200,30);
			if (GUI.Button(rect, "振動開始"))
			{
				DS4Input.SetVibration(DS4ControllerType.P1,new DS4Vibration((byte)vibrationR, (byte)vibrationL));
			}
			rect.Set(rect.position.x, rect.position.y + rect.size.y + 5,200,30);
			if (GUI.Button(rect, "振動停止"))
			{
				DS4Input.SetVibration(DS4ControllerType.P1,DS4Vibration.Min);
			}
			rect.Set(rect.position.x, rect.position.y + rect.size.y + 5,200,30);
			GUI.Label(rect,"R");
			rect.Set(rect.position.x, rect.position.y + rect.size.y + 5,200,30);
			ds4Color.red = (byte) GUI.Slider(rect, ds4Color.red, 1, 0, 255, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 2);
			rect.Set(rect.position.x, rect.position.y + rect.size.y + 5,200,30);
			GUI.Label(rect,"G");
			rect.Set(rect.position.x, rect.position.y + rect.size.y + 5,200,30);
			ds4Color.green = (byte) GUI.Slider(rect, ds4Color.green, 1, 0, 255, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 3);
			rect.Set(rect.position.x, rect.position.y + rect.size.y + 5,200,30);
			GUI.Label(rect,"B");
			rect.Set(rect.position.x, rect.position.y + rect.size.y + 5,200,30);
			ds4Color.blue = (byte) GUI.Slider(rect, ds4Color.blue, 1, 0, 255, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, true, 4);
			rect.Set(rect.position.x,rect.position.y + rect.size.y + 5,200,30);
			if (GUI.Button(rect, "色変更"))
			{
				DS4Input.SetColor(DS4ControllerType.P1,ds4Color);
			}
		}
	}
}
