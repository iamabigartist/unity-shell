using System.Collections.Generic;
using UnityEngine.Scripting;
using UnityEngine.UIElements;
namespace Labs.Test1
{
    public class GameProgressBar : TemplateContainer
    {
        [Preserve]
        public new class UxmlFactory : UxmlFactory<GameProgressBar, UxmlTraits> { }

        [Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription label = new UxmlStringAttributeDescription() { name = "AAS_d_A_labelaas1asAss_as_atr", defaultValue = "Progress" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init( ve, bag, cc );
                GameProgressBar ate = ve as GameProgressBar;

                ate.Label_atr = label.GetValueFromBag( bag, cc );
            }
        }

        Label label;

        public string Label_atr
        {
            get => label.text;
            set => label.text = value;
        }

        public GameProgressBar()
        {
            label = new Label() { name = "progress-label" };
            hierarchy.Add( label );
        }

    }
}
