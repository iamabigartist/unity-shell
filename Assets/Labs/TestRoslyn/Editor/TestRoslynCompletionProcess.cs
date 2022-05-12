using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Recommendations;
using Microsoft.CodeAnalysis.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
namespace Labs.TestRoslyn.Editor
{
	public class TestRoslynCompletionProcess : EditorWindow
	{
		[MenuItem("Labs/Labs.TestRoslyn.Editor/TestRoslynCompletionProcess")]
		static void ShowWindow()
		{
			var window = GetWindow<TestRoslynCompletionProcess>();
			window.titleContent = new("TestRoslynCompletionProcess");
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
			// var host = MefHostServices.Create(AppDomain.CurrentDomain.GetAssemblies());
			var workspace = new AdhocWorkspace(host);
			var code = @"
using System;
using System.Collections.Generic;
using System.Text;
Guid.
";
			//code = "var a = Guid.Empty;";
			var syntaxTree = CSharpSyntaxTree.ParseText(code);
			// Debug.Log(syntaxTree.GetRoot().Kind());
			// Debug.Log(syntaxTree.GetRoot().ChildNodesAndTokens()[0].Kind());
			var compilation = CSharpCompilation.Create("foo")
				.AddReferences(MetadataReference.CreateFromFile(typeof(DateTime).Assembly.Location))
				.AddSyntaxTrees(syntaxTree);




			AssemblyDefinitionAsset a = Resources.Load<AssemblyDefinitionAsset>("");


			var semanticModel = compilation.GetSemanticModel(syntaxTree);

			// var r = syntaxTree.GetCompilationUnitRoot();
			// // Use the syntax tree to find "using System;"
			// UsingDirectiveSyntax usingSystem = r.Usings[0];
			// NameSyntax systemName = usingSystem.Name;

			// Use the semantic model for symbol information:
			// SymbolInfo nameInfo = semanticModel.GetSymbolInfo(systemName);
			// var systemSymbol = (INamespaceSymbol)nameInfo.Symbol;
			// foreach (INamespaceSymbol ns in systemSymbol.GetNamespaceMembers())
			// {
			// 	Debug.Log(ns);
			// }


			var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "MyProject", "MyProject", LanguageNames.CSharp).WithMetadataReferences(new[]
			{
				MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
			});
			var project = workspace.AddProject(projectInfo);
			var document = workspace.AddDocument(DocumentInfo.Create(DocumentId.CreateNewId(project.Id), "Script",
				sourceCodeKind: SourceCodeKind.Script,
				loader: TextLoader.From(TextAndVersion.Create(SourceText.From(code), VersionStamp.Create()))));
			// position is the last occurrence of "Guid." in our test code
			// in real life scenarios the editor surface should inform us
			// about the current cursor position
			var cursor_position = code.LastIndexOf("Guid.") + 5;
			var completionService = Recommender.GetRecommendedSymbolsAtPositionAsync(semanticModel, cursor_position, workspace);
			// var helloWorldString = r.DescendantNodes().OfType<LiteralExpressionSyntax>().Single();
			// Use the semantic model for type information:
			// TypeInfo literalInfo = semanticModel.GetTypeInfo(helloWorldString);
			
			// var stringTypeSymbol = (INamedTypeSymbol)literalInfo.Type;
			// var allMembers = stringTypeSymbol.GetMembers();
			// var methods = allMembers.OfType<IMethodSymbol>();
			// var public_return_string_methods = methods.Where(m => m.DeclaredAccessibility == Accessibility.Public && m.ReturnType.Equals(stringTypeSymbol));
			// var distinct_public_return_string_methods_names = public_return_string_methods.Select(m => m.Name).Distinct();
			// foreach (string methods_name in distinct_public_return_string_methods_names)
			// {
			// 	Debug.Log(methods_name);
			//
			// }

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