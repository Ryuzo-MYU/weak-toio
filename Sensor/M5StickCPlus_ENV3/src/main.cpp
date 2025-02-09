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

const char* DEVICE_NAME = "ENV-SENSOR-02";
const int SERIAL_BAUD = 115200;
const int SLEEP_SECONDS = 5;       // スリープ時間（秒）
const int DISPLAY_BRIGHTNESS = 0;  // ディスプレイ輝度

// センサーインスタンス
SHT3X sht3x;
QMP6988 qmp;
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
    if (!qmp.begin(&Wire, QMP6988_SLAVE_ADDRESS_L, 0, 26, 400000U)) {
        while (1) delay(1);
    }
}

void loop() {
    if (!sht3x.update()) {
        while (1) delay(1);
    }
    if (!qmp.update()) {
        while (1) delay(1);
    }

    float temp = sht3x.cTemp;
    float hum = sht3x.humidity;
    float pa = qmp.pressure;

    Serial.printf("%s\t%.2f\t%.2f\t%.1f\t%d\n", DEVICE_NAME, temp, hum, pa,
                  M5.Power.getBatteryLevel());
    SerialBT.printf("%s\t%.2f\t%.2f\t%.1f\t%d\n", DEVICE_NAME, temp, hum, pa,
                    M5.Power.getBatteryLevel());

    M5.Lcd.setBrightness(DISPLAY_BRIGHTNESS);
    M5.Lcd.powerSaveOn();

    esp_sleep_enable_timer_wakeup(sleep(SLEEP_SECONDS));
    esp_light_sleep_start();
}
