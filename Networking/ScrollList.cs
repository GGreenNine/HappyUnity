using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace HappyUnity
{
    /// <summary>
    /// Load data as a list
    /// </summary>
    public class ScrollList : MonoBehaviour
    {
        public List<dynamic> panelObjects = new List<dynamic>();

        public void CreateList<T>(string webRequest, Action<GameObject, T> itemOperation, GameObject buttonPrefab, RectTransform scrollPanel)
        {
            var json = webRequest;
            JsonConverter g = new VectorConverter(true,true,true);
            var results = JsonConvert.DeserializeObject<IEnumerable<T>>(json,g);

            foreach (var result in results)
            {
                var item = Instantiate(buttonPrefab, scrollPanel);
                itemOperation(item, result);
                panelObjects.Add(item);
            }
        }

        public void ClearList()
        {
            panelObjects.ForEach(x => Destroy(x));
        }
    }
}
