using UnityEditor;
using UnityEngine;

public class Assistant : MonoBehaviour
{
	[SerializeField] private bool _debugLog;
	[SerializeField] private bool _debugRay;
	[SerializeField] private bool _destroyRoutes;

	public static bool DebugLog;
	public static bool DebugRay;
	public static bool DestroyRoutes;

	[HideInInspector] public static Assistant Harry;

	private void Awake()
	{
		if (Harry == null) { Harry = this; }

		DestroyRoutes = _destroyRoutes;
	}
	private void FixedUpdate()
	{
		DebugLog = _debugLog;
		DebugRay = _debugRay;
	}
}

//_____________________________________________________________________________________________________________________________________________________________________________
[CustomEditor(typeof(Assistant))]
public class ServicePostEditor : Editor
{
	private Route[] _routes;
	private GameObject[][] _posts;
	private int _countOfroutes = 0;

	private string _materialAssetsFolder = "Assets/Prefabs/ServicePosts/Materials/";
	private string[] _materialGUIDS;
	private Material[] _materials;
	private int _countOfMaterials = 0;

	//_____________________________________________________________________________________________________________________________________________________________________________
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.Label("\nService Point Management");

		bool areArraysFull = _countOfroutes > 0 && _posts != null;

		if (areArraysFull)
		{
			PrintNames();

			if (GUILayout.Button("Rename and color points"))
			{
				RenamePosts();
				CheckMaterials();
				CreateMaterials();
				PullMaterials();
				ApplyMaterials();
			}

			if (GUILayout.Button("Generate new colors"))
			{
				CheckMaterials();
				if (_countOfMaterials >= _countOfroutes)
				{
					PullMaterials();
					UpdateColors();
				}
			}
		}
		else
		{
			FindRoutes();
			FindPosts();
		}
	}

	//_____________________________________________________________________________________________________________________________________________________________________________
	private void FindRoutes()
	{
		_routes = FindObjectsOfType<Route>();
		_countOfroutes = _routes.Length;
	}

	public void FindPosts()
	{
		_posts = new GameObject[_countOfroutes][];

		for (int i = 0; i < _countOfroutes; i++)
		{
			int numberOfPoints = _routes[i].transform.childCount;

			_posts[i] = new GameObject[numberOfPoints];

			for (int j = 0; j < numberOfPoints; j++)
			{
				_posts[i][j] = _routes[i].transform.GetChild(j).gameObject;
			}
		}
	}

	private void CreateMaterials()
	{
		int difference = _countOfroutes - _countOfMaterials;

		if (difference > 0)
		{
			for (int i = 0; i < difference; i++)
			{
				Material materialSample = new(Shader.Find("Unlit/Color"))
				{
					color = GenerateColor()
				};

				int numbering = _countOfMaterials == 0 ? i : i + _countOfMaterials;

				AssetDatabase.CreateAsset(materialSample, _materialAssetsFolder + "Material" + numbering + ".mat");
			}
		}
		CheckMaterials();
	}

	private void CheckMaterials()
	{
		if (!AssetDatabase.IsValidFolder(_materialAssetsFolder))
		{
			AssetDatabase.CreateFolder("Assets/Prefabs/ServicePosts", "Materials");
		}
		_materialGUIDS = AssetDatabase.FindAssets("Material", new[] { _materialAssetsFolder });
		_countOfMaterials = _materialGUIDS.Length;
	}

	private void PullMaterials()
	{
		_materials = new Material[_countOfroutes];

		for (int i = 0; i < _countOfroutes; i++)
		{
			string pathToAsset = AssetDatabase.GUIDToAssetPath(_materialGUIDS[i]);

			Material materialAsset = (Material)AssetDatabase.LoadAssetAtPath(pathToAsset, typeof(Material));
			_materials[i] = materialAsset;
		}
	}

	private void UpdateColors()
	{
		foreach (Material tMat in _materials)
		{
			tMat.color = GenerateColor();
		}
	}

	private void ApplyMaterials()
	{
		for (int i = 0; i < _countOfroutes; i++)
		{
			for (int j = 0; j < _posts[i].Length; j++)
			{
				_posts[i][j].GetComponent<MeshRenderer>().material = _materials[i];
			}
		}
	}

	private Color GenerateColor()
	{
		Color generatedColor = Random.ColorHSV(0, 1, 0.75f, 0.75f, 0.75f, 0.75f);
		return generatedColor;
	}

	private void RenamePosts()
	{
		for (int i = 0; i < _countOfroutes; i++)
		{
			int numberOfPoints = _posts[i].Length;
			string routeName = _routes[i].name;

			for (int j = 0; j < numberOfPoints; j++)
			{
				_posts[i][j].name = i + 1 + "." + routeName + "_" + (j + 1);
			}
		}
	}

	private void PrintNames()
	{
		GUILayout.Label("Routes:");

		for (int i = 0; i < _countOfroutes; i++)
		{
			int num = _posts[i].Length;
			string lastWord = num > 1 ? " points" : " point";
			GUILayout.Label(_routes[i].name + ": " + _posts[i].Length + lastWord);
		}
	}
}