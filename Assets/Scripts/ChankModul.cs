using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChankModul : MonoBehaviour
{

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ChankBuilder chankBuilder))
        {
            chankBuilder.Update_Player_Position();
        }
    }

}
