using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransport : MonoBehaviour
{
    public static bool hasNoMap = true;
    public static bool hasUnit = false;

    public static List<Battle> battleList = new List<Battle>();

    public static int Respawn_X;//Respawn at first village
    public static int Respawn_Y;
}
