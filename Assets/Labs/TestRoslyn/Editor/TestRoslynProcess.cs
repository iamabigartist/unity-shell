using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Recommendations;
using Microsoft.CodeAnalysis.Text;
using UnityEditor;
using UnityEngine;
namespace Labs.TestRoslyn.Editor
{
	public class TestRoslynProcess : EditorWindow
	{
		[MenuItem("Labs/Labs.TestRoslyn.Editor/TestRoslynProcess")]
		static void ShowWindow()
		{
			var window = GetWindow<TestRoslynProcess>();
			window.titleContent = new("TestRoslynProcess");
			window.Show();
		}

		void OnEnable()
		{
			// var code0 = @"
			//  using System;
			//  public class Test
			//  {
			//      public void TestMethod()
			//      {
			//          var now = DateTime.Now;
			//          now.
			//      }
			//  }";
			// Debug.Log(code0);
			//
			// var syntaxTree = CSharpSyntaxTree.ParseText(code0);
			// var compilation = CSharpCompilation.Create("foo")
			// 	.AddReferences(MetadataReference.CreateFromFile(typeof(DateTime).Assembly.Location))
			// 	.AddSyntaxTrees(syntaxTree);
			// var semanticModel = compilation.GetSemanticModel(syntaxTree);
			//
			// var dotTextSpan = new TextSpan(code0.IndexOf("now.") + 3, 1);
			// var memberAccessNode = (MemberAccessExpressionSyntax)syntaxTree.GetRoot().DescendantNodes(dotTextSpan).Last();
			//
			// var lhsType = semanticModel.GetTypeInfo(memberAccessNode.Expression).Type;
			//
			// foreach (var symbol in lhsType.GetMembers())
			// {
			// 	if (!symbol.CanBeReferencedByName
			// 		|| symbol.DeclaredAccessibility != Accessibility.Public
			// 		|| symbol.IsStatic)
			// 		continue;
			//
			// 	Debug.Log(symbol.Name);
			// }

			Process1();


		}

		public async void Process1()
		{
			var host = MefHostServices.Create(MefHostServices.DefaultAssemblies);
			var workspace = new AdhocWorkspace(host);
			var code = @"using System;
 
public class MyClass
{
    public static void MyMethod(int value)
    {
        Guid.
    }
}";
			var syntaxTree = CSharpSyntaxTree.ParseText(code);
			var compilation = CSharpCompilation.Create("foo")
				.AddReferences(MetadataReference.CreateFromFile(typeof(DateTime).Assembly.Location))
				.AddSyntaxTrees(syntaxTree);
			var semanticModel = compilation.GetSemanticModel(syntaxTree);

			var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "MyProject", "MyProject", LanguageNames.CSharp).WithMetadataReferences(new[]
			{
				MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
			});
			var project = workspace.AddProject(projectInfo);
			var document = workspace.AddDocument(project.Id, "MyFile.cs", SourceText.From(code));
			// position is the last occurrence of "Guid." in our test code
			// in real life scenarios the editor surface should inform us
			// about the current cursor position
			var cursor_position = code.LastIndexOf("Guid.") + 5;

			var completionService = Recommender.GetRecommendedSymbolsAtPositionAsync(semanticModel, cursor_position, workspace);
			var results = await completionService;
			foreach (ISymbol result in results)
			{
				Debug.Log(result.Name);
			}
		}

		void OnGUI()
		{

		}
	}
}