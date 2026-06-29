using UnityEngine;

public class btninfo : MonoBehaviour
{
    public GameObject mainmenupanel;
    public GameObject infopanel;
    public GameObject settingpanel;
    public GameObject playpanel;

    [SerializeField] private MainMenuParallax parallax;

    public void Start()
    {
        parallax.enabled = true;
        mainmenupanel.SetActive(true);
        infopanel.SetActive(false);
        settingpanel.SetActive(false);
        playpanel.SetActive(false);
    }

    public void info()
    {
        parallax.enabled = false;
        mainmenupanel.SetActive(true);
        infopanel.SetActive(true);
        settingpanel.SetActive(false);
        playpanel.SetActive(false);
    }

    public void back()
    {
        parallax.enabled = true;
        mainmenupanel.SetActive(true);
        infopanel.SetActive(false);
        settingpanel.SetActive(false);
        playpanel.SetActive(false);
    }

    public void setting()
    {
        parallax.enabled = false;
        mainmenupanel.SetActive(true);
        infopanel.SetActive(false);
        settingpanel.SetActive(true);
        playpanel.SetActive(false);
    }

    public void play()
    {
        parallax.enabled = false;
        mainmenupanel.SetActive(true);
        infopanel.SetActive(false);
        settingpanel.SetActive(false);
        playpanel.SetActive(true);
    }
}
