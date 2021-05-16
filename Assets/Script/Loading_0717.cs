using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Scene과 관련된 모든 것을 하기위한 선언.


public class Loading_0717 : MonoBehaviour
{
    // Scene 전환 작업을 효율적으로 가능하게 해줌
    private AsyncOperation async;
    //public float time = 0f;

	// Use this for initialization
	void Start ()
    {
        // LoadSceneAsync 는 Scene을 비동기식으로 호출
        // 동기식으로 호출시 로딩화면 전 씬은 멈춤.
        // 비동기식으로 호출시 로딩화면 전 씬 모두 활동.
        async = SceneManager.LoadSceneAsync(DontDestroy.SceneName);

        // 다음 씬으로 전환할지 여부를 정함.
        async.allowSceneActivation = false;

	}
	
	// Update is called once per frame
	void Update ()
    {
        // 로딩 상태가 90%이상 진행될 시 씬을 전환해줌
        if (async.progress >= 0.9f)
            async.allowSceneActivation = true;

        //time += Time.deltaTime;
        //
        //if(time >= 2f)
        //{
        //    SceneManager.LoadScene(DontDestroy_0717.SceneName);
        //}
	}
}
