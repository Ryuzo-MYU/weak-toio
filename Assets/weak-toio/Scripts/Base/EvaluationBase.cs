using System.Collections;
using System.Collections.Generic;
using Environment;
using Evaluation;
using Robot;
using UnityEngine;

public abstract class EvaluationBase<TSensor, TEvaluate, TActionSender> : MonoBehaviour
	where TSensor : ISensorUnit
	where TEvaluate : EvaluationResultSender<TSensor>
	where TActionSender : ActionSender
{
	[SerializeField] protected string toioName;
	[SerializeField] protected string attachedToioName;
	public ToioConnector connector;
	public string PORTNAME = "COM9";
	public int BAUDRATE = 115200;
	[SerializeField] protected float envParam;
	protected SerialHandler serial;
	protected TSensor sensor;
	public bool UseDummy;
	public EnvType envType = EnvType.NotAppointed;
	protected TEvaluate evaluator;
	protected TActionSender actionGenerator;
	protected bool connected = false;
	protected Toio toio;

	protected virtual void Awake()
	{
		connector.OnConnectSuccessed += OnConnectSuccessed;
		InitializeSerial();
		InitializeSensor();
		InitializeEvaluator();
	}

	protected virtual void Start()
	{
		sensor.Start();
	}

	protected virtual void Update()
	{
		serial.Update();
		sensor.Update();
	}

	protected virtual void OnConnectSuccessed(List<Toio> toios)
	{
		Debug.Log("接続開始");
		toio = toios.Find(t => t.Name == toioName);
		attachedToioName = toio.Name;
		toio.EnvType = sensor.GetEnvType();
		connected = true;
		StartCoroutine(UpdateEvaluate());
	}

	protected abstract void InitializeSensor();
	protected abstract void InitializeEvaluator();

	private void InitializeSerial()
	{
		serial = new SerialHandler(PORTNAME, BAUDRATE);
		UseDummy = !serial.Awake();
	}

	protected IEnumerator UpdateEvaluate()
	{
		while (true)
		{
			if (!connected && sensor == null)
			{
				yield return new WaitForSeconds(0.1f);
				continue;
			}

			Result result = evaluator.GetEvaluationResult(sensor);
			Robot.Action action = actionGenerator.GenerateAction(result);

			if (!toio.AddNewAction(action))
			{
				Debug.LogWarning("アクション溜まってんね");
			}

			yield return StartCoroutine(toio.Move());
		}
	}
}