using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public class TrashTrooper : MonoBehaviour
{
    public GameObject TrafficPoint;
    public bool IsFreeAvilabel = true;
    public int FreeHireTruck = 3;
    public Button button_HireTruck;
    public int TruckActiveCount;
    public TMP_Text Text_Quantity;
    public GameObject[] Trucks;
    public int RequiredAmountToHireTruck;


    public Slider slider_TruckSpeed;
    public TMP_Text text_TruckLevelSet;
    public int TruckLevel;
    public int TruckLevelRequiredMoney;
    public TMP_Text text_TruckMoney;
    public GameObject TextPopUp;
    private void Start()
    {
        button_HireTruck.onClick.AddListener(() => Onclick_HireTruck());
    }
    public async void VehcileReached(GameObject TempGameObject)
    {
        Vehcile vehcile = TempGameObject.GetComponent<Vehcile>();
        while (vehcile.NotReachedTarget)
        {
            await Task.Delay(100); // Check every 100 milliseconds
        }

        TempGameObject.SetActive(false);

        await Task.Delay((int)10 * 1000);
        TempGameObject.SetActive(true);

        TextPopUp.SetActive(true);
    }

    public void Onclick_AvilableFree()
    {
        IsFreeAvilabel = true;
        FreeHireTruck = 3;
        RequiredAmountToHireTruck = 800;
    }
    async void Onclick_HireTruck()
    {
        if (IsFreeAvilabel)
        {
            if (FreeHireTruck > 0)
            {
                button_HireTruck.interactable = false;

                FreeHireTruck--;
                Trucks[TruckActiveCount].SetActive(true);
                TruckActiveCount++;

                await Task.Delay((int)1 * 1000);
                IsFreeAvilabel = FreeHireTruck == 0 ? false : true;

                await Task.Delay((int)1 * 1000);
                button_HireTruck.interactable = true;
                button_HireTruck.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text =
                    "Hire\nFree";
                if (FreeHireTruck == 0)
                {
                    button_HireTruck.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text =
                    "Hire\n" + RequiredAmountToHireTruck;
                    Gamemanager.Instance.StepNumberUpdate(5);
                    Gamemanager.Instance.WaitToPlayNextStep(15);
                }
            }
        }
        else
        {
            if (RequiredAmountToHireTruck < Gamemanager.Instance.Money)
            {
                button_HireTruck.interactable = false;

                Trucks[TruckActiveCount].SetActive(true);
                TruckActiveCount++;
                await Task.Delay((int)3 * 1000);
                button_HireTruck.interactable = true;
                button_HireTruck.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text =
                    "Hire\n" + RequiredAmountToHireTruck;
                Gamemanager.Instance.Money -= 800;
            }
        }
        Gamemanager.Instance.CarCount = TruckActiveCount;
        Gamemanager.Instance.Onclick_SaveSystem();
        Text_Quantity.text = "Quantity :\n" + " " + TruckActiveCount + "/10";
    }

    public void Onclick_UpgradeTruckSpeed()
    {
        if (TruckLevelRequiredMoney < Gamemanager.Instance.Money)
        {
            TruckLevelRequiredMoney += 1;
            TruckLevel += 1;
            slider_TruckSpeed.value = TruckLevel;
            text_TruckLevelSet.text ="Lv."+ TruckLevel.ToString();
            text_TruckMoney.text = TruckLevelRequiredMoney.ToString();
        }
        //else
    }

    public async void Onclick_WhenStart(int TempTruckCount)
    {
        IsFreeAvilabel = false;
        TruckActiveCount = TempTruckCount;
        button_HireTruck.interactable = false;
        button_HireTruck.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text =
      "Hire\n" + RequiredAmountToHireTruck;
        Text_Quantity.text = "Quantity :\n" + " " + TruckActiveCount + "/10";
     
        for (int i = 0; i < TempTruckCount; i++)
        {
            await Task.Delay((int)3 * 1000);
            Trucks[i].SetActive(true);
        }
        Gamemanager.Instance.OnMoneyCollect(200);
        button_HireTruck.interactable = true;
    }
}
