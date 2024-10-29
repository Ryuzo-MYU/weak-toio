// ==============================
// M5StickC, ENV2で環境データを取得し、シリアル通信で送信
// データリスト			対応する変数	単位
// 0: 	デバイス名 		DEVICE_NAME		-
// 1: 	X軸加速度 		accX			g(9.8 m/s^2)
// 2. 	Y軸加速度 		accY			g(9.8 m/s^2)
// 3. 	Z軸加速度 		accZ			g(9.8 m/s^2)
// 4. 	X軸ジャイロ 	gyroX			deg/s
// 5.	Y軸ジャイロ		gyroY			deg/s
// 6.	Z軸ジャイロ		gyroZ			deg/s
// 7.	気温		   	temperature		℃
// 8.	湿度			humidity		%
// 9.	気圧			pressure		Pa
// 10.	バッテリー電圧	 vbat			mV
// ==============================

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
    M5.begin();

    M5.Lcd.setRotation(3);  // ディスプレイの向きを調整（お好みで）
    // 輝度設定
    M5.Lcd.fillScreen(BLACK);
    M5.Lcd.setTextColor(WHITE);
    // 文字サイズ設定とデバイス名の表示
    M5.Lcd.setTextSize(2);
    M5.Lcd.setCursor(10, 30);  // 表示位置調整
    M5.Lcd.print(Config::DEVICE_NAME);

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