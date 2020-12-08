using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SetParentScript : MonoBehaviour
{
    [SerializeField] Image panel = null;
    [SerializeField] GameObject[] images = new GameObject[8];

    public void SetPanel(int cardID)
    {
        var gameobject = Instantiate(images[cardID]);

        gameobject.transform.SetParent(panel.transform);

        gameobject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void DeletePanel()
    {
        // tag""である子オブジェクトのTransformのコレクションを取得
        var childTransforms = panel.transform.GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag("CardImage"));

        foreach (var item in childTransforms)
        {
            // tag""である子オブジェクトを削除する
            Destroy(item.gameObject);
        }
    }
}
