using UnityEngine;

public class btninfo : MonoBehaviour
{
    public GameObject mainmenupanel;
    public GameObject infopanel;
    public GameObject settingpanel;
    public GameObject playpanel;
    public void Start()
    {
        mainmenupanel.SetActive(true);
        infopanel.SetActive(false);
        settingpanel.SetActive(false);
        playpanel.SetActive(false);
    }
    public void info()
    {
        mainmenupanel.SetActive(false);
        infopanel.SetActive(true);
        settingpanel.SetActive(false);
        playpanel.SetActive(false);
    }
    public void back()
    {
        mainmenupanel.SetActive(true);
        infopanel.SetActive(false);
        settingpanel.SetActive(false);
        playpanel.SetActive(false);
    }
    public void setting()
    {
        mainmenupanel.SetActive(false);
        infopanel.SetActive(false);
        settingpanel.SetActive(true);
        playpanel.SetActive(false);
    }
    public void play()
    {
        mainmenupanel.SetActive(false);
        infopanel.SetActive(false);
        settingpanel.SetActive(false);
        playpanel.SetActive(true);
    }
}
