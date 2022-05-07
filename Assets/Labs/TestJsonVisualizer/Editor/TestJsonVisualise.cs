using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace Labs.TestJsonVisualizer.Editor
{
	public class TestJsonVisualise : EditorWindow
	{
		[MenuItem("Labs/Labs.TestJsonVisualizer.Editor/TestJsonVisualise")]
		static void ShowWindow()
		{
			var window = GetWindow<TestJsonVisualise>();
			window.titleContent = new("TestJsonVisualise");
			window.Show();
		}

		object m_object;

		void OnEnable()
		{
			m_object = new
			{
				text = "text",
				number = 1,
				hidden = true,
				array = new[] { 1, 2, 3 },
				objects = new
				{
					a1 = new { a = 1, b = 2 },
					a2 = new { a = 3, b = 4 }
				}
			};

			var arraya = new int[10000000];

			var json_obj = JObject.FromObject(m_object);
			Debug.Log(JsonConvert.SerializeObject(new Button(), Formatting.Indented, new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			}));
			Debug.Log(JObject.FromObject(new
			{
				date = new DateTime(2000, 1, 1),
				a = 2
			}).Properties().First().Value.Value<string>());
		}

		void CreateGUI()
		{
			var json_ve_factory = new JsonVEFactory();
			// var m_object_json_ve = json_ve_factory.GenVE(JToken.Parse(File.ReadAllText("Assets/Labs/TestJsonVisualizer/TestJson.json")));
			var m_object_json_ve = json_ve_factory.GenVE(JToken.FromObject(this, new()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			}));

			var scroll_view = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
			scroll_view.Add(m_object_json_ve);
			rootVisualElement.Add(scroll_view);
		}
	}
}