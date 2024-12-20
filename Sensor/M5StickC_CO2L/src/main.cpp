// ==============================
// M5StickC, ENV2でCO2データを取得し、シリアル通信で送信
// データリスト          対応する変数    単位
// 0:   デバイス名      DEVICE_NAME     -
// 1:   CO2濃度         co2             ppm
// 2.   気温            temperature     ℃
// 3.   湿度            humidity        %
// ==============================

#include <BluetoothSerial.h>
#include <M5Unified.h>

#include "M5UnitENV.h"

// 定数定義
const char* DEVICE_NAME = "M5_CO2";
const int SERIAL_BAUD = 115200;
const int LOOP_DELAY = 1000;  // 1秒間隔

// センサーインスタンス
SCD4X scd4x;
BluetoothSerial SerialBT;

// センサーデータ保存用変数
float temperature;
float humidity;
float co2;

void setup() {
    auto cfg = M5.config();
    M5.begin(cfg);

    M5.Lcd.setRotation(3);
    M5.Lcd.fillScreen(BLACK);
    M5.Lcd.setTextColor(WHITE);
    M5.Lcd.setTextSize(1);  // テキストサイズを小さくして全体を表示
    M5.Lcd.setCursor(5, 5);
    M5.Lcd.print(DEVICE_NAME);

    Serial.begin(SERIAL_BAUD);
    SerialBT.begin(DEVICE_NAME);

    // CO2センサーの初期化
    if (!scd4x.begin(&Wire, 0x76, 0x44)) {
        Serial.println("CO2 init failed");
        while (1) {
            delay(1);
        }
    }
}

void loop() {
    // センサーからデータ読み取り
    scd4x.update();

    // データ送信
    char data[100];
    sprintf(data, "%s\t%.2f\t%.2f\t%.1f", DEVICE_NAME, scd4x.getTemperature(),
            scd4x.getHumidity(), scd4x.getCO2());

    Serial.println(data);
    SerialBT.println(data);

    M5.update();

    // LCD表示更新
    M5.Lcd.setCursor(10, 20);
    M5.Lcd.printf("Temp: %.1f C", temperature);
    M5.Lcd.setCursor(10, 35);
    M5.Lcd.printf("Hum:  %.1f %%", humidity);
    M5.Lcd.setCursor(10, 50);
    M5.Lcd.printf("CO2:  %.0f ppm", co2);

    delay(LOOP_DELAY);
}