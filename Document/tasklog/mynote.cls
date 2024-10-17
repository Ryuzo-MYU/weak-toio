% ==============================
% 自分ノート用クラスファイル
% ==============================
\LoadClassWithOptions{jlreq}

% 数式
\RequirePackage{amsmath,amsfonts}
\RequirePackage{bm}
% 画像
\RequirePackage{graphicx}

% ソースコード
% ここから https://qiita.com/ta_b0_/items/2619d5927492edbb5b03
\RequirePackage{listings,jvlisting}
% ソースコード設定
\lstset{
  basicstyle={\ttfamily},
  identifierstyle={\small},
  keywordstyle={\small\bfseries},
  stringstyle={\small\ttfamily},
  frame={tb},
  breaklines=true,
  columns=[l]{fullflexible},
  numbers=left,
  numberstyle={\scriptsize},
  stepnumber=1,
  lineskip=-0.5ex
}

% URL
\RequirePackage{hyperref}

% listing環境のキャプション表示を「プログラム」に
\renewcommand{\lstlistingname}{プログラム}

% standaloneパッケージ umlを取り込むのに使ってる
\RequirePackage{standalone}
\RequirePackage{tikz}