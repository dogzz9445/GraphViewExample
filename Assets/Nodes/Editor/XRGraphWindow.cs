using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using GraphProcessor;

namespace XRNodes.Editor
{
    public class XRGraphWindow : BaseGraphWindow
    {
        BaseGraph graph;

        [MenuItem("XR/GraphView")]
        public static XRGraphWindow OpenWithGraph()
        {
            var graphWindow = CreateWindow<XRGraphWindow>();

            // When the graph is opened from the window, we don't save the graph to disk
            graphWindow.graph = ScriptableObject.CreateInstance<BaseGraph>();
            graphWindow.graph.hideFlags = HideFlags.HideAndDontSave;
            graphWindow.InitializeGraph(graphWindow.graph);

            graphWindow.Show();

            return graphWindow;
        }

        protected override void OnDestroy()
        {
            graphView?.Dispose();
            DestroyImmediate(graph);
        }

        protected override void InitializeWindow(BaseGraph graph)
        {
            titleContent = new GUIContent("XR Graph");

            if (graphView == null)
            {
                graphView = new XRGraphView(this);
                //graphView.Add(new CustomToolbarView(graphView));
            }

            rootView.Add(graphView);
        }
    }
}
