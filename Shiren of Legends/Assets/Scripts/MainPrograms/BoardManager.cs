using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] Transform[] transforms = new Transform[(int)EnumBoardLength.MaxBoardLengthX];
    [SerializeField] Transform[] transforms1 = new Transform[(int)EnumBoardLength.MaxBoardLengthX];
    [SerializeField] Transform[] transforms2 = new Transform[(int)EnumBoardLength.MaxBoardLengthX];
    [SerializeField] Transform[] transforms3 = new Transform[(int)EnumBoardLength.MaxBoardLengthX];

    public Transform[,] TransformList { get; } = new Transform[(int)EnumBoardLength.MaxBoardLengthX, (int)EnumBoardLength.MaxBoardLengthY];

    private void Start()
    {
        SetupBlock();
    }

    private void SetupBlock()
    {
        for (int i = 0; i < TransformList.GetLength(0); i++)
        {
            TransformList[i, 0] = transforms[i];
            TransformList[i, 1] = transforms1[i];
            TransformList[i, 2] = transforms2[i];
            TransformList[i, 3] = transforms3[i];
        }
    }
}