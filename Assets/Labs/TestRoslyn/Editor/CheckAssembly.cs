using System;
using System.Linq;
using Labs.TestJsonVisualizer;
using UnityEditor;
using UnityEngine.UIElements;
namespace Labs.TestRoslyn.Editor
{
	public class CheckAssembly : EditorWindow
	{
		[MenuItem("Labs/Labs.TestRoslyn.Editor/CheckAssembly")]
		static void ShowWindow()
		{
			var window = GetWindow<CheckAssembly>();
			window.titleContent = new("CheckAssembly");
			window.Show();
		}

		string[] app_domain_assemblies;
		string some_class_assembly;


		void OnEnable()
		{
			some_class_assembly = typeof(JsonVEFactory).Assembly.ToString();
			app_domain_assemblies = AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.GetName().Name + "\n").ToArray();
		}

		void CreateGUI()
		{
			var scroll_view = new ScrollView(ScrollViewMode.VerticalAndHorizontal)
				{ style = { flexGrow = 1 } };
			scroll_view.Add(new Label(some_class_assembly));
			var app_domain_assemblies_list = new ListView(app_domain_assemblies, 30, () => new Label(), (ve, i) =>
			{
				var label = ve as Label;
				label.text = app_domain_assemblies[i];
			});
			scroll_view.Add(app_domain_assemblies_list);
			rootVisualElement.Add(scroll_view);
		}
	}
}