using toio;
using UnityEngine;

public class AutoPatrol : MonoBehaviour
{
    CubeManager cubeManager;
    public int cubeCount;
    public ConnectType connectType;
    public int phase;
    public int collisionThreshold;

    [SerializeField] int distance;
    [SerializeField] int speed;
    [SerializeField] float elapsedTime;
    [SerializeField] int stop = 2;
    async void Start()
    {
        cubeManager = new CubeManager(connectType);
        await cubeManager.MultiConnect(cubeCount);

        foreach (var cube in cubeManager.cubes)
        {
            cube.collisionCallback.AddListener("AutoPatrol", OnCollision);
            cube.ConfigCollisionThreshold(collisionThreshold);

            // デフォルト状態がオフのセンサーを有効化
            await cube.ConfigMotorRead(true);
            await cube.ConfigAttitudeSensor(Cube.AttitudeFormat.Eulers, 100, Cube.AttitudeNotificationType.OnChanged);
            await cube.ConfigMagneticSensor(Cube.MagneticMode.MagnetState);
        }
    }

    private void Update()
    {
        // 巡回挙動
        //なにかに衝突したら下がって、向きを変えて、再度進む
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 2.0f)
        {
            foreach (var hundle in cubeManager.handles)
            {
                hundle.Update();
                if (phase == 0)
                {
                    Movement foward = hundle.TranslateByDist(distance, speed);
                    hundle.Move(foward, false);
                }
                else if (phase == 1)
                {
                    Movement rotate = hundle.RotateByRad(Mathf.PI / 2, speed);
                    hundle.Move(rotate);
                }
                else if (phase == 2)
                {
                    Movement back = hundle.TranslateByDist(-distance, speed);
                    hundle.Move(back, false);
                }
                phase--;
                if (phase < 0) { phase = 0; }
                elapsedTime = 0.0f;
            }
        }
    }

    void OnCollision(Cube c)
    {
        foreach (var cube in cubeManager.cubes)
        {
            Debug.Log("何かと衝突");
            cube.PlayPresetSound(2);
            phase = stop;
        }
    }

    public void VirtualCollision()
    {
        foreach (var cube in cubeManager.syncCubes)
        {
            OnCollision(cube);
        }
    }
}