using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System;
using HappyUnity.Singletons;

public class VideoStreamReciever : Singleton<VideoStreamReciever>
{
    public RawImage image;
    Texture2D tex;

    private bool stop = false;
    const int SEND_RECEIVE_COUNT = 15;
    

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;

        tex = new Texture2D(0, 0);
    }
    
    //Converts the data size to byte array and put result to the fullBytes array
    void byteLengthToFrameByteArray(int byteLength, byte[] fullBytes)
    {
        //Clear old data
        Array.Clear(fullBytes, 0, fullBytes.Length);
        //Convert int to bytes
        byte[] bytesToSendCount = BitConverter.GetBytes(byteLength);
        //Copy result to fullBytes
        bytesToSendCount.CopyTo(fullBytes, 0);
    }

    //Converts the byte array to the data size and returns the result
    int frameByteArrayToByteLength(byte[] frameBytesLength)
    {
        int byteLength = BitConverter.ToInt32(frameBytesLength, 0);
        return byteLength;
    }


    ///////////////////////////////////////////////////Read Image SIZE from Server///////////////////////////////////////////////////
//    private int readImageByteSize(int size)
//    {
//        bool disconnected = false;
//
//        NetworkStream serverStream = client.GetStream();
//        byte[] imageBytesCount = new byte[size];
//        var total = 0;
//        do
//        {
//            var read = serverStream.Read(imageBytesCount, total, size - total);
//            //Debug.LogFormat("Client recieved {0} bytes", total);
//            if (read == 0)
//            {
//                disconnected = true;
//                break;
//            }
//            total += read;
//        } while (total != size);
//
//        int byteLength;
//
//        if (disconnected)
//        {
//            byteLength = -1;
//        }
//        else
//        {
//            byteLength = frameByteArrayToByteLength(imageBytesCount);
//        }
//        return byteLength;
//    }

    /////////////////////////////////////////////////////Read Image Data Byte Array from Server///////////////////////////////////////////////////
//    private void readFrameByteArray(int size)
//    {
//        bool disconnected = false;
//        
//        NetworkStream serverStream = s.GetStream();
//        byte[] imageBytes = new byte[size];
//        var total = 0;
//        do
//        {
//            var read = serverStream.Read(imageBytes, total, size - total);
//            //Debug.LogFormat("Client recieved {0} bytes", total);
//            if (read == 0)
//            {
//                disconnected = true;
//                break;
//            }
//            total += read;
//        } while (total != size);
//
//        bool readyToReadAgain = false;
//
//        //Display Image
//        if (!disconnected)
//        {
//            //Display Image on the main Thread dispatcher
////            Loom.QueueOnMainThread(() =>
////            {
////                DisplayReceivedImage(imageBytes);
////                readyToReadAgain = true;
////            });
//        }
//
//        //Wait until old Image is displayed
//        while (!readyToReadAgain)
//        {
//            System.Threading.Thread.Sleep(1);
//        }
//    }


    public void DisplayReceivedImage(byte[] receivedImageBytes)
    {
        tex.LoadImage(receivedImageBytes);
        image.texture = tex;
    }
    
}