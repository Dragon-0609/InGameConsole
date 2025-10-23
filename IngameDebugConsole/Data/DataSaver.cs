using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global

namespace Data;

public static class DataSaver
{
	public const string GeneralSection = "General";
	public const string MiscSection = "Misc";
	private static List<ConfigEntry<KeyboardShortcut>> _keys;
	private static List<ConfigEntry<string>> _strings;
	private static List<ConfigEntry<int>> _ints;
	private static List<ConfigEntry<float>> _floats;
	private static List<ConfigEntry<bool>> _bools;
	private static List<ConfigEntry<Vector2>> _vector2s;

	private static List<string> _configs;

	static DataSaver()
	{
		_configs = new List<string>();
		_keys = new List<ConfigEntry<KeyboardShortcut>>();
		_strings = new List<ConfigEntry<string>>();
		_ints = new List<ConfigEntry<int>>();
		_floats = new List<ConfigEntry<float>>();
		_bools = new List<ConfigEntry<bool>>();
		_vector2s = new List<ConfigEntry<Vector2>>();
	}

	public static void Save(string key, bool value, string section = GeneralSection, string description = "")
	{
		ValidateConfig(key, value, section, description);
		SetValue(key, value);
	}

	public static void Save(string key, int value, string section = GeneralSection, string description = "")
	{
		ValidateConfig(key, value, section, description);
		SetValue(key, value);
	}

	public static void Save(string key, float value, string section = GeneralSection, string description = "")
	{
		ValidateConfig(key, value, section, description);
		SetValue(key, value);
	}

	public static void Save(string key, string value, string section = GeneralSection, string description = "")
	{
		ValidateConfig(key, value, section, description);
		SetValue(key, value);
	}

	public static void Save(string key, Vector2 value, string section = GeneralSection, string description = "")
	{
		ValidateConfig(key, value, section, description);
		SetValue(key, value);
	}

	public static void Save(string key, KeyboardShortcut value, string section = GeneralSection,
		string description = "")
	{
		ValidateConfig(key, value, section, description);
		SetValue(key, value);
	}

	public static bool Load(string key, bool defaultValue = true, string section = GeneralSection,
		string description = "")
	{
		ValidateConfig(key, defaultValue, section, description);
		return FindConfig(key, defaultValue).Value;
	}

	public static int Load(string key, int defaultValue = 0, string section = GeneralSection, string description = "")
	{
		ValidateConfig(key, defaultValue, section, description);
		return FindConfig(key, defaultValue).Value;
	}

	public static float Load(string key, float defaultValue = 0, string section = GeneralSection,
		string description = "")
	{
		ValidateConfig(key, defaultValue, section, description);
		return FindConfig(key, defaultValue).Value;
	}

	public static string Load(string key, string defaultValue = "", string section = GeneralSection,
		string description = "")
	{
		ValidateConfig(key, defaultValue, section, description);
		return FindConfig(key, defaultValue).Value;
	}

	public static Vector2 Load(string key, Vector2 defaultValue = default, string section = GeneralSection,
		string description = "")
	{
		ValidateConfig(key, defaultValue, section, description);
		return FindConfig(key, defaultValue).Value;
	}

	public static KeyboardShortcut Load(string key, KeyboardShortcut defaultValue, string section = GeneralSection,
		string description = "")
	{
		ValidateConfig(key, defaultValue, section, description);
		return FindConfig(key, defaultValue).Value;
	}

	public static KeyboardShortcut LoadShortcut(string key, string section = GeneralSection, string description = "")
	{
		return Load(key, default(KeyboardShortcut), section, description);
	}

	public static void Increment(string key, int defaultValue = 0)
	{
		Add(key, defaultValue);
	}

	public static void Decrement(string key, int defaultValue = 0)
	{
		Decrease(key, defaultValue);
	}

	public static void Add(string key, int defaultValue, int add = 1)
	{
		Save(key, Load(key, defaultValue) + add);
	}

	public static void Decrease(string key, int defaultValue, int decrease = 1, bool minZero = false)
	{
		int value = Load(key, defaultValue) - decrease;
		if (minZero) value = Mathf.Max(value, 0);
		Save(key, value);
	}


	private static void ValidateKey(string key)
	{
		if (string.IsNullOrEmpty(key)) throw new InvalidOperationException($"Key is empty. Key is '{key}'");
	}

	public static void ValidateConfig<T>(string key, T value, string section = GeneralSection, string description = "")
	{
		ValidateKey(key);
		if (!_configs.Contains(key))
		{
			_configs.Add(key);
			ConfigEntry<T> config = Plugin.Instance.Config.Bind(section, key, value, description);
			IList list = GetList(value);
			list.Add(config);
		}
	}

	public static void ValidateConfig<T>(string key, T value, string section = GeneralSection, string description = "",
		params T[] values) where T : IEquatable<T>
	{
		ValidateKey(key);
		if (!_configs.Contains(key))
		{
			_configs.Add(key);
			ConfigDescription configDescription =
				new ConfigDescription(description, new AcceptableValueList<T>(values));
			ConfigEntry<T> config = Plugin.Instance.Config.Bind(section, key, value, configDescription);
			IList list = GetList(value);
			list.Add(config);
		}
	}

	private static List<ConfigEntry<T>> GetList<T>(T value)
	{
		IList list = value switch
		{
			string => _strings,
			int => _ints,
			float => _floats,
			bool => _bools,
			Vector2 => _vector2s,
			KeyboardShortcut => _keys,
			_ => null
		};

		if (list == null)
		{
			throw new InvalidOperationException(
				$"Config is invalid. Value was {typeof(T).Name} type. Was expecting string, int, float, bool or KeyboardShortcut");
		}

		return list as List<ConfigEntry<T>>;
	}

	private static void SetValue<T>(string key, T value)
	{
		ConfigEntry<T> foundKey = FindConfig(key, value);
		foundKey.Value = value;
	}

	private static ConfigEntry<T> FindConfig<T>(string key, T value)
	{
		List<ConfigEntry<T>> list = GetList(value);
		var foundKey = list.FirstOrDefault(s => s.Definition.Key == key);
		if (foundKey == null) throw new InvalidOperationException($"{key} config was not found");
		return foundKey;
	}
}