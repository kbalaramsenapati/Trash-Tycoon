using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class DumpYard : MonoBehaviour
{
    public GameObject TrafficPoint;
    public Animator CraneAnimator;

    public GameObject DumpYardMesh;
    public GameObject DumpYardLockGround;
    public GameObject TextPopUp;

    public async void VehcileReached(GameObject TempGameObject)
    {
        Vehcile vehcile = TempGameObject.GetComponent<Vehcile>();
        TrafficPoint.SetActive(true);
        while (vehcile.NotReachedTarget)
        {
            await Task.Delay(100); // Check every 100 milliseconds
        }
        CraneAnimator.Play("LoadItem");
        await Task.Delay((int)5 * 1000);
        vehcile.VehcileSetDestination(GarbageStatus.TrashTrooper);
        TrafficPoint.SetActive(false);

        Gamemanager.Instance.OnMoneyCollect(500);
        TextPopUp.SetActive(true);

    }

    public void Onclick_DumpYadLockUnLock(bool TempCondition)
    {
        if (TempCondition)
        {
            TrafficPoint.SetActive(true);
            DumpYardLockGround.SetActive(true);
            DumpYardMesh.SetActive(false);
            
        }
        else
        {
            TrafficPoint.SetActive(false);
            DumpYardLockGround.SetActive(false);
            DumpYardMesh.SetActive(true);
        }
    }

}
