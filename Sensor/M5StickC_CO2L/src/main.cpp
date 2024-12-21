// ==============================
// M5StickC, ENV2でCO2データを取得し、シリアル通信で送信
// データリスト          対応する変数    単位
// 0:   デバイス名      DEVICE_NAME     -
// 1.   気温            temperature     ℃
// 2.   湿度            humidity        %
// 3.   CO2濃度         co2             ppm
// 参考:
// https://github.com/m5stack/M5Unit-ENV/blob/master/examples/Unit_CO2_M5StickC/Unit_CO2_M5StickC.ino
// ==============================

#include <BluetoothSerial.h>
#include <M5Unified.h>

#include "M5UnitENV.h"

// 定数定義
const char* DEVICE_NAME = "CO2_SENSOR";
const int SERIAL_BAUD = 115200;
const int DISPLAY_TIME = 3000;  // 更新間隔

// センサーインスタンス
SCD4X scd4x;
BluetoothSerial SerialBT;

float temp;
float hum;
float co2;

void UpdateInfo(float temp, float hum, float co2);

void setup() {
    auto cfg = M5.config();
    M5.begin(cfg);

    if (!scd4x.begin(&Wire, SCD4X_I2C_ADDR, 32, 33, 400000U)) {
        Serial.println("Couldn't find SCD4X");
        while (1) delay(1);
    }

    uint16_t error;
    // stop potentially previously started measurement
    error = scd4x.stopPeriodicMeasurement();
    if (error) {
        Serial.print("Error trying to execute stopPeriodicMeasurement(): ");
    }

    // Start Measurement
    error = scd4x.startPeriodicMeasurement();
    if (error) {
        Serial.print("Error trying to execute startPeriodicMeasurement(): ");
    }

    Serial.println("Waiting for first measurement... (5 sec)");

    M5.Lcd.setRotation(3);
    M5.Lcd.fillScreen(BLACK);
    M5.Lcd.setTextColor(WHITE);

    Serial.begin(SERIAL_BAUD);
    SerialBT.begin(DEVICE_NAME);
}

void loop() {
    // センサーからデータ読み取り
    if (scd4x.update()) {
        temp = scd4x.getTemperature();
        hum = scd4x.getHumidity();
        co2 = scd4x.getCO2();
    }

    UpdateInfo(temp, hum, co2);

    delay(DISPLAY_TIME);
}

void UpdateInfo(float temp, float hum, float co2) {
    // データ送信
    char data[100];
    sprintf(data, "%s\t%.2f\t%.2f\t%.1f", DEVICE_NAME, temp, hum, co2);
    Serial.println(data);
    SerialBT.println(data);

    // LCD表示更新
    M5.update();
    M5.Lcd.clear();
    M5.Lcd.setTextSize(1.5);
    M5.Lcd.setCursor(10, 10);
    M5.Lcd.print(DEVICE_NAME);
    M5.Lcd.setTextSize(1);
    M5.Lcd.setCursor(10, 30);
    M5.Lcd.printf("Temp: %.2f `C", temp);
    M5.Lcd.setCursor(10, 45);
    M5.Lcd.printf("Hum:  %.2f %%", hum);
    M5.Lcd.setCursor(10, 60);
    M5.Lcd.printf("CO2:  %.1f ppm", co2);
}