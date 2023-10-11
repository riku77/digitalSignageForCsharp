# こちらのリンクからアプリの説明動画を視聴できます！
https://www.youtube.com/watch?v=-wW4ORfJBuc

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

C#を選んだ理由として、uiをxmalで定義できるので作成しやすいことと<br>
Microsoftが開発したプログラミング言語なのでoffice365との連携がしやすいと思ったことと
windowsで実行できるインストーラーの作成をVitualStudioが公式でサポートしているからです<br>


# 苦戦したところ

## powerPointスライド(pptx)を画面にどうやって埋め込むか
最初は直接pptxを埋め込もうと思ったけど<br>
公式ドキュメント,Github,ChatGPTを見てもサンプルが出てこなかったので<br>
pptxを動画に変換して埋め込むという方式にしました

## 動画再生の途中でアプリがフリーズする
再生時間が1分を超えたあたりでフリーズする問題が発生しました<br>
原因の調査のために動画のループ再生を実装しているリポジトリをクローンして実行したところフリーズは発生しませんでした<br>
私が作成したプログラムではループ再生の定義をxamlに入れていたのですが参考にしたコードでは.csに定義していたので真似をしたところ<br>
動画再生の途中でフリーズする不具合の修正ができました。

## 可変長なプレイリスト再生の実装
1つの動画をループ再生するだけなら簡単だけど<br>
複数の動画ファイルをプレイリストにしてループ再生する必要性が発生した<br>
  (例 : 動画1が終了したら動画2を再生してそれが終わったらまた動画1に戻るのを繰り返す)<br>
動画を埋め込む画面領域は2つあるので、二次元のリストを作成<br>
その中にiniファイルに記述した動画ファイルのパスをaddして<br>
動画の再生が終了するとindexをインクリメントまたはリセットする方式に変更した

## 動画再生の終了判定が衝突する問題が発生
2つの動画を同時に再生しているのですが<br>
片方の動画が終了するともう片方の動画を終了したと判定される問題が発生した<br>
調べてみたところ下記のようにすることで終了判定を個別に実装することができた<br>
```
MediaElement player = sender as MediaElement;
      if (player == videoPlayer)
      {
          // ウィンドウ1の動画が終了した場合、次の動画を再生
          currentVideoIndex[0] += 1;
          player.Position = TimeSpan.FromSeconds(0);
          StartNextVideo(player, 0);
      }
      else if (player == videoPlayer2)
      {
          // ウィンドウ2の動画が終了した場合、次の動画を再生
          currentVideoIndex[1] += 1;
          player.Position = TimeSpan.FromSeconds(0);
          StartNextVideo(player, 1);
      }
```

## 相対パスが使用できない
相対パスをSystem.IO.Path.GetFullPathで絶対パスに変換する必要があって
pythonやjavaScriptにない仕様だったので苦戦した。


