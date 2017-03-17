using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeyManager : MonoBehaviour {
    public int teamId;

    void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            GameManager.Instance
                .GetPlayer(teamId);
                //.Something()
        }
    }
}
