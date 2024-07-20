using System;
using UnityEngine;

// デバイスの加速度を取得する
public class M5DataReceiver : MonoBehaviour
{
    //先ほど作成したクラス
    public SerialHandler serialHandler;
    public SensorInfo sensorInfo;


    //--------------------------------------------------
    void Start()
    {
        //信号を受信したときに、そのメッセージの処理を行う
        serialHandler.OnDataReceived += OnDataReceived;
    }

    //受信した信号(message)に対する処理
    void OnDataReceived(string message)
    {
        var data = message.Split(
                new string[] { "\t" }, System.StringSplitOptions.None);
        if (data.Length < 1) return;

        try
        {
            // Debug.Log(data.Length);
            // foreach (var item in data) { Debug.Log(item); }

            string name = data[0];

            int[] btnInfo = new int[4];
            for (int i = 0; i < 4; i++) { int.TryParse(data[i + 1], out btnInfo[i]); }

            float[] imuInfo = new float[6];
            for (int j = 0; j < 6; j++) { float.TryParse(data[j + 5], out imuInfo[j]); }

            float temp;
            float.TryParse(data[11], out temp);

            sensorInfo = new SensorInfo(
                name,
                btnInfo[0],
                btnInfo[1],
                btnInfo[2],
                btnInfo[3],
                new Vector3(imuInfo[0], imuInfo[1], imuInfo[2]),
                new Vector3(imuInfo[3], imuInfo[4], imuInfo[5]),
                temp
                );

        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    // M5が取得したセンサー情報
    [Serializable]
    public struct SensorInfo
    {
        // デバイス名
        public string name;

        // ボタン状態
        // Tactは実ボタンの状態、Toggleは仮想のトグルボタンの状態
        public int btnATactSwitch;
        public int btnBTactSwitch;
        public int btnAToggleSwitch;
        public int btnBToggleSwitch;

        // 加速度
        public Vector3 accelaration;

        // ジャイロ
        public Vector3 gyro;

        // M5StickCの内部温度
        public float temp;

        // コンストラクタ
        public SensorInfo(string name, int btnATactSwitch, int btnBTactSwitch, int btnAToggleSwitch, int btnBToggleSwitch, Vector3 accelaration, Vector3 gyro, float temp)
        {
            this.name = name;
            this.btnATactSwitch = btnATactSwitch;
            this.btnBTactSwitch = btnBTactSwitch;
            this.btnAToggleSwitch = btnAToggleSwitch;
            this.btnBToggleSwitch = btnBToggleSwitch;
            this.accelaration = accelaration;
            this.gyro = gyro;
            this.temp = temp;
        }
    }
}