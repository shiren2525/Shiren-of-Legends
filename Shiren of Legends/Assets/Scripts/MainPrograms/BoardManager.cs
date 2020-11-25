using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] Transform[] transforms = new Transform[7];
    [SerializeField] Transform[] transforms1 = new Transform[7];
    [SerializeField] Transform[] transforms2 = new Transform[7];
    [SerializeField] Transform[] transforms3 = new Transform[7];

    public Transform[,] TransformList { get; } = new Transform[4, 7];

    private void Start()
    {
        SetupBlock();
    }

    private void SetupBlock()
    {
        for (int i = 0; i < TransformList.GetLength(1); i++)
        {
            TransformList[0, i] = transforms[i];
            TransformList[1, i] = transforms1[i];
            TransformList[2, i] = transforms2[i];
            TransformList[3, i] = transforms3[i];
        }
    }
}