using UnityEngine.UIElements;

namespace MaxHelpers
{
    public abstract class UIBaseState
    {
        internal readonly UIDocument UIDoc;
        internal readonly VisualTreeAsset Asset;
        protected UIBaseState(UIDocument uiDoc, VisualTreeAsset asset)
        {
            UIDoc = uiDoc;
            Asset = asset;
        }
    }
}