using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SetParentScript : MonoBehaviour
{
    [SerializeField] private Image panel = null;
    [SerializeField] private GameObject[] images = new GameObject[(int)EnumNumbers.Cards];

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
