using Notifications;
using Plugins.CountlySDK;
using Plugins.CountlySDK.Enums;
using Plugins.CountlySDK.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserDetail : MonoBehaviour
{
    public async void SetUserDetail()
    {
        CountlyUserDetailsModel userDetails = new CountlyUserDetailsModel(
                                  "Full Name", "username", "useremail@email.com", "Organization",
                                  "222-222-222",
                  "http://webresizer.com/images2/bird1_after.jpg",
          "M", "1986",
                                  new Dictionary<string, object>
                                  {
                                    { "Hair", "Black" },
                                    { "Age", "30" },
                                  });

        await Countly.Instance.UserDetails.SetUserDetailsAsync(userDetails);

    }

    public async void SetCustomUserDetail()
    {
        Dictionary<string, object> userCustomDetail = null;

        Countly.Instance.UserDetails.SetCustomUserDetails(userCustomDetail);
        userCustomDetail = new Dictionary<string, object> {
                        { "Language", "English" },
                        { "Height", "5.9" },
            };
        Countly.Instance.UserDetails.SetCustomUserDetails(userCustomDetail);
    }

    public async void SetPropertyOnce()
    {
        Countly.Instance.UserDetails.SetOnce("Distance", "10KM");
        await Countly.Instance.UserDetails.SaveAsync();

    }

    public async void IncrementValue()
    {
        Countly.Instance.UserDetails.Increment("Weight");
        await Countly.Instance.UserDetails.SaveAsync();

    }

    public async void IncrementBy()
    {
        Countly.Instance.UserDetails.IncrementBy("ShoeSize", 2);
        await Countly.Instance.UserDetails.SaveAsync();

    }

    public async void Multiply()
    {
        Countly.Instance.UserDetails.Multiply("PetNumber", 2);
        await Countly.Instance.UserDetails.SaveAsync();

    }

    public async void Max()
    {
        Countly.Instance.UserDetails.Max("TravelDistance", 90);
        await Countly.Instance.UserDetails.SaveAsync();

    }

    public async void Min()
    {
        Countly.Instance.UserDetails.Min("YearsExperience", 10);
        await Countly.Instance.UserDetails.SaveAsync();

    }

    public async void Push()
    {
        Countly.Instance.UserDetails.Push("Area", new string[] { "width", "height" });
        await Countly.Instance.UserDetails.SaveAsync();

    }

    public async void PushUnique()
    {
        Countly.Instance.UserDetails.PushUnique("Mole", new string[] { "Left Cheek", "Right Cheek" });
        await Countly.Instance.UserDetails.SaveAsync();

    }

    public async void Pull()
    {
        //Remove one or many values
        Countly.Instance.UserDetails.Pull("Cat", new string[] { "Claw" });
        await Countly.Instance.UserDetails.SaveAsync();

    }

    public async void RecordMultiple()
    {
        //Remove one or many values
        Countly.Instance.UserDetails.Max("Income", 9000);
        Countly.Instance.UserDetails.SetOnce("FavoriteColor", "Blue");
        Countly.Instance.UserDetails.Push("Inventory", new string[] { "Sword", "Shield", "Armor" });
        await Countly.Instance.UserDetails.SaveAsync();

    }
}
