using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BackgroundStripeController : StripeControllerBase
{
    public override Vector3 GetPosition(StripeBase s)
    {
        var ratio = s.id * 1f / (count - 1);
        var p = Vector3.Lerp(left(s.type), right(s.type), ratio);
        return p;
    }
}
