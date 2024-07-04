using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class House : MonoBehaviour
{
    public GameObject Garbages;
    public int CountProgress = 3;
    public bool IsHouseBuild;
    public Slider slider;
    public TMP_Text Text_CountProgress;
    public GameObject UI;
    public GameObject TextPopUp;
    private void Start()
    {
        Text_CountProgress.text = CountProgress.ToString();
        slider.value = CountProgress;
    }
    public async void VehcileReached(GameObject TempGameObject)
    {
        Vehcile vehcile = TempGameObject.GetComponent<Vehcile>();
        while (vehcile.NotReachedTarget)
        {
            await Task.Delay(100); // Check every 100 milliseconds
        }
        Debug.Log("Checked");
        await Task.Delay((int)10 * 1000);
        vehcile.VehcileSetDestination(GarbageStatus.DumpYard);

        Gamemanager.Instance.OnMoneyCollect(200);
        Gamemanager.Instance.OnTrashCollect(1);
        TextPopUp.SetActive(true);

        if (CountProgress > 0)
        {
            CountProgress--;
            Text_CountProgress.text = CountProgress.ToString();
            slider.value = CountProgress;
        }
        if(CountProgress<=0 && !IsHouseBuild)
        {
            Garbages.SetActive(false);
            IsHouseBuild = true;
            UI.SetActive(false);
            if (Gamemanager.Instance.int_TutorialCount == 7)
            {
                Gamemanager.Instance.StepNumberUpdate(8);
                Gamemanager.Instance.TutorialEventCall();
            }
        }
    }

    public void Onclick_WhenStart()
    {
        CountProgress = 0;
        IsHouseBuild = true;
        Garbages.SetActive(false);
        IsHouseBuild = true;
        UI.SetActive(false);
    }

    #region Dustbin
    //public GameObject TrafficPoint;
    //private void OnTriggerEnter(Collider other)
    //{
    //    //if(other.TryGetComponent<Vehcile>(out Vehcile veh))
    //    switch (other.tag)
    //    {
    //        case "NPCVehcile":
    //            //TrafficPoint.SetActive(true);
    //            //TriggerEnterVehcile(other.gameObject);
    //            break;
    //    }

    //}
    //void TriggerEnterVehcile(GameObject TempGameObject)
    //{
    //    #region Dustbin
    //    ////await Task.Delay((int)5 * 1000);
    //    ////TempGameObject.GetComponent<NavMeshAgent>().enabled = false;
    //    //////await Task.Delay((int)2 * 1000);
    //    ////TempGameObject.GetComponent<NavMeshAgent>().enabled = true;
    //    ////await Task.Delay((int)5 * 1000);
    //    ////TempGameObject.GetComponent<Vehcile>().PauseAgent();
    //    //await Task.Delay((int)10 * 1000);
    //    //TempGameObject.GetComponent<Vehcile>().ResumeAgent();
    //    //TempGameObject.GetComponent<Vehcile>().VehcileSetDestination(GarbageStatus.DumpYard);
    //    ////TrafficPoint.SetActive(false);
    //    ///
    //    #endregion

    //    TempGameObject.GetComponent<Vehcile>().garbageStatus = GarbageStatus.DumpYard;
    //}
    //public bool a;

    //public IEnumerator waitVehcileReached(GameObject TempGameObject)
    //{
    //    Vehcile vehcile = TempGameObject.GetComponent<Vehcile>();
    //    Debug.Log("entered");
    //    //vehcile.istruehere = false;
    //    while (vehcile.NotReachedTarget)
    //    {
    //        Debug.Log("Waiting");
    //        yield return null;
    //    }
    //    Debug.Log("Checked");
    //    yield return new WaitForSeconds(10);
    //    vehcile.VehcileSetDestination(GarbageStatus.DumpYard);
    //}
    //public bool a = false;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    // Start the async method
    //    CheckConditionAsync();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // For testing purposes, let's set a to true when pressing the space bar
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        a = true;
    //    }
    //}

    //// Async method to check the condition
    //private async void CheckConditionAsync()
    //{
    //    // Await until a is true
    //    while (!a)
    //    {
    //        await Task.Delay(100); // Check every 100 milliseconds
    //    }

    //    // Once a is true, print "Hello"
    //    Debug.Log("Hello");
    //}
    #endregion
}
