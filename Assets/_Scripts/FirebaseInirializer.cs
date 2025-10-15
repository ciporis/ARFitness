using Firebase;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseInirializer : MonoBehaviour
{
    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(OnDependencyStatusReceived);
    }

    private void OnDependencyStatusReceived(Task <DependencyStatus> task)
    {
        try
        {
            if (!task.IsCompletedSuccessfully)
                throw new Exception("Could not resolve all Firebase dependencies", task.Exception);

            DependencyStatus status = task.Result;

            if (status != DependencyStatus.Available)
                throw new Exception($"Could not resolve all Firebase dependencies: {status}");

            print("Firebase initialized successfully!");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
