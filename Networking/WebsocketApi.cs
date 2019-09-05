using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HappyUnity.Singletons;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using WebSocketSharp;

public class WebsocketApi : PersistentSingleton<WebsocketApi>
{
    public delegate void ConnectionStateHangler();
    public event ConnectionStateHangler onConnected;

    
    /// <summary>
    /// Connect to concreate websocket
    /// </summary>
    /// <param name="roomName"></param>
    /// <param name="token"></param>
    public void WebsocketConnection(string Adress, string token, out WebSocket websocket)
    {
        websocket = new WebSocket($"{Adress}/?{token}");
        websocket.Compression = CompressionMethod.Deflate;

        Debug.Log("Пытаюсь установить соединение с сервером");

        websocket.OnOpen += delegate(object sender, EventArgs args)
        {
            onConnected?.Invoke();
        };

        websocket.OnMessage += ServerMessageHandler;

        websocket.OnError += WebSocket_OnError;

        websocket.Connect();
    }

    private void ServerMessageHandler(object sender, MessageEventArgs e)
    {
        Debug.Log($"{e.Data} - data, \t {sizeof(char)* e.Data.Length} - message weigth");
    }
    private void WebSocket_OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError($"server says: {e.Message} -message {e.Exception.Message} - exception {e.Exception.InnerException} - inner exception {e.Exception.Data} - exception data");
    }
    
    /// <summary>
    /// Desirializing object to dictionary
    /// </summary>
    /// <param name="listTo"></param>
    /// <param name="listFrom"></param>
    /// <typeparam name="T"></typeparam>
    private static void DesirializeUndefinedObjectToDictionary<T>(ref Dictionary<string, T> listTo, JToken listFrom)
    {
        if (listFrom == null || !listFrom.Any()) return;

        foreach (var v in listFrom)
        foreach (var s in v)
        foreach (JProperty x in s)
        {
            var obj = JsonConvert.DeserializeObject<T>(x.First.ToString());
            listTo.Add(x.Name, obj);
        }
    }

    private static void DesirializeDefinedObjectToDictionary<T>(ref Dictionary<string, T> listTo, JToken listFrom)
        where T : class
    {
        if (listFrom == null || !listFrom.Any()) return;

        foreach (var v in listFrom)
        foreach (var s in v)
        foreach (JProperty x in s)
        {
            var obj = JsonConvert.DeserializeObject<T>(x.First.ToString());
            listTo.Add(x.Name, obj);
        }
    }
}
