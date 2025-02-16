# M5StickC_CO2L Project

このプロジェクトは、M5StickCとSCD41二酸化炭素センサを使用して、CO2濃度を測定するアプリケーションです。以下に、プロジェクトのセットアップ手順と使用方法を示します。

## セットアップ手順

1. **必要なライブラリのインストール**  
   プロジェクトに必要なライブラリをインストールします。`platformio.ini`ファイルに記載された依存関係を確認してください。

2. **ハードウェアの接続**  
   M5StickCとSCD41センサを適切に接続します。接続方法については、センサのデータシートを参照してください。

3. **コードのビルドとアップロード**  
   PlatformIOを使用して、`src/main.cpp`をビルドし、M5StickCにアップロードします。

## 使用方法

- プログラムを実行すると、SCD41センサが初期化され、CO2濃度が測定されます。
- 測定結果はシリアルモニタに表示されます。

## 注意事項

- センサの動作には時間がかかる場合があります。初回の測定には数分かかることがあります。
- 環境条件によって測定値が変動することがありますので、注意してください。

## ライセンス

このプロジェクトはMITライセンスの下で提供されています。詳細はLICENSEファイルを参照してください。