using UnityEngine;

public class ActiveTrigger : MonoBehaviour
{
    public void activeTrigger()
    {
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);    
    }
}
