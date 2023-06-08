using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BackgroundStripeController : StripeControllerBase
{
    public override Vector3 GetPosition(int id, int type)
    {
        var ratio = id * 1f / (count - 1);
        var p = Vector3.Lerp(left(type), right(type), ratio);
        return p;
    }
}
