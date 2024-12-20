#include <BluetoothSerial.h>
#include <M5Unified.h>

#include "M5UnitENV.h"

// 定数定義
const char* DEVICE_NAME = "ENV_SENSOR";
const uint32_t SERIAL_BAUD = 115200;
const int LOOP_DELAY = 1000;  // 1秒間隔信間隔(ms)

// オブジェクト初期化
BluetoothSerial SerialBT;
DHT12 dht;
BMP280 bmp;
unsigned long lastSendTime = 0;

void setup() {
    auto cfg = M5.config();
    M5.begin(cfg);
    Serial.begin(SERIAL_BAUD);
    SerialBT.begin(DEVICE_NAME);

    // LCD初期化
    M5.Lcd.setRotation(3);
    M5.Lcd.fillScreen(BLACK);
    M5.Lcd.setTextColor(WHITE);
    M5.Lcd.setTextSize(1);
    M5.Lcd.setCursor(10, 10);
    M5.Lcd.print(DEVICE_NAME);

    // センサーの初期化
    if (!dht.begin(&Wire, DHT12_I2C_ADDR, 32, 33, 400000U)) {
        Serial.println("DHT12 error");
        while (1) delay(1);
    }
    if (!bmp.begin(&Wire, BMP280_I2C_ADDR, 32, 33, 400000U)) {
        Serial.println("BMP280 error");
        while (1) delay(1);
    }

    // BMP280の設定
    bmp.setSampling(BMP280::MODE_NORMAL, BMP280::SAMPLING_X2,
                    BMP280::SAMPLING_X16, BMP280::FILTER_X16,
                    BMP280::STANDBY_MS_500);
}

void loop() {
    // センサーの更新
    dht.update();
    bmp.update();

    // データ送信文字列の作成
    char data[100];
    sprintf(data, "%s\t%.2f\t%.2f\t%.2f", DEVICE_NAME,
            bmp.cTemp,      // 気温（BMP280の値を使用）
            dht.humidity,   // 湿度
            bmp.pressure);  // 気圧

    // データの送信
    Serial.println(data);
    SerialBT.println(data);

    M5.update();

    // LCD表示の更新
    M5.Lcd.setCursor(0, 70);
    M5.Lcd.printf("Temp: %.1f C", bmp.cTemp);
    M5.Lcd.setCursor(10, 35);
    M5.Lcd.printf("Hum : %.1f %%", dht.humidity);
    M5.Lcd.setCursor(10, 50);
    M5.Lcd.printf("Pres: %.0f Pa", bmp.pressure);

    delay(LOOP_DELAY);
}