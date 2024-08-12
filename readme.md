# 1. ppm toio WBS 兼企画設計いろいろ書

本稿は、卒業研究「ppm toio」の WBS である。\
WBS といいつつ、使うもの、補記等も記入するので、「この通りに作業進めればモチベ無いときでも仕事は進められるんじゃないですかね～」なドキュメントを目指している。

進捗表は[こちら](https://docs.google.com/spreadsheets/d/1U639k4QWcusb2OYTR_0ER-CtmLVivnQl1xtwEm9VkqM/edit?usp=sharing)\
チェックボックスを埋めていくと、なんだか仕事している気がする……

- [1. ppm toio WBS 兼企画設計いろいろ書](#1-ppm-toio-wbs-兼企画設計いろいろ書)
	- [1.1. ハードウェア準備](#11-ハードウェア準備)
		- [1.1.1. toio のセットアップ](#111-toio-のセットアップ)
			- [接続](#接続)
			- [Bluetooth 通信](#bluetooth-通信)
		- [1.1.2. M5StickC のセットアップ](#112-m5stickc-のセットアップ)
		- [1.1.3. HAT モジュールのセットアップ](#113-hat-モジュールのセットアップ)
		- [1.1.4. センサーの調達と接続](#114-センサーの調達と接続)
	- [1.2. ソフトウェア開発](#12-ソフトウェア開発)
		- [1.2.1. Unity プロジェクトの作成](#121-unity-プロジェクトの作成)
			- [1.2.1.1. プロジェクト構造の決定](#1211-プロジェクト構造の決定)
			- [1.2.1.2. 必要なパッケージのインストール](#1212-必要なパッケージのインストール)
		- [1.2.2. センサー受取クラスの実装](#122-センサー受取クラスの実装)
			- [1.2.2.1. toio からのデータ取得](#1221-toio-からのデータ取得)
			- [1.2.2.2. M5StickC からのデータ取得](#1222-m5stickc-からのデータ取得)
	- [todo list](#todo-list)

## 1.1. ハードウェア準備

本プロジェクトで利用するハードウェアは次の 3 点である

- PC
- toio
- M5Stick（以下「M5」）

### 1.1.1. toio のセットアップ

toio との接続には特別な手順は必要ない。手順は toio の Github を確認すれば問題なく進行できる。以下にお世話になっているリンクを示す。

- [toio Unity チュートリアル](https://github.com/morikatron/toio-sdk-for-unity/blob/main/docs/tutorials_basic.md)

- [toio SDK for Unity](https://github.com/morikatron/toio-sdk-for-unity/blob/main/docs/download_sdk.md)

- [toio Unity Package](https://github.com/morikatron/toio-sdk-for-unity/releases/)

#### 接続

Unity と toio の接続は、toio パッケージに入っている `Sample_ConnectType` というスクリプトを toio オブジェクトにアタッチすることで実現できる。

スクリプトの変数である `Connect Type` を `Auto` から `Real` に変えることで、仮想 toio の動きと同じことを、現実世界の(電源の入っている)toio もするようになる。

ただ接続が Bluetooth のほうが早いかもしれないので、おいおい比べていきたい。

#### Bluetooth 通信

Bluetooth 通信は `Assets/toio-sdk/Samples/Sample_Bluetooth/Sample_Bluetooth.unity` のサンプルプロジェクトで可能。ただ Bluetooth 以外の通信で基本的に可能？のようなので、必要ならやる。

### 1.1.2. M5StickC のセットアップ

~~(2024/07/14 時点) M5StickC のコンパイルが Arduino IDE でできなくなってます。\
原因不明。備え付けのスケッチ例をコンパイルしてもダメだったので、たぶんおれのせいではないはず。あるいはどれかのライブラリ or ボードが競合している？\
直るまでは M5 のプログラムはできないので、Unity 上でダミー M5 をつくるなりしてやり過ごしてください~~

> [!IMPORTANT]
> Arudino IDE を使う場合、ボードに注意！\
> ESP32 と M5Stack の 2 つのボードグループがありますが、M5StickC ライブラリが M5Stack 版ボードをベースに作られているようなので、ESP32 版使うとピン設定が合わずにコンパイルエラー起こします。

USB および Bluetooth を用いて、センサー情報をシリアル通信で送る。\
シリアルデータは次の形式で送信される

```Arduino
Serial.printf("%s\t", 任意のデータ); \\USBシリアル通信
SerialBT.printf("%s\t", 任意のデータ); \\Bluetoothシリアル通信

\\なお SerialBT という部分は、BluetoothSerialクラスのインスタンスなので、名前は好きに設定できる。
```

これを M5 で取得できるデータの分だけ書く\
BluetoothSerial クラスの起動とか、他にもいろいろやることはあるが、細かいことは`Assets\M5Stick\SendSensorInfo`の`SendSensorInfo.ino`を参照のこと。

コーディング、コンパイル、および書き込みには Arduino IDE を使用する。

### 1.1.3. HAT モジュールのセットアップ

toio および M5 の基本的なデータ送受信が完成次第、必要に応じてセットアップする。

### 1.1.4. センサーの調達と接続

現在使用可能な HAT モジュールは

- ENV.2
- ToF

の 2 点である。目下 HAT を使用して実装したいのは、二酸化炭素濃度測定機能の実装。けっこうあちこちで「ppm toio」のネタは話してきたので、なんとかして形にしたい。

## 1.2. ソフトウェア開発

本システムの俯瞰図を示す

![システム俯瞰図](system_overview.jpg)

こいつを実装できるように頑張る

### 1.2.1. Unity プロジェクトの作成

通常の 3D プロジェクトでよろし

#### 1.2.1.1. プロジェクト構造の決定

審議中
素材ごとに分けるか、シーンごとに分けるか
ただ最終的にはいろいろ抽象化したいので、基底階層にはすべてのシーンで利用されるクラスを、各シーンフォルダには各シーンフォルダ特有の機能やアクションを入れる。というふうにできると、なんかプロっぽい

```cmd
Assets
|--Base...ほとんどすべてのシーンで利用されるプログラム
|	|--M5Stick
|	|--toio
|
|--Library...おれが書いてない外部のプログラム
|	|--ble-plugin-unity
|	|--Plugins
|	|--toio-sdk
|	|--WebGLTemplates
|
|--Scenes...各シーンと追加スクリプトを配置
	|--ppm-measure
	:
	:
```

#### 1.2.1.2. 必要なパッケージのインストール

- toio
  - [UniTask のインストール](https://github.com/morikatron/toio-sdk-for-unity/blob/main/docs/download_sdk.md)
  - [toio SDK for Unity パッケージのインストール](https://github.com/morikatron/toio-sdk-for-unity/releases/)
- M5
  - なし

### 1.2.2. センサー受取クラスの実装

toio、M5 それぞれのデータ受取クラスを作る

#### 1.2.2.1. toio からのデータ取得

(2024.07.13 時点)不明。M5 のセットアップが終わったらやります。

#### 1.2.2.2. M5StickC からのデータ取得

前にやったのでできなくはないと思う。

> [!IMPORTANT]
> シリアル通信を可能にするには、Unity の設定を変える必要がある
>
> 編集(Edit) > プロジェクト設定(Project Setting) > プレイヤー(Player) > API 互換性レベル(API Compability)
> の部分を確認。
>
> デフォルトでは `.NET Standard 2.1` となっているのを、 `.NET Framework` に変更\
> これでよろし\
> [情報元](https://qiita.com/Ninagawa123/items/f6595dcf788dd316be8a)

シーン中に M5 のデータを受信させるオブジェクトを作成。そいつに

- M5DataReceiver
- SerialHundler

をアタッチ。

`SerialHundler` の M5Ports には、M5 と接続されているポート番号を登録。

## todo list

- 巡回システムの開発
- 異常検知システムの開発
- 異常情報の送信
- 異常情報の評価システムの開発
- 振る舞い決定システム開発
- 行動命令システム開発
