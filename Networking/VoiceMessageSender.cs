using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using HappyUnity.Singletons;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;

public class MessageManager : Singleton<MessageManager>
{
    public static int frequency = 44100;
    private AudioSource source;
    private int lastSample;
    private AudioClip c;
    private List<float> samplesBuffer = new List<float>();

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.V) && !Microphone.IsRecording(null))
        {
            
            c = Microphone.Start(null, false, 60, frequency);
        }
        else if (!Input.GetKey(KeyCode.V) && Microphone.IsRecording(null))
        {
            samplesBuffer.Clear();
            Microphone.End(null);
        }
    }

//    private void FixedUpdate()
//    {
//        if (Microphone.IsRecording(null))
//        {
//            int pos = Microphone.GetPosition(null);
//            int diff = pos - lastSample;
//            
//            if (diff > 0)
//            {
//                float[] samples = new float[diff * c.channels];
//                c.GetData(samples, lastSample);
//                for (int i = 0; i < samples.Length; i++)
//                    samplesBuffer.Add(samples[i]);
//            }
//
//            if (samplesBuffer.Count > 10000)
//            {
//                byte[] ba = ToByteArray(samplesBuffer.ToArray());
//                samplesBuffer.Clear();
//                Debug.Log("Send Before Compress " + sizeof(byte) * ba.Length);
//                byte[] cBa = CLZF2.Compress(ba);
//                Debug.Log("Send After Compress " + sizeof(byte) * cBa.Length);
//                SendVoiceToServer(cBa, c.channels);
//            }
//            lastSample = pos;
//        }
//    }

//    private void SendVoiceToServer(byte[] message, int channels)
//    {
//        ServerMessageModel serverMessageModel = new ServerMessageModel { type = Enums.MessageTypes.chat };
//
//        JObject state = new JObject();
//
//        string playerName = UserManagment.Instance.CurrentUser.username;
//
//        VoiceMessage voiceMessage = new VoiceMessage(playerName, message, channels);
//
//        state.Add(playerName, JsonConvert.SerializeObject(voiceMessage));
//
//        serverMessageModel.data.bodyType = Enums.BodyMessageTypes.voice;
//        serverMessageModel.data.bodyData.Add("objects", state);
//
//        var serverAnswerJson = JsonConvert.SerializeObject(serverMessageModel);
//        WebsocketApi.Instance._queueToSend.Add(new PackageModel(WebsocketApi.chatWebsocket, serverAnswerJson));
//    }

    public void ReciveData(byte[] message, int chan, string userName)
    {
        Debug.Log("Get Compressed " + sizeof(byte) * message.Length);
        byte[] decBa = CLZF2.Decompress(message);
        Debug.Log("Get Uncompressed " + sizeof(byte) * decBa.Length);
        float[] f = ToFloatArray(decBa);
        
        UnityMainThreadDispatcher.Instance().Enqueue(delegate
        {
            source.clip = AudioClip.Create("test", f.Length, chan, frequency, false);
            source.clip.SetData(f, 0);
            if (!source.isPlaying) source.Play();
        });
    }

    public static byte[] ToByteArray(float[] floatArray)
    {
        int len = floatArray.Length * 4;
        byte[] byteArray = new byte[len];
        int pos = 0;
        foreach (float f in floatArray)
        {
            byte[] data = System.BitConverter.GetBytes(f);
            System.Array.Copy(data, 0, byteArray, pos, 4);
            pos += 4;
        }
        return byteArray;
    }

    public static float[] ToFloatArray(byte[] byteArray)
    {
        int len = byteArray.Length / 4;
        float[] floatArray = new float[len];
        for (int i = 0; i < byteArray.Length; i += 4)
        {
            floatArray[i / 4] = System.BitConverter.ToSingle(byteArray, i);
        }
        return floatArray;
    }
}
