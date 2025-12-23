using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace Dialect.Editor
{
    [Serializable]
    [Graph(AssetExtension)]
    internal class DialectDirectorGraph : Graph
    {
        internal const string AssetExtension = "dialect";
        
        [MenuItem("Assets/Create/Dialect Director Graph")]
        static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialectDirectorGraph>("Dialect Graph");
        }
        
        public override void OnGraphChanged(GraphLogger infos)
        {
            base.OnGraphChanged(infos);
            //Todo add error checking logic here
        }
    }
}