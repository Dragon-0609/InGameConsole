using System;
using System.Collections;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace InGameDebugConsole;

public class WindowScaler : MonoBehaviour
{
	[Range(0.5f, 2f)] public float UIScale = 1f;

	private CanvasScaler _scaler;
	private float _baseScaleFactor;
	private Canvas _canvas;
	private float _prevScale = 1f;
	private bool _initedScaleFactor = false;

	private void Awake()
	{
		_scaler = GetComponent<CanvasScaler>();
		_canvas = _scaler.GetComponent<Canvas>();

		DataSaver.ValidateConfig("UIScale", 1f, description: "Scale Factor of UI", values:
		[
			0.5f,
			0.75f,
			1f,
			1.25f,
			1.5f,
			1.75f,
			2f
		]);
	}

	private IEnumerator Start()
	{
		yield return null;
		_baseScaleFactor = _canvas.scaleFactor;
		_initedScaleFactor = true;
		ApplyScale();
	}

	private void Update()
	{
		if (!_initedScaleFactor) return;

		UIScale = DataSaver.Load("UIScale", 1f);
		if (!Mathf.Approximately(_prevScale, UIScale))
			ApplyScale();
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (scaler == null)
		{
			scaler = GetComponent<CanvasScaler>();
			_canvas = scaler.GetComponent<Canvas>();
		}
		ApplyScale();
	}
#endif
	
	

	private void ApplyScale()
	{
		_prevScale = UIScale;
		_canvas.scaleFactor = _baseScaleFactor * UIScale;
	}
}