using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HappyUnity.UI
{
    public static class UIUtils
    {
        public static bool LayerInLayerMask(int layer, LayerMask layerMask)
        {
            return ((1 << layer) & layerMask) != 0;
        }
    }
}