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

#include "M5UnitENV.h"

const char* DEVICE_NAME = "ENV_SENSOR";
const int SERIAL_BAUD = 115200;
const int SLEEP_SECONDS = 10;  // スリープ時間（秒）

// センサーインスタンス
SHT3X sht3x;
BMP280 bmp;
BluetoothSerial SerialBT;

void setup() {
    M5.begin();
    Serial.begin(SERIAL_BAUD);
    SerialBT.begin(DEVICE_NAME);

    Wire.begin();
    // センサー初期化待機
    M5.begin();
    if (!sht3x.begin(&Wire, SHT3X_I2C_ADDR, 32, 33, 400000U)) {
        while (1) delay(1);
    }

    if (!bmp.begin(&Wire, BMP280_I2C_ADDR, 32, 33, 400000U)) {
        while (1) delay(1);
    }
    /* Default settings from datasheet. */
    bmp.setSampling(BMP280::MODE_NORMAL,     /* Operating Mode. */
                    BMP280::SAMPLING_X2,     /* Temp. oversampling */
                    BMP280::SAMPLING_X16,    /* Pressure oversampling */
                    BMP280::FILTER_X16,      /* Filtering. */
                    BMP280::STANDBY_MS_500); /* Standby time. */
}

void loop() {
    if (!sht3x.update()) {
        while (1) delay(1);
    }
    if (!bmp.update()) {
        while (1) delay(1);
    }

    float temp = sht3x.cTemp;
    float hum = sht3x.humidity;
    float pa = bmp.readPressure();

    SerialBT.printf("%s\t%.2f\t%.2f\t%.1f\t%d%%\n", DEVICE_NAME, temp, hum, pa,
                    M5.Power.getBatteryLevel());

    M5.Lcd.setBrightness(1);
    esp_sleep_enable_timer_wakeup(sleep(SLEEP_SECONDS));
    esp_light_sleep_start();
}
