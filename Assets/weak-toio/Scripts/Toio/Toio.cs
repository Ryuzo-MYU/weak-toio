using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;
using ActionLibrary;
using System;

namespace ActionGenerate
{
  public class Toio : MonoBehaviour
  {
    [SerializeField] private int _id;
    [Tooltip("toio- 以降の文字列を記入")]
    [SerializeField] private string _localName;
    [SerializeField] private List<EnvType> type;
    private Cube _cube;
    private CubeHandle _handle;

    public int ID { get { return _id; } }
    public string LocalName
    {
      get
      {
        ConnectType connectType = FindObjectOfType<ToioConnecter>().connectType;
        if (connectType == ConnectType.Real)
        {
          return "toio-" + _localName;
        }
        else
        {
          return this.name;
        }
      }
    }
    public List<EnvType> Type { get { return type; } }
    public Cube Cube { get { return _cube; } }
    public CubeHandle Handle { get { return _handle; } }

    private bool moveCommandCompleted;
    private bool lightCommandCompleted;
    private bool soundCommandCompleted;
    [SerializeField] private int actionMaxCount;
    private Queue<Action> actions;
    private Action normalAction;
    private Action emergencyAction;
    private bool isMoving = false;

    private ActionGenerator actionGenerator;

    [SerializeField] private int CollisionThreshold = 7;
    [SerializeField] string CallbackSceneName = "SampleScene";

    private void Start()
    {
      actions = new Queue<Action>();
      normalAction = new Action();

      actionGenerator = gameObject.GetComponent<ActionGenerator>();
      actionGenerator.OnActionGenerated += AddNewAction;
    }

    /// <summary>
    /// ToioConnecterが接続しているToioから，自分のToioの情報をもらう
    /// CubeManager不使用版
    /// </summary>
    /// <param name="cube">toio-sdkのCubeクラス</param>
    public void Register(Cube cube)
    {
      this._cube = cube;
      this._handle = new CubeHandle(cube);
      Cube.ConfigCollisionThreshold(CollisionThreshold);
      Cube.collisionCallback.AddListener(CallbackSceneName, OnCubeCollisioned);
      OnRegisterCompleted();
    }

    /// <summary>
    /// ToioConnecterが接続しているToioから，自分のToioの情報をもらう
    /// CubeManager使用版
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="handle"></param>
    public void Register(Cube cube, CubeHandle handle)
    {
      this._cube = cube;
      this._handle = handle;
      Cube.collisionCallback.AddListener(CallbackSceneName, OnCubeCollisioned);
      OnRegisterCompleted();
    }

    private void OnRegisterCompleted()
    {
      actionGenerator.InitializeCubeHandle(Handle);
      StartCoroutine(StartAct());
    }

    // actionを実行するコルーチン
    // 平常時はcurrentActionを実行，緊急アクションがあればemergencyActionを処理する
    public IEnumerator StartAct()
    {
      while (true)
      {
        if (emergencyAction != null && emergencyAction.Count() != 0)
        {
          Debug.Log("緊急アクションを実行します");
          yield return Act(emergencyAction);
          continue;
        }

        if (normalAction == null || normalAction.Count() == 0)
        {
          yield return normalAction = actionGenerator.GetNewAction();
          AllCommandFlagReset();

          if (normalAction == null)
          {
            yield return new WaitForSeconds(0.1f);
            continue;
          }
        }
        yield return Act(normalAction);
      }
    }

    private IEnumerator ProcessCommands<T>(Action action, Func<Action, Queue<T>> getCommands) where T : IToioCommand
    {
      Queue<T> commands = getCommands(action);
      while (commands.Count > 0)
      {
        var command = commands.Dequeue();
        if (command != null)
        {
          Debug.Log($"アクション時間 : {command.GetInterval():F3}[s]\nコマンドタイプ : {command.GetType()}");
          command.Exec(this);
          yield return new WaitForSeconds(command.GetInterval());
        }
      }
    }

    private IEnumerator Act(Action action)
    {
      while (action != null && action.Count() > 0)
      {
        Debug.Log($"アクション開始\nこのアクションの時間 : {action.GetInterval():F3}[s]");

        var moveTask = StartCoroutine(ProcessCommands<IMovementCommand>(action, a => a.GetMovements()));
        var lightTask = StartCoroutine(ProcessCommands<ILightCommand>(action, a => a.GetLightCommands()));
        var soundTask = StartCoroutine(ProcessCommands<ISoundCommand>(action, a => a.GetSoundCommands()));

        yield return moveTask;
        yield return lightTask;
        yield return soundTask;

        Debug.Log("全アクションが終了するまで待機");

        yield return new WaitForSeconds(action.GetInterval());
      }
    }

    /// <summary>
    /// actionを消す
    /// </summary>
    public void Stop()
    {
      isMoving = false;
      normalAction.Clear();
    }

    /// <summary>
    /// アクションを追加する
    /// </summary>
    /// <param name="action"></param>
    public void AddNewAction(Action action)
    {
      if (action == null)
      {
        Debug.LogWarning("actionがnullです");
        return;
      }
      if (actions.Count > actionMaxCount)
      {
        Debug.Log("アクション溜まりすぎ");
        return;
      }
      actions.Enqueue(action);
      Debug.Log("アクション足しました");

      Stop();
      if (!isMoving)
      {
        isMoving = true;
      }
    }

    /// <summary>
    /// 緊急アクションを追加する
    /// </summary>
    /// <param name="action"></param>
    public void AddEmergencyAction(Action action)
    {
      if (action == null)
      {
        Debug.LogWarning("null のアクション送るな");
        return;
      }
      Debug.Log("緊急アクションを追加");
      emergencyAction = action;
      if (!isMoving)
      {
        isMoving = true;
      }
    }

    /// <summary>
    /// 衝突時に方向転換する緊急アクションを追加する
    /// </summary>
    /// <param name="c"></param> <summary>
    private void OnCubeCollisioned(Cube c)
    {
      Debug.Log("衝突");
      ToioActionLibrary lib = new ToioActionLibrary(Handle);
      AddEmergencyAction(lib.Collisioned());
    }

    private bool AllCommandCompleted()
    {
      bool[] flags = {
        moveCommandCompleted,
        lightCommandCompleted,
        soundCommandCompleted
      };
      foreach (bool flag in flags)
      {
        if (!flag)
        {
          return false;
        }
      }
      return true;
    }

    private void AllCommandFlagReset()
    {
      moveCommandCompleted = false;
      lightCommandCompleted = false;
      soundCommandCompleted = false;
    }
  }
}