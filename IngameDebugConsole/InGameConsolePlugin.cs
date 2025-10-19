using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using KKAPI.Studio;
using KKAPI.Utilities;
using UnityEngine;
using static PluginInfo;

[BepInPlugin(PACKAGE_NAME, PLUGIN_NAME, PLUGIN_VERSION)]
[BepInProcess(StudioName)]
public class Plugin : BaseUnityPlugin
{
	private const string StudioName = "CharaStudio.exe";
	public static Plugin Instance;

	internal static new ManualLogSource Logger;

	private static string _currentDirectory = "";

	private string _assetBundleName = "ingameconsole";

	private bool _isStudio;
	
	private void Awake()
	{
		if (!StudioAPI.InsideStudio) return;
		DetermineGameTarget();
		if (!_isStudio) return;

		Instance = this;

		Logger = base.Logger;
		Logger.LogInfo($"Plugin {PLUGIN_NAME} is loaded");

		Logger.LogInfo($"Loading bundle: {_assetBundleName}");
		
		AssetBundle assetBundle = LoadAssetBundle(_assetBundleName);

		var prefab = assetBundle.LoadAsset<GameObject>("IngameDebugConsole");
		GameObject instance = Instantiate(prefab, null, true);

		Logger.LogInfo($"Instantiated {prefab.name}");
	}

	private AssetBundle LoadAssetBundle(string bundleName)
	{
		Stream bundleResource = GetAssetBundleResource(bundleName);
		Debug.Log($"Is Bundle null: {bundleResource == null}");
		
		var bytes = bundleResource.ReadAllBytes();
		AssetBundle assetBundle = AssetBundle.LoadFromMemory(bytes);
		bundleResource.Dispose();
		return assetBundle;
	}

	private void DetermineGameTarget()
	{
		switch (Application.productName)
		{
			case "Koikatsu Party":
			case "Koikatu":
			case "KoikatuVR":
				_isStudio = false;
				break;
			case "CharaStudio":
				_isStudio = true;
				break;
		}
	}
	
	
	private static Stream GetAssetBundleResource(string name)
	{
		Debug.Log($"Loading Bundle: {"InGameDebugConsole.AssetBundles." + name}");
		return Assembly.GetExecutingAssembly().GetManifestResourceStream("InGameDebugConsole.AssetBundles." + name);
	}
}