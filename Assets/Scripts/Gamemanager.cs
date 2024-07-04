using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Threading.Tasks;

public class Gamemanager : MonoBehaviour
{
    public SaveSystem saveSystem;
    #region Singleton
    public static Gamemanager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }


    }
    #endregion

    public GameObject MainButtonPanel;
    public Button TrashTrooper_Button;
    public Button House_Button;
    public Button DumpYard_Button;

    public GameObject Areas;
    public GameObject TrashTrooperPanel;
    public Button button_TrashClose;

    public bool IsDontWantTutorial;
    public GameObject MainIntractionPanel;
    public GameObject TopPanel;
    public GameObject DownPanel;
    public GameObject MainButtons_Image;

    public GameObject CameraObject;
    public UnityEvent[] Tutorial;
    public int int_TutorialCount;

    public TMP_Text text_Money;
    public TMP_Text text_TrashCollect;
    public int TrashCollect;

    public int Money;
    public int CarCount;

    #region HM Character
    public TMP_Text HM_Text;
    public GameObject MsgPanel;
    public TMP_Text Msg_Text;
    public GameObject CenterMsgPanel;
    public TMP_Text CenterMsg_Text;
    public AudioSource BG;
    #endregion

    public House house;
    public TrashTrooper trashTrooper;
    public DumpYard dumpYard;

    // Start is called before the first frame update
    async void Start()
    {
        Progress p = saveSystem.playerProgress;

        IsDontWantTutorial = p.IsDontWantTutorial;
        Money = p.money;
        CarCount = p.carCount;
        TrashCollect = p.TrashCollect;

        text_TrashCollect.text = TrashCollect.ToString();
        text_Money.text = Money.ToString();
        await Task.Delay(2 * 1000);

        OnButtonClickEvent();

        if (!IsDontWantTutorial)
        {
            StepNumberUpdate(0);
            TutorialEventCall();
        }
        else
        {
            BG.Play();
            MainIntractionPanel.SetActive(true);
            TopPanel.SetActive(true);
            DownPanel.SetActive(true);
            MainButtons_Image.SetActive(true);
            TrashTrooper_Button.gameObject.SetActive(true);
            CameraObject.GetComponent<PanZoom>().StartPanTouch = true;

            house.Onclick_WhenStart();
            trashTrooper.Onclick_WhenStart(CarCount);
            dumpYard.Onclick_DumpYadLockUnLock(false);
        }
    }
    void OnButtonClickEvent()
    {
        TrashTrooper_Button.onClick.AddListener(() => OnCameraFocusArea(0,50));
        button_TrashClose.onClick.AddListener(() => Onclick_TrashClose());
        House_Button.onClick.AddListener(() => OnCameraFocusArea(1,50));
        DumpYard_Button.onClick.AddListener(() => OnCameraFocusArea(2,50));
    }

    void OnCameraFocusArea(int TempNum,int TempZoom)
    {
        CameraObject.GetComponent<PanZoom>().OrthographicCameraLocateOn(TempNum, TempZoom);
        //MainButtonPanel.SetActive(false);
        switch(TempNum)
        {
            case 0:
                MainButtonPanel.SetActive(false);
                Areas.SetActive(true);
                MsgPanel.SetActive(false);
                TrashTrooperPanel.SetActive(true);
                break;

        }
    }
    void Onclick_TrashClose()
    {
        MainButtonPanel.SetActive(true);
        Areas.SetActive(false);
        TrashTrooperPanel.SetActive(false);
    }
    #region Tutorial
    public void Onclick_TutorialDeactive()
    {
        IsDontWantTutorial = true;
        CenterMsgPanel.SetActive(false);
    }
    public void StepNumberUpdate(int TempStepNum)
    {
        int_TutorialCount = TempStepNum;
    }
    public void TutorialEventCall()
    {
        Tutorial[int_TutorialCount].Invoke();
    }
    public void Onclick_Tutorial()
    {
        switch (int_TutorialCount)
        {
            case 0:
                CameraObject.GetComponent<PanZoom>().OnCameraLocatToNPC();
                HM_Text.text = "Hey, Welcome to TRASH TOWN!" +
                    "\nI'm the only one left in this once-thriving town";
                //int_TutorialCount = 1;
                break;
            case 2:
                HM_Text.text = "Today, I appoint you as the MAYOR, please restore our town to it's former glory!";
                //int_TutorialCount = 1;
                break;
            case 5:
                //MsgPanel.SetActive(true);
                //Areas.SetActive(false);
                //Msg_Text.text = "The loading time of Trucks seems be too slow, lets increase it";
                //CameraObject.GetComponent<PanZoom>().OrthographicCameraLocateOn(1,50);

                //Msg_Text.text = "Where will the trucks dump the trash? Let's unlock the DumpYard!";
                //int_TutorialCount = 1;
                break;
            case 6:
                MsgPanel.SetActive(true);
                Areas.SetActive(false);
                Msg_Text.text = "Where will the trucks dump the trash? Let's unlock the DumpYard!";
                CameraObject.GetComponent<PanZoom>().OrthographicCameraLocateOn(2,50);
                //int_TutorialCount = 1;
                break;
            case 8:
                //CameraObject.GetComponent<PanZoom>().OnCameraLocatToNPC();
                //HM_Text.text = "Wow, my house looks great again! Now let's upgrade it";
                CenterMsgPanel.SetActive(true);
                Areas.SetActive(false);
                CenterMsg_Text.text = "Great job as a Mayor so far! Hope you will make this town the cleanest in the world. All the Best";
                //int_TutorialCount = 1;
                break;
        }
    }
    public async void WaitToPlayNextStep(int TempTime)
    {
        await Task.Delay(TempTime * 1000);
        TutorialEventCall();
    }
    #endregion

    public void OnMoneyCollect(int TempIncrement)
    {
        Money += TempIncrement;
        text_Money.text = Money.ToString();
        Onclick_SaveSystem();
    }
    public void OnTrashCollect(int TempIncrement)
    {
        TrashCollect += TempIncrement;
        text_TrashCollect.text = TrashCollect.ToString();
        Onclick_SaveSystem();
    }

    public void Onclick_SaveSystem()
    {
        Progress p = new Progress();

        p.IsDontWantTutorial = IsDontWantTutorial;
        p.money = Money;
        p.carCount = CarCount;
        p.TrashCollect = TrashCollect;

        if (IsDontWantTutorial)
        {
            saveSystem.SaveProgress(p);
        }
    }

}
