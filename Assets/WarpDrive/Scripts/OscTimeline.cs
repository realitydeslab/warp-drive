using UnityEngine;
using UnityEngine.Playables;
using OscJack;

public class OscTimeline : MonoBehaviour
{
  public GameObject Portal1;
  public GameObject Portal2;
  public GameObject Portal3;
  //public GameObject TimelineController;
   void Start()
  {

Portal1.SetActive(false);
Portal2.SetActive(false);
Portal3.SetActive(false);

}

  
  public void ActivePortal(float time)
  {
  if (time >= 65.0f && time < 220.0f)
    {
        Portal1.SetActive(true);
        Portal2.SetActive(false);
        Portal3.SetActive(false);
    }
    else if (time >= 220.0f && time < 375.0f)
    {
        Portal1.SetActive(false);
        Portal2.SetActive(true);
        Portal3.SetActive(false);
    }
    else if (time >= 375.0f && time < 525.0f)
    {
        Portal1.SetActive(false);
        Portal2.SetActive(false);
        Portal3.SetActive(true);
    }
    else
    {
        Portal1.SetActive(false);
        Portal2.SetActive(false);
        Portal3.SetActive(false);
    }
}
}