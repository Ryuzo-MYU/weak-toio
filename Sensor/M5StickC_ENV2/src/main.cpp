// ==============================
// M5StickC, ENV2でCO2データを取得し、シリアル通信で送信
// データリスト          対応する変数    単位
// 0:   デバイス名      DEVICE_NAME     -
// 1.   気温            temperature     ℃
// 2.   湿度            humidity        %
// 3.   気圧            pressure        Pa
// ==============================

#include <BluetoothSerial.h>
#include <M5Unified.h>
#include <Wire.h>

#include "M5UnitENV.h"

// 定数定義
const char* DEVICE_NAME = "ENV_SENSOR";
const int SERIAL_BAUD = 115200;
const int SLEEP_TIME = 3000;  // スリープ時間（ミリ秒）

// センサーインスタンス
SHT3X sht;
BMP280 bmp;
BluetoothSerial SerialBT;

float temp;
float hum;
float pa;

void UpdateInfo(float temp, float hum, float pa);

void setup() {
    auto cfg = M5.config();
    M5.begin(cfg);
    Wire.begin();

    Serial.begin(SERIAL_BAUD);
    SerialBT.begin(DEVICE_NAME);

    // センサー初期化待機
    while (!bmp.begin()) {
        delay(100);
    }
}

void loop() {
    // センサーからデータ読み取り
    while (!sht.update() || !bmp.update()) {
        delay(100);  // 更新されるまで待機
    }

    temp = bmp.cTemp;
    hum = sht.humidity;
    pa = bmp.pressure;

    UpdateInfo(temp, hum, pa);

    // ディープスリープモードに移行
    esp_sleep_enable_timer_wakeup(SLEEP_TIME * 1000);
    esp_deep_sleep_start();
}

void UpdateInfo(float temp, float hum, float pa) {
    char data[100];
    sprintf(data, "%s\t%.2f\t%.2f\t%.1f", DEVICE_NAME, temp, hum, pa);
    Serial.println(data);
    SerialBT.println(data);
}