using UnityEngine;

namespace Assets.Scripts.Util
{

    // Use CallAfterDelay to cause something to happen later in Unity3D.

    // For instance:

    // CallAfterDelay.Create( 2.0f, () => {
    //  Debug.Log( "This is two seconds later.");
    // });

    // To make it survive from scene to scene, simply set the returned reference as DontDestroyOnLoad(), like so:

    // DontDestroyOnLoad(CallAfterDelay.Create(2.0f, () => {
    //     Debug.Log("This is two seconds later and survives scene changes.");
    // }));

public class CallAfterDelay : MonoBehaviour
    {
        float delay;
        System.Action action;

        // Will never call this frame, always the next frame at the earliest
        public static CallAfterDelay Create(float delay, System.Action action)
        {
            CallAfterDelay cad = new GameObject("CallAfterDelay").AddComponent<CallAfterDelay>();
            cad.delay = delay;
            cad.action = action;
            return cad;
        }

        float age;

        void Update()
        {
            if (age > delay)
            {
                action();
                Destroy(gameObject);
            }
        }
        void LateUpdate()
        {
            age += Time.deltaTime;
        }
    }
}
