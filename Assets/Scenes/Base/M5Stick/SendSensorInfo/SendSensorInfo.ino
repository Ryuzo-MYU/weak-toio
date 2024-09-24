#include "BluetoothSerial.h"
#include "SensorData.h"  // センサに関するクラスファイル
#include "Config.h"      // 通信と更新間隔の設定ファイル

// センサーインスタンス(SensorData.h, EncSensor.cpp)
SensorData sensorData;
EnvSensor envSensor;

// 更新間隔設定(Config.h, SensorData.h)
Timer loopTimer(Config::LOOP_DELAY);
Timer batteryTimer(Config::BATTERY_UPDATE_INTERVAL);
Timer envTimer(Config::ENV_UPDATE_INTERVAL);
Timer sensorTimer(Config::SENSOR_UPDATE_INTERVAL);

// バッテリー情報
float vbat = 0;

// Bluetooth
BluetoothSerial SerialBT;

// データ送信関数のプロトタイプ宣言
void sendData();

void setup() {
  // M5StickCの初期化
  M5.begin();
  Wire.begin();
  M5.Axp.ScreenBreath(0);
  M5.MPU6886.Init();

  // シリアル通信の初期化
  Serial.begin(Config::SERIAL_BAUD);    // シリアル通信の初期化
  SerialBT.begin(Config::DEVICE_NAME);  // Bluetoothシリアルの初期化

  // ENVの初期化
  envSensor.begin();

  // === 初回のデータ取得 ===
  // 加速度センサとジャイロ
  M5.MPU6886.getAccelData(&sensorData.accX, &sensorData.accY, &sensorData.accZ);
  M5.MPU6886.getGyroData(&sensorData.gyroX, &sensorData.gyroY, &sensorData.gyroZ);
  // バッテリー情報
  vbat = M5.Axp.GetVbatData() * 1.1 / 1000;
  // ENV
  envSensor.update(sensorData);
}


void loop() {
  // センサデータの更新
  if (sensorTimer.isTimeToUpdate()) {
    M5.MPU6886.getAccelData(&sensorData.accX, &sensorData.accY, &sensorData.accZ);
    M5.MPU6886.getGyroData(&sensorData.gyroX, &sensorData.gyroY, &sensorData.gyroZ);
  }

  // バッテリー情報の更新
  if (batteryTimer.isTimeToUpdate()) {
    vbat = M5.Axp.GetVbatData() * 1.1 / 1000;
  }

  // ENVセンサデータの更新
  if (envTimer.isTimeToUpdate()) {
    envSensor.update(sensorData);
  }

  // データを送信
  if (loopTimer.isTimeToUpdate()) {
    sendData();
  }
}

// データ送信関数
void sendData() {
  // 送信するデータをフォーマット
  char data[150];
  sprintf(data, "%s\t%.6f\t%.6f\t%.6f\t%.6f\t%.6f\t%.6f\t%.2f\t%.2f\t%.2f\t%.3f",
          Config::DEVICE_NAME,
          sensorData.accX, sensorData.accY, sensorData.accZ,
          sensorData.gyroX, sensorData.gyroY, sensorData.gyroZ,
          sensorData.temperature, sensorData.humidity, sensorData.pressure,
          vbat);

  // Bluetoothシリアルと通常のシリアルにデータを送信
  Serial.println(data);  // 通常のシリアルモニタにも出力
  SerialBT.println(data);
}