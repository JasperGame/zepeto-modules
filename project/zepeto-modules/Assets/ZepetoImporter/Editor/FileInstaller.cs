using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileInstaller : MonoBehaviour
{
    public string fileUrl = "https://www.example.com/example.txt"; // 다운로드할 파일의 URL
    public string fileName = "example.txt"; // 저장될 파일 이름

    public void DownloadFile()
    {
        StartCoroutine(DownloadFileCoroutine());
    }

    IEnumerator DownloadFileCoroutine()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(fileUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Failed to download file: " + www.error);
            }
            else
            {
                string filePath = Path.Combine(Application.persistentDataPath, fileName); // 저장될 파일 경로

                File.WriteAllBytes(filePath, www.downloadHandler.data);

                Debug.Log("File downloaded to: " + filePath);
            }
        }
    }
}
