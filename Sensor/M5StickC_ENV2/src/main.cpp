// ==============================
// M5StickC, ENV2でCO2データを取得し、シリアル通信で送信
// データリスト          対応する変数    単位
// 0:   デバイス名      DEVICE_NAME     -
// 1.   気温            temperature     ℃
// 2.   湿度            humidity        %
// 3.   CO2濃度         co2             ppm
// 参考:
// https://github.com/m5stack/M5Unit-ENV/blob/master/examples/Unit_ENV_M5StickC/Unit_ENV_M5StickC.ino
// ==============================

#include <BluetoothSerial.h>
#include <M5Unified.h>
#include <Wire.h>

#include "M5UnitENV.h"

// 定数定義
const char* DEVICE_NAME = "ENV_SENSOR";
const uint32_t SERIAL_BAUD = 115200;
const int LOOP_DELAY = 3000;  // 更新間隔

// オブジェクト初期化
BluetoothSerial SerialBT;

SHT3X sht;
BMP280 bmp;

float temp;
float hum;
float pa;

void UpdateInfo(float temp, float hum, float pa);

void setup() {
    M5.begin();
    Wire.begin();
    Serial.begin(SERIAL_BAUD);
    SerialBT.begin(DEVICE_NAME);

    // LCD初期化
    M5.Lcd.setRotation(3);
    M5.Lcd.fillScreen(BLACK);
    M5.Lcd.setTextColor(WHITE);
}

void loop() {
    if (sht.update()) {
        temp = sht.cTemp;
        hum = sht.humidity;
    }

    if (bmp.update()) {
        temp = bmp.cTemp;
        pa = bmp.pressure;
    }

    UpdateInfo(temp, hum, pa);

    delay(LOOP_DELAY);
}

void UpdateInfo(float temp, float hum, float pa) {
    char data[100];
    // データ送信文字列の作成
    sprintf(data, "%s\t%.2f\t%.2f\t%.2f", DEVICE_NAME,
            temp,  // 気温（BMP280の値を使用）
            hum,   // 湿度
            pa);   // 気圧
    // データの送信
    Serial.println(data);
    SerialBT.println(data);

    M5.update();

    // LCD表示の更新
    M5.Lcd.setCursor(0, 10);
    M5.Lcd.printf("Temp: %.1f C", bmp.cTemp);
    M5.Lcd.setCursor(10, 20);
    M5.Lcd.printf("Hum : %.1f %%", dht.humidity);
    M5.Lcd.setCursor(10, 30);
    M5.Lcd.printf("Pres: %.0f Pa", bmp.pressure);

    delay(LOOP_DELAY);
}