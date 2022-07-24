using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using GraphProcessor;
using UnityEngine.UIElements;

namespace XRNodes
{
    public class XRGraphWindow : BaseGraphWindow
    {
        BaseGraph xrGraph;

        [MenuItem("XR/GraphView")]
        public static XRGraphWindow OpenWithGraph()
        {
            var graphWindow = CreateWindow<XRGraphWindow>();

            // When the graph is opened from the window, we don't save the graph to disk
            graphWindow.xrGraph = ScriptableObject.CreateInstance<BaseGraph>();
            graphWindow.xrGraph.hideFlags = HideFlags.HideAndDontSave;
            graphWindow.InitializeGraph(graphWindow.xrGraph);

            graphWindow.Show();

            return graphWindow;
        }

        protected override void OnDestroy()
        {
            graphView?.Dispose();
            DestroyImmediate(xrGraph);
        }

        private ListView GenerateListViewItems()
        {
            const int itemCount = 1000;
            var items = new List<string>(itemCount);
            for (int i = 1; i <= itemCount; i++)
                items.Add(i.ToString());

            // The "makeItem" function is called when the
            // ListView needs more items to render.
            Func<VisualElement> makeItem = () => new Label();

            // As the user scrolls through the list, the ListView object
            // recycles elements created by the "makeItem" function,
            // and invoke the "bindItem" callback to associate
            // the element with the matching data item (specified as an index in the list).
            Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = items[i];

            // Provide the list view with an explict height for every row
            // so it can calculate how many items to actually display
            const int itemHeight = 16;

            var listView = new ListView(items, itemHeight, makeItem, bindItem);

            listView.selectionType = SelectionType.Multiple;

            listView.onItemsChosen += objects => Debug.Log(objects);
            listView.onSelectionChange += objects => Debug.Log(objects);

            listView.style.flexGrow = 1.0f;
            return listView;
        }

        protected override void InitializeWindow(BaseGraph graph)
        {
            titleContent = new GUIContent("XR Graph");

            if (graphView == null)
            {
                graphView = new XRGraphView(this);
                //graphView.Add(new CustomToolbarView(graphView));
            }

            rootVisualElement.style.flexDirection = FlexDirection.Row;

            rootView.Add(graphView);
            rootView.style.position = Position.Relative;
            rootView.style.left = 16;

            var listView = GenerateListViewItems();
            listView.style.position = Position.Relative;
            listView.style.left = 16;

            //rootVisualElement.Add(listView);
            graphView.Add(listView);
        }
    }
}
