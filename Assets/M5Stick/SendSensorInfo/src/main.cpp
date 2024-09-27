#include <Arduino.h>
#include <BluetoothSerial.h>
#include <Config.h>
#include <SensorData.h>

// センサクラス
SensorData sensorData;

// 更新間隔設定(Config.h, Timer.h)
Timer loopTimer(Config::LOOP_DELAY);

// Bluetooth
BluetoothSerial SerialBT;

// データ送信関数のプロトタイプ宣言
void sendData();

void setup() {
    // 輝度0
    M5.Axp.ScreenBreath(0);

    // シリアル通信の初期化
    Serial.begin(Config::SERIAL_BAUD);    // シリアル通信の初期化
    SerialBT.begin(Config::DEVICE_NAME);  // Bluetoothシリアルの初期化

    // 初期化および初回データの取得
    sensorData.setup();
    sensorData.update();
}

void loop() {
    // センサの更新
    sensorData.update();

    // データを送信
    if (loopTimer.isTimeToUpdate()) {
        sendData();
    }
}

// データ送信関数
void sendData() {
    // 送信するデータをフォーマット
    char data[150];
    sprintf(data,
            "%s\t%.6f\t%.6f\t%.6f\t%.6f\t%.6f\t%.6f\t%.2f\t%.2f\t%.2f\t%.3f",
            Config::DEVICE_NAME, sensorData.getAccX(), sensorData.getAccY(),
            sensorData.getAccZ(), sensorData.getGyroX(), sensorData.getGyroY(),
            sensorData.getGyroZ(), sensorData.getTemperature(),
            sensorData.getHumidity(), sensorData.getPressure(),
            sensorData.getVbat());

    // Bluetoothシリアルと通常のシリアルにデータを送信
    Serial.println(data);  // 通常のシリアルモニタにも出力
    SerialBT.println(data);
}