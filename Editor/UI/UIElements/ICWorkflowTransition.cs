using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Innoactive.CreatorEditor.UI.Windows;

namespace Innoactive.CreatorEditor.UI.UIElements
{

    public class ICWorkflowTransition : ImmediateModeElement
    {
        Vector2 _positionA;
        Vector2 _positionB;

        static Material _lineMaterial;

        public Vector2 positionA
        {
            get { return _positionA; }
            set
            {
                _positionA = value;
                UpdateBounds();
            }
        }

        public Vector2 positionB
        {
            get { return _positionB; }
            set
            {
                _positionB = value;
                UpdateBounds();
            }
        }


        void UpdateBounds()
        {
            Vector2 min = Vector2.Min(_positionA, _positionB);
            Vector2 max = Vector2.Max(_positionA, _positionB);

            style.left = min.x;
            style.top = min.y;
            style.width = max.x - min.x;
            style.height = max.y - min.y;

            MarkDirtyRepaint();
        }


        protected override void ImmediateRepaint()
        {
            GetLineMaterial().SetPass(0);

            Vector2 min = Vector2.Min(_positionA, _positionB);
            Vector3 relA = _positionA - min;
            Vector3 relB = _positionB - min;

            GL.Begin(GL.LINES);
            GL.Color(Color.white);
            GL.Vertex3(relA.x, relA.y, 0);
            GL.Vertex3(relB.x, relB.y, 0);
            GL.End();

        }


        static Material GetLineMaterial()
        {
            if (!_lineMaterial)
            {
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                _lineMaterial = new Material(shader);
                _lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                _lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                _lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                _lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                _lineMaterial.SetInt("_ZWrite", 0);
            }

            return _lineMaterial;
        }

        public new class UxmlFactory : UxmlFactory<ICWorkflowTransition, UxmlTraits>
        {

        }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {


            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as ICWorkflowTransition;
                ate.Clear();



                ate.positionA = new Vector2(0, 0);
                ate.positionB = new Vector2(100, 100);
            }
        }
    }

}
