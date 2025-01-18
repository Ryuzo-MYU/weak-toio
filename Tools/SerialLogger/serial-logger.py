import serial
import serial.tools.list_ports
import datetime
import time
import sys

def list_com_ports():
    """利用可能なCOMポートの一覧を表示"""
    ports = serial.tools.list_ports.comports()
    if not ports:
        print("利用可能なCOMポートが見つかりません")
        return []
    
    print("\n利用可能なCOMポート:")
    for i, port in enumerate(ports):
        print(f"{i}: {port.device} - {port.description}")
    return ports

def create_log_file():
    """タイムスタンプ付きのログファイル名を生成"""
    timestamp = datetime.datetime.now().strftime("%Y%m%d_%H%M%S")
    return f"serial_log_{timestamp}.txt"

def log_serial_data(port_name, baud_rate=9600):
    """シリアルデータを受信してログファイルに記録"""
    log_file = create_log_file()
    
    try:
        with serial.Serial(port_name, baud_rate, timeout=1) as ser:
            print(f"\n{port_name}からの読み取りを開始します...")
            print(f"ログファイル: {log_file}")
            print("終了するにはCtrl+Cを押してください\n")
            
            with open(log_file, 'a', encoding='utf-8') as f:
                # ヘッダー情報を記録
                start_time = datetime.datetime.now()
                f.write(f"記録開始: {start_time}\n")
                f.write(f"ポート: {port_name}\n")
                f.write(f"ボーレート: {baud_rate}\n")
                f.write("-" * 50 + "\n")
                
                while True:
                    if ser.in_waiting > 0:
                        try:
                            # データを読み取り
                            data = ser.readline().decode('utf-8').strip()
                            timestamp = datetime.datetime.now()
                            
                            # ログに記録
                            log_entry = f"{timestamp}: {data}"
                            print(log_entry)
                            f.write(log_entry + "\n")
                            f.flush()  # バッファをすぐに書き込み
                            
                        except UnicodeDecodeError:
                            print("データのデコードに失敗しました")
                    
                    time.sleep(0.1)  # CPU負荷を下げるための短い待機
                    
    except serial.SerialException as e:
        print(f"エラー: {e}")
        sys.exit(1)
    except KeyboardInterrupt:
        print("\n記録を終了します")
        sys.exit(0)

def main():
    # 利用可能なポートを表示
    available_ports = list_com_ports()
    if not available_ports:
        return
    
    # ポート選択
    while True:
        try:
            port_index = int(input("\n使用するポートの番号を入力してください: "))
            if 0 <= port_index < len(available_ports):
                selected_port = available_ports[port_index].device
                break
            else:
                print("有効な番号を入力してください")
        except ValueError:
            print("数値を入力してください")
    
    # ボーレート設定
    while True:
        try:
            baud_rate = int(input("ボーレートを入力してください (デフォルト: 9600): ") or "9600")
            break
        except ValueError:
            print("有効な数値を入力してください")
    
    # ロギング開始
    log_serial_data(selected_port, baud_rate)

if __name__ == "__main__":
    main()
