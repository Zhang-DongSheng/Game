using System;
using UnityEditor;
using UnityEngine;

public class PlayerPrefsWindow : EditorWindow
{
	private readonly string label_key = "键：";

	private readonly string label_value = "值：";

	private readonly string label_copy = "复制";

	private readonly string label_delete = "删除";

	private PlayerPrefPair[] m_data;

	private Vector2 scroll;

	[MenuItem("Other/PlayerPrefs/Open")]
	private static void Open()
	{
		EditorWindow window = EditorWindow.GetWindow<PlayerPrefsWindow>();
		window.minSize = Vector2.one * 300;
		window.titleContent = new GUIContent("PlayerPrefs");
	}

	private void Awake()
	{
		m_data = PlayerPrefsExtension.GetAll();
	}

	private void OnGUI()
	{
		scroll = EditorGUILayout.BeginScrollView(scroll);
		{
			for (int i = 0; i < m_data.Length; i++)
			{
				DrawItem(m_data[i].Key, m_data[i].Value.ToString());
			}
		}
		EditorGUILayout.EndScrollView();
	}

	private void DrawItem(string key, string value)
	{
		EditorGUILayout.Space();

		GUILayout.BeginHorizontal();
		{
			GUILayout.Label(label_key + key, GUILayout.Width(240));

			GUILayout.Label(label_value);

			string variable = value;

			variable = GUILayout.TextField(variable);

			if (GUILayout.Button(label_copy, GUILayout.Width(100)))
			{
				GUIUtility.systemCopyBuffer = variable;
			}

			if (GUILayout.Button(label_delete, GUILayout.Width(100)))
			{
				PlayerPrefs.DeleteKey(key);

				m_data = PlayerPrefsExtension.GetAll();
			}
		}
		GUILayout.EndHorizontal();
	}
}

public static class PlayerPrefsExtension
{
	public static PlayerPrefPair[] GetAll()
	{
		return GetAll(PlayerSettings.companyName, PlayerSettings.productName);
	}

	private static PlayerPrefPair[] GetAll(string companyName, string productName)
	{
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			// From Unity docs: On Windows, PlayerPrefs are stored in the registry under HKCU\Software\[company name]\[product name] key, where company and product names are the names set up in Project Settings.
#if UNITY_5_5_OR_NEWER
			Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Unity\\UnityEditor\\" + companyName + "\\" + productName);
#else
            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\" + companyName + "\\" + productName);
#endif
			if (registryKey != null)
			{
				string[] valueNames = registryKey.GetValueNames();

				PlayerPrefPair[] tempPlayerPrefs = new PlayerPrefPair[valueNames.Length];

				int i = 0;

				foreach (string valueName in valueNames)
				{
					string key = valueName;

					int index = key.LastIndexOf("_");

					key = key.Remove(index, key.Length - index);

					object ambiguousValue = registryKey.GetValue(valueName);

					if (ambiguousValue.GetType() == typeof(int))
					{
						if (PlayerPrefs.GetInt(key, -1) == -1 && PlayerPrefs.GetInt(key, 0) == 0)
						{
							ambiguousValue = PlayerPrefs.GetFloat(key);
						}
					}
					else if (ambiguousValue.GetType() == typeof(byte[]))
					{
						ambiguousValue = System.Text.Encoding.Default.GetString((byte[])ambiguousValue);
					}

					tempPlayerPrefs[i] = new PlayerPrefPair() { Key = key, Value = ambiguousValue };

					i++;
				}

				return tempPlayerPrefs;
			}
			else
			{
				return new PlayerPrefPair[0];
			}
		}
		else
		{
			throw new NotSupportedException("PlayerPrefsEditor doesn't support this Unity Editor platform");
		}
	}
}

[System.Serializable]
public struct PlayerPrefPair
{
	public string Key { get; set; }

	public object Value { get; set; }
}