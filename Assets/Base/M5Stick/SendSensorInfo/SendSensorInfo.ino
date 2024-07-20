#include <M5StickC.h>
#include "BluetoothSerial.h"

// Bluetooth
// --------------------------------
// Bluetoothデバイスの識別名
String name = "M5-02";

// BluetoothSerialインスタンスを生成
BluetoothSerial SerialBT;

// LED
// --------------------------------
#define LED_PIN 10

// ボタン
// --------------------------------
int btnATactSwitch = false;  // ボタンA(真ん中のボタン)の押下状態(タクト)
int btnBTactSwitch = false;  // ボタンB(「M5」から見て右側のボタン)の押下状態(タクト)

int btnAToggleSwitch = false;  // ボタンA(真ん中のボタン)の押下状態(トグル)
int btnBToggleSwitch = false;  // ボタンB(「M5」から見て右側のボタン)の押下状態(トグル)

// MPU6886
// --------------------------------
// 加速度
float accX = 0;
float accY = 0;
float accZ = 0;

// ジャイロ
float gyroX = 0;
float gyroY = 0;
float gyroZ = 0;

// バッテリー情報
// --------------------------------
int8_t bat_charge_p;

void setup() {
  // シリアル通信の開始
  M5.begin();
  Serial.begin(115200);
  SerialBT.begin(name);  // Bluetoothシリアル通信の開始


  // ディスプレイにテキストを描画
  M5.Lcd.fillScreen(BLACK);
  M5.Lcd.setRotation(0);
  M5.Lcd.setTextFont(2);
  M5.Lcd.println(name);  // Bluetoothデバイス名を表示

  M5.MPU6886.Init();  // MPU6886(加速度センサ)を起動

  // LED有効化
  pinMode(LED_PIN, OUTPUT);

  // I2C初期化
  Wire.begin();

  // 出力情報のヘッダーを出力
  Serial.println("Device\tBtnA\tBtnB\tToggleA\tToggleB\tAccX(cm/s^2)\tAccY(cm/s^2)\tAccZ(cm/s^2)\tGyroX(deg/s)\tGyroY(deg/s)\tGyroZ(deg/s)\tBattery(%)");
  SerialBT.println("Device\tBtnA\tBtnB\tToggleA\tToggleB\tAccX(cm/s^2)\tAccY(cm/s^2)\tAccZ(cm/s^2)\tGyroX(deg/s)\tGyroY(deg/s)\tGyroZ(deg/s)\tBattery(%)");
}

void loop() {
  // バッテリー残量表示
  bat_charge_p = GetBatteryInfo();
  M5.Lcd.setCursor(0, 25);
  M5.Lcd.printf("Batt :%3d%%", bat_charge_p);

  M5.update();  // 本体のボタン状態更新 ※必ず最初に記入が必要

  Serial.printf("%s\t", name);
  SerialBT.printf("%s\t", name);

  // タクトスイッチの状態
  btnATactSwitch = M5.BtnA.isPressed();
  btnBTactSwitch = M5.BtnB.isPressed();
  Serial.printf("%d\t%d\t", btnATactSwitch, btnBTactSwitch);
  SerialBT.printf("%d\t%d\t", btnATactSwitch, btnBTactSwitch);

  // トグルスイッチ(仮想)の状態
  if (M5.BtnA.wasPressed()) {
    btnAToggleSwitch = !btnAToggleSwitch;
  }
  if (M5.BtnB.wasPressed()) {
    btnBToggleSwitch = !btnBToggleSwitch;
  }
  Serial.printf("%d\t%d\t", btnAToggleSwitch, btnBToggleSwitch);
  SerialBT.printf("%d\t%d\t", btnAToggleSwitch, btnBToggleSwitch);

  // Lチカ
  LEDBlink(btnAToggleSwitch);

  // 加速度の取得
  M5.MPU6886.getAccelData(&accX, &accY, &accZ);
  Serial.printf("%.2f\t%.2f\t%.2f\t", accX * 100, accY * 100, accZ * 100);
  SerialBT.printf("%.2f\t%.2f\t%.2f\t", accX * 100, accY * 100, accZ * 100);

  // ジャイロの取得
  M5.MPU6886.getGyroData(&gyroX, &gyroY, &gyroZ);
  Serial.printf("%.2f\t%.2f\t%.2f\t", gyroX, gyroY, gyroZ);
  SerialBT.printf("%.2f\t%.2f\t%.2f\t", gyroX, gyroY, gyroZ);

  Serial.println();
  SerialBT.println();
  delay(500);
}

// Lチカ
void LEDBlink(bool LEDon) {
  if (LEDon) {
    digitalWrite(LED_PIN, LOW);
  } else {
    digitalWrite(LED_PIN, HIGH);
  }
}

// バッテリー残量取得
int8_t GetBatteryInfo() {
  float vbat = M5.Axp.GetBatVoltage();
  int8_t battery_level = (vbat - 3.0) / (4.2 - 3.0) * 100;
  return constrain(battery_level, 0, 100);
}