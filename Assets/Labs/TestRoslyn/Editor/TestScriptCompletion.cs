using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using UnityEditor;
using UnityEngine;
namespace Labs.TestRoslyn.Editor
{
	public class TestScriptCompletion : EditorWindow
	{
		[MenuItem("Labs/Labs.TestRoslyn.Editor/TestScriptCompletion")]
		static void ShowWindow()
		{
			var window = GetWindow<TestScriptCompletion>();
			window.titleContent = new("TestScriptCompletion");
			window.Show();
		}

		async void OnEnable()
		{
			var host = MefHostServices.Create(MefHostServices.DefaultAssemblies);
			var workspace = new AdhocWorkspace(host);
			var scriptCode = "using System; Guid.N";
			var scriptCode0 = "using System; (int font_size, float light) Monoid=(1,1f);";
			var scriptCode1 = "Monoid.";
			var compilationOptions = new CSharpCompilationOptions(
				OutputKind.DynamicallyLinkedLibrary);
			var metadataReferences = new[]
			{
				MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
			};
			var csharpParseOptions = new CSharpParseOptions(kind: SourceCodeKind.Script);

			ProjectInfo scriptProjectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "project_0", "project_0", LanguageNames.CSharp, isSubmission: true).
				WithMetadataReferences(metadataReferences).
				WithCompilationOptions(compilationOptions).
				WithParseOptions(csharpParseOptions);

			var scriptProject = workspace.AddProject(scriptProjectInfo);
			var scriptDocument = scriptProject.AddDocument("script_0", scriptCode0);

			scriptProject = workspace.AddProject(ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "project_1", "project_1", LanguageNames.CSharp, isSubmission: true).
				WithMetadataReferences(metadataReferences).
				WithCompilationOptions(compilationOptions).
				WithParseOptions(csharpParseOptions).WithProjectReferences(new[]
				{
					new ProjectReference(scriptProject.Id)
				}));
			
			new InteractiveScriptGlobals();

			
			scriptDocument = scriptProject.AddDocument("script_1", scriptCode1);

			var caret_position = scriptCode1.Length - 1;
			var completionService = CompletionService.GetService(scriptDocument);
			var results = await completionService.GetCompletionsAsync(scriptDocument, caret_position);

			results.Items.ToList().ForEach(item =>
			{
				var display_text = item.DisplayText;
				var properties = string.Join(" ", item.Properties.Select(p => p.Key + ": " + p.Value));
				var tags = string.Join(" ", item.Tags.Select(t => t.ToString()));

				Debug.Log($"{display_text}\n{properties}\n{tags}");
			});
		}

	}
}