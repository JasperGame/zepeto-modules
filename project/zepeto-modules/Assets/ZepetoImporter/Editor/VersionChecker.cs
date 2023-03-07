using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class VersionChecker : MonoBehaviour
{
    private const string Url = "https://github.com/JasperGame/zepeto-modules/tree/main/release/GestureAPI";

    private IEnumerator Start()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(Url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                yield break;
            }

            string html = request.downloadHandler.text;
            StartCoroutine(ParseHtml(html));
        }
    }

    // private IEnumerator ParseHtml(string html)
    // {
    //     HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
    //     doc.LoadHtml(html);
    //
    //     var fileList = doc.DocumentNode.Descendants("div")
    //         .Where(x => x.GetAttributeValue("class", "") == "js-details-container Details content")
    //         .FirstOrDefault()
    //         .Descendants("a")
    //         .Where(x => x.GetAttributeValue("class", "") == "js-navigation-open Link--primary")
    //         .Select(x => x.InnerHtml)
    //         .ToList();
    //
    //     foreach (var file in fileList)
    //     {
    //         Debug.Log(file);
    //     }
    // }
}