# アプリ実行方法

下記コマンドの実行を実行してリポジトリをローカルにクローン(保存)してください。
``` bash
git clone https://github.com/riku77/digitalSignageForCsharp.git
```

下記パスのexeをクリックするとアプリを実際に試すことが可能です
```
pptTester\bin\Debug\net6.0-windows\pptTester.exe
```

# 制作背景
担任の先生が、とある施設のデジタルサイネージが手動でやっていて<br>
非効率なのでシステム化してみたい人いないかという話をしていたので<br>
面白そうだと思って手を挙げたため<br>

下記の画像のような手動でのデジタルサイネージをシステム化しようというプロジェクトです。
![20230804_103413219](https://github.com/riku77/digitalSignageForCsharp/assets/117050555/4a37071c-e8f3-45ab-8833-21b3a82caafa)



# 技術選定
C#<br>
.net6<br>
wpfアプリケーション<br>
という構成でデジタルサイネージを作成しています。

C#を選んだ理由として、xmlでUiが作成しやすいことと<br>
Microsoftが開発したプログラミング言語なのでoffice365との連携がしやすいと思ったことと
windowsで実行できるインストーラーの作成をVitualStudioが公式でサポートしているからです<br>


# 苦戦したところ

## pdfを動画に変換する


## 相対パスが使用できない
相対パスをSystem.IO.Path.GetFullPathで絶対パスに変換する必要があって
pythonやjavaScriptにない仕様だったので苦戦した。
