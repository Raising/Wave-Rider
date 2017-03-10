using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

[RequireComponent(typeof(Text))]
public class Test : MonoBehaviour
{
	[Tooltip("How many items will be show")]
	[SerializeField]
	private uint stackSize = 10;
	[Tooltip("Should printing be colorful (white for debug, yellow for warning, red for errors)")]
	[SerializeField]
	private bool debugColorLogType = false;

	private Text text = null;
	private IList<string> list = new List<string> ();
	private StringBuilder sb = new StringBuilder();
	private System.Action stackDelegate;

	private void Awake()
	{
		Application.logMessageReceived += HandlelogMessageReceived;
		this.text = this.gameObject.GetComponent<Text> ();
		SetText ();
	}

	private void OnDestroy()
	{
		Application.logMessageReceived -= HandlelogMessageReceived;
	}

	private void HandlelogMessageReceived (string message, string stackTrace, LogType type)
	{
		if (this.debugColorLogType == true)
		{
			message = SetColorMessage(message, type);
		}

		this.list.Add(message);
		if(this.list.Count >= this.stackSize)
		{
			this.list.RemoveAt(0);
		}
		sb.Length = 0;
		stackDelegate ();
		this.text.text = sb.ToString();
	}

	private void LowStacking()
	{
		for(int i = 0; i < this.list.Count ;i++)
		{
			sb.Append(list[i]);
			if(i != this.list.Count -1)
			{
				sb.Append("\n");
			}
		}
	}
	private void HighStacking()
	{
		for(int i = list.Count - 1; i >= 0 ; i--)
		{
			sb.Append(list[i] +"\n");
		}
	}

	private string SetColorMessage (string message, LogType type)
	{
		sb.Append ("<color=");
		switch (type)
		{
		case LogType.Assert:
			sb.Append("blue>");
			break;
		case LogType.Error:
			sb.Append("red>");
			break;
		case LogType.Exception:
			sb.Append("red>");
			break;
		case LogType.Log:
			sb.Append("white>");
			break;
		case LogType.Warning:
			sb.Append("yellow>");
			break;
		}
		sb.Append (message);sb.Append ("</color>");
		return sb.ToString ();
	}

	private void SetText()
	{
		if (this.text == null) { return; }
		this.text.text = "";
		TextAnchor ta = this.text.alignment;
		if (ta == TextAnchor.LowerCenter || ta == TextAnchor.LowerLeft || ta == TextAnchor.LowerRight)
		{
			stackDelegate = LowStacking;
		}
		else if (ta == TextAnchor.UpperCenter || ta == TextAnchor.UpperLeft || ta == TextAnchor.UpperRight)
		{
			stackDelegate = HighStacking;
		}
		else
		{
			stackDelegate = LowStacking;
		}
	}
}