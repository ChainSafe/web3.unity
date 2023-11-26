#nullable enable
using System;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    // todo remove
    // public abstract class RampExchanger
    // {
    //     protected readonly RampData _rampData;
    //     
    //     //It is very difficult to create a struct that matches 1:1 from swift/obj-c to C#,
    //     //That's why I'm passing so  many parameters, but our end-users (i.e. the developers) wouldn't have to worry about it
    //     //since they would be using the OnRampPurchaseData/OffRampSaleData classes for retrieving the infos.
    //     #region On Ramp
    //     
    //     public static  Action<OnRampPurchaseData> OnRampPurchaseEvent = null!;
    //     #endregion
    //     #region Off Ramp
    //     
    //     
    //     public static Action<OffRampSaleData>? OffRampSaleEvent = null;
    //     #endregion
    //
    //     protected RampExchanger(RampData rampData)
    //     {
    //         _rampData = rampData;
    //     }
    //
    //     public abstract void OpenRamp();
    // }
}