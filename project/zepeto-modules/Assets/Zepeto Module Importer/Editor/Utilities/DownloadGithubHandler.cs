using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadGithubHandler
{
    public static IEnumerator GetDataAsync(System.Action<string> onDataLoaded)
    {
        UnityWebRequest www = UnityWebRequest.Get(ConstantManager.CONTENT_DATA_PATH);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            onDataLoaded(www.downloadHandler.text);
        }
    }
    
    public static IEnumerator GetTextureAsync(string url, System.Action<Texture2D> onTextureLoaded)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            float ratio = (float)texture.height / (float)texture.width;
            int fixedWidth = 550;
            int height = Mathf.RoundToInt(ratio * fixedWidth);
            texture = ScaleTexture(texture, fixedWidth, height);
            onTextureLoaded(texture);
        }
        else
        {
            Debug.Log("Failed to download image. Error: " + www.error);
        }
    }
    
    public static Texture2D ScaleTexture(Texture2D texture, int targetWidth, int targetHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

        Graphics.Blit(texture, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D scaledTexture = new Texture2D(targetWidth, targetHeight);
        scaledTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        scaledTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return scaledTexture;
    }

}