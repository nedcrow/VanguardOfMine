using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class CommonStatics
{
    public static Dictionary<int, Color> mineColor = new Dictionary<int, Color>() {
        {0, Color.black },
        {1, Color.blue },
        {2, Color.green },
        {3, Color.red },
        {4, new Vector4(75, 0, 95, 1) },
        {5, new Vector4(90, 15, 30, 1) },
        {6, new Vector4(255, 192, 200, 1) },
        {7, Color.yellow },
        {8, Color.white },
    };

}