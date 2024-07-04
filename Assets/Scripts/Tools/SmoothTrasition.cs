using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SmoothTrasition : MonoBehaviour
{
    public int time;
    public float Start;
    public float End;
    private async void OnEnable()
    {
        LeanTween.moveLocalY(this.gameObject, End, time);

        await Task.Delay(time * 1000);
        LeanTween.moveLocalY(this.gameObject, Start, 0);
        this.gameObject.SetActive(false);
    }
}
