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
const int SLEEP_SECONDS = 5;  // スリープ時間（秒）

// センサーインスタンス
SCD4X scd4x;
BluetoothSerial SerialBT;

void setup() {
    M5.begin();
    Wire.begin();
    Serial.begin(SERIAL_BAUD);
    SerialBT.begin(DEVICE_NAME);

    if (!scd4x.begin(&Wire, SCD4X_I2C_ADDR, 32, 33, 400000U)) {
        while (1) delay(1);
    }
    scd4x.stopPeriodicMeasurement();
    scd4x.startPeriodicMeasurement();
}

void loop() {
    if (scd4x.update()) {
        float temp = scd4x.getTemperature();
        float hum = scd4x.getHumidity();
        float co2 = scd4x.getCO2();

		SerialBT.printf("%s\t%.2f\t%.2f\t%.1f\t%d\n", DEVICE_NAME, temp, hum, co2, M5.Power.getBatteryLevel());
    }

    M5.Lcd.setBrightness(0);
    esp_sleep_enable_timer_wakeup(sleep(SLEEP_SECONDS));
    esp_light_sleep_start();
}
