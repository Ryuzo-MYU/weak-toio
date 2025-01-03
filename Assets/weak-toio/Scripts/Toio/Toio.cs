using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;
using UnityEditor;

namespace ActionGenerate
{
	public class Toio : MonoBehaviour
	{
		[SerializeField] private int _id;
		[SerializeField, LocalName] private string _localName; // Change _name to _localName
		[SerializeField] private List<EnvType> type;
		private Cube _cube;
		private CubeHandle _handle;

		public int ID { get { return _id; } }
		public string LocalName
		{
			get
			{
				ToioConnecter toioConnecter = GameObject.FindGameObjectWithTag("ToioConnecter");
				return "toio-" + _localName;
			}
		}
		public List<EnvType> Type { get { return type; } }
		public Cube Cube { get { return _cube; } }
		public CubeHandle Handle { get { return _handle; } }

		[SerializeField] private int movementCount;
		[SerializeField] private int lightCount;
		[SerializeField] private int soundCount;
		[SerializeField] private int actionMaxCount;
		private Queue<Action> actions;
		private Action currentAction;
		private bool isMoving = false;

		private ActionGenerator actionGenerator;

		private void Start()
		{
			actions = new Queue<Action>();
			currentAction = new Action();

			actionGenerator = gameObject.GetComponent<ActionGenerator>();
			actionGenerator.OnActionGenerated += AddNewAction;
		}

		public void Register(int id, CubeManager cubeManager)
		{
			this._id = id;
			this._cube = cubeManager.cubes[_id];
			this._handle = cubeManager.handles[_id];

			OnRegisterCompleted();
		}

		public void Register(Cube cube)
		{
			this._cube = cube;
			this._handle = new CubeHandle(cube);
			OnRegisterCompleted();
		}

		private void OnRegisterCompleted()
		{
			StartCoroutine(actionGenerator.StartMove(this));
		}

		// 実行関連のメソッド
		public IEnumerator Act()
		{
			if (currentAction == null || currentAction.Count() == 0)
			{
				if (actions.Count > 0)
				{
					currentAction = actions.Dequeue();
					Debug.Log("アクション無いんで入れ替えますね");
				}
				else
				{
					yield return null;
				}
			}

			while (currentAction.Count() > 0)
			{
				StartCoroutine(Move());
				StartCoroutine(ControllLED());
				StartCoroutine(PlaySound());
			}
			yield return null;
		}

		private IEnumerator Move()
		{
			if (currentAction.MovementCount() == 0) yield return null;
			IMovementCommand move = currentAction.GetNextMovement();
			movementCount = currentAction.MovementCount();
			if (move != null)
			{
				move.Exec(this);
				yield return new WaitForSeconds(move.GetInterval());
			}
			yield return null;
		}

		private IEnumerator ControllLED()
		{
			if (currentAction.LightCount() == 0) yield return null;
			ILightCommand light = currentAction.GetNextLight();
			lightCount = currentAction.LightCount();
			if (light != null)
			{
				light.Exec(this);
				yield return new WaitForSeconds(light.GetInterval());
			}
			yield return null;
		}

		private IEnumerator PlaySound()
		{
			if (currentAction.SoundCount() == 0) yield return null;
			ISoundCommand sound = currentAction.GetNextSound();
			soundCount = currentAction.SoundCount();
			if (sound != null)
			{
				sound.Exec(this);
				yield return new WaitForSeconds(sound.GetInterval());
			}
			yield return null;
		}

		public void Stop()
		{
			isMoving = false;
			actions.Clear();
			currentAction = null;
			_handle.Update();
			_handle.Move(new Movement(_handle, 0, 0));
		}

		public void AddNewAction(Action action)
		{
			if (action == null)
			{
				Debug.LogWarning("null のアクション送るな");
				return;
			}
			if (actions.Count > actionMaxCount)
			{
				Debug.Log("アクション溜まりすぎ");
				return;
			}
			actions.Enqueue(action);
			Debug.Log("アクション足しました");

			if (!isMoving)
			{
				isMoving = true;
			}

		}
	}
}

// Custom Property Drawer for LocalName
[CustomPropertyDrawer(typeof(LocalNameAttribute))]
public class LocalNameDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("toio-"));
		property.stringValue = EditorGUI.TextField(position, property.stringValue);
		EditorGUI.EndProperty();
	}
}

// Attribute to mark the LocalName field
public class LocalNameAttribute : PropertyAttribute { }