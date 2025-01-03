# ToioConnecter.ConnectToio

`CubeScanner`，`CubeConnecter`を使って toio に接続する非同期処理．
Unity 上の toio と実機 toio のローカルネームを照合して，同名のものを接続する．

```mermaid
 graph TD
  Start([ボタンクリック]) --> CallConnctToio
  CallConnctToio --> ConnectToio
  ConnectToio --> GetToioComponets
  GetToioComponets --> |ToioComponentが0個| Error[エラー表示]
  GetToioComponets --> |ToioComponentが1個以上| 
```
