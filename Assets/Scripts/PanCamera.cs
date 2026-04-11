using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PanCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera fpCamera;
    [SerializeField] private float panDuration = 1.5f;
    [SerializeField] private AnimationCurve panCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private Coroutine LookAtFunc;

    void Start() { }
    void Update() { }

    public void PanTo(Vector3 target)
    {
        if (LookAtFunc != null) StopCoroutine(LookAtFunc);
        LookAtFunc = StartCoroutine(LookAt(target));
    }

    private IEnumerator LookAt(Vector3 target)
    {
        Quaternion startRotation = fpCamera.transform.rotation;
        float elapsed = 0f;

        Vector3 direction = target - fpCamera.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (elapsed < panDuration)
        {
            elapsed += Time.deltaTime;
            float t = panCurve.Evaluate(elapsed / panDuration);

            

            fpCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        Vector3 finalDirection = target - fpCamera.transform.position;
        fpCamera.transform.rotation = Quaternion.LookRotation(finalDirection);
    }
}