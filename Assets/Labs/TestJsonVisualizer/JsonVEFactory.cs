using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UIElements;
namespace Labs.TestJsonVisualizer
{
	public class JsonVEFactory
	{
		static JsonVEFactory()
		{
			styleSheet = Resources.Load<StyleSheet>("JsonVE");
		}

		static StyleSheet styleSheet;

		int max_obj_preview_elements_count = 5;
		int max_single_foldout_array_elements_count = 5;

		void SetObjFoldOutName(JObject obj, Foldout foldout)
		{
			var properties = obj.Properties().ToArray();
			var preview_element_names = new List<string>();
			for (int i = 0; i < max_obj_preview_elements_count && i < properties.Length; i++)
			{
				preview_element_names.Add(properties[i].Name);
			}
			if (max_obj_preview_elements_count < properties.Length)
			{
				preview_element_names.Add("...");
			}

			var preview_name = $"{{{string.Join(", ", preview_element_names)}}}";
			foldout.text = preview_name;
			foldout.RegisterValueChangedCallback(e =>
			{
				if (e.target != e.currentTarget) { return; }
				var fold_out = e.target as Foldout;
				bool is_open = e.newValue;
				if (is_open)
				{
					fold_out.text = "";
				}
				else
				{
					fold_out.text = preview_name;
				}
				e.StopPropagation();
			});
		}

		VisualElement JObjectToVE(JObject obj)
		{
			var obj_foldout = new Foldout
			{
				value = false
			};
			SetObjFoldOutName(obj, obj_foldout);
			foreach (JProperty property in obj.Properties())
			{
				var property_ve = JTokenToVE(property);
				obj_foldout.Add(property_ve);
			}
			return obj_foldout;
		}

		VisualElement JArrayToVE(JArray array)
		{
			var array_fold_out = new Foldout
			{
				value = false
			};
			foreach (JToken token in array.AsJEnumerable())
			{
				var token_ve = JTokenToVE(token);
				array_fold_out.Add(token_ve);
			}
			return array_fold_out;
		}

		VisualElement JPropertyToVE(JProperty property)
		{
			var property_ve = new VisualElement();
			property_ve.AddToClassList("JProperty");
			var name_label = new Label($"{property.Name}: ")
			{
				focusable = true,
				isSelectable = true
			};
			name_label.AddToClassList("JPropertyName");
			property_ve.Add(name_label);
			property_ve.Add(JTokenToVE(property.Value));
			return property_ve;
		}

		VisualElement JTokenToVE(JToken token)
		{
			VisualElement token_ve = token.Type switch
			{
				JTokenType.Object => JObjectToVE((JObject)token),
				JTokenType.Array => JArrayToVE((JArray)token),
				JTokenType.Property => JPropertyToVE((JProperty)token),
				JTokenType.Integer or
					JTokenType.Float or
					JTokenType.String or
					JTokenType.Boolean or
					JTokenType.Date or
					JTokenType.Raw or
					JTokenType.Bytes or
					JTokenType.Guid or
					JTokenType.Uri or
					JTokenType.TimeSpan
					=> new Label(token.Value<string>())
					{
						isSelectable = true,
					},
				JTokenType.Null => new Label("Null"),
				JTokenType.Undefined => new Label("Undefined"),
				JTokenType.None => new Label("None"),
				_ => throw new ArgumentOutOfRangeException()
			};

			token_ve.AddToClassList("JToken");
			token_ve.focusable = true;
			// token_ve.AddManipulator(new ContextualMenuManipulator(e =>
			// {
			// 	e.menu.AppendAction(token.Path, null);
			// }));

			return token_ve;
		}

		public VisualElement GenVE(JToken json)
		{
			var container = new VisualElement();
			var ve = JTokenToVE(json);
			container.Add(ve);
			container.styleSheets.Add(styleSheet);
			return container;
		}
	}
}